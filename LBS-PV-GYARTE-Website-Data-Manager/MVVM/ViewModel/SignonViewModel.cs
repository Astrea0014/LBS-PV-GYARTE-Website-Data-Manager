using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DataManager.Core;

namespace DataManager.MVVM.ViewModel
{
    class SignonViewModel : ObservableObject
    {
        private CancellationTokenSource _cts;

        public bool IsKeyStorePresent { get; }
        public Visibility KeyStoreButtonVisibility => IsKeyStorePresent ? Visibility.Visible : Visibility.Collapsed;

        private bool _isButtonsEnabled = true;
        public bool IsButtonsEnabled
        {
            get => _isButtonsEnabled;
            set
            {
                _isButtonsEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsManualInsertionButtonEnabled));
            }
        }

        private bool? _isManualInsertionButtonEnabled = null;
        public bool IsManualInsertionButtonEnabled
        {
            get => _isManualInsertionButtonEnabled ?? _isButtonsEnabled;
            set
            {
                _isManualInsertionButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isPopupOpen = false;

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PopupVisibility));
            }
        }

        public Visibility PopupVisibility => _isPopupOpen ? Visibility.Visible : Visibility.Collapsed;

        public string Key { get; set; }

        public RelayCommand LoadFromKeystoreCommand { get; }
        public RelayCommand ManualInsertionCommand { get; }
        public RelayCommand CancelConnectingCommand { get; }

        public SignonViewModel()
        {
            _cts = new CancellationTokenSource();

            IsKeyStorePresent = File.Exists("key.store");

            Key = "";

            LoadFromKeystoreCommand = new RelayCommand(async (o) =>
            {
                IsButtonsEnabled = false;

                using FileStream fs = File.OpenRead("key.store");

                long len = fs.Length;
                byte[] buffer = new byte[len];

                if (len > int.MaxValue)
                    throw new Exception("Length is larger than 32 bits; cannot continue execution.");

                fs.Read(buffer, 0, (int)len);

                byte[] unencrypted = ProtectedData.Unprotect(buffer, null, DataProtectionScope.CurrentUser);
                string key = Encoding.UTF8.GetString(unencrypted);

                IsPopupOpen = true;
                await Global.SubmitMasterAndAuthenticateAsync(key, _cts.Token);

                MainViewModel.Instance.CurrentView = new object();
            });

            ManualInsertionCommand = new RelayCommand(async (o) =>
            {
                IsButtonsEnabled = false;

                IsPopupOpen = true;
                await Global.SubmitMasterAndAuthenticateAsync(Key, _cts.Token);

                MainViewModel.Instance.CurrentView = new object();

                byte[] encoded = Encoding.UTF8.GetBytes(Key);
                byte[] encrypted = ProtectedData.Protect(encoded, null, DataProtectionScope.CurrentUser);

                using FileStream fs = File.OpenWrite("key.store");
                fs.Write(encrypted);
            });

            CancelConnectingCommand = new RelayCommand(o =>
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = new CancellationTokenSource();

                IsPopupOpen = false;
                IsButtonsEnabled = true;
                _isManualInsertionButtonEnabled = null;
                OnPropertyChanged(nameof(IsManualInsertionButtonEnabled));
            });
        }
    }
}
