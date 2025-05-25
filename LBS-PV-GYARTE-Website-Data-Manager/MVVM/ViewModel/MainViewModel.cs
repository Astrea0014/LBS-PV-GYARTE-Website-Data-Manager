using DataManager.Core;

namespace DataManager.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public static MainViewModel Instance; // TERRIBLE solution but will have to do for now....

        private readonly SignonViewModel SignonViewModel;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Instance = this; // Absolutely TERRIBLE

            SignonViewModel = new SignonViewModel();

            _currentView = SignonViewModel;
        }
    }
}
