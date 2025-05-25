using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataManager.MVVM.View
{
    /// <summary>
    /// Interaction logic for SignonView.xaml
    /// </summary>
    public partial class SignonView : UserControl
    {
        public SignonView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ManualInsertionButton.IsEnabled = false;

            Storyboard storyboard = new Storyboard();

            DoubleAnimation opacityAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(300)));
            storyboard.Children.Add(opacityAnimation);
            Storyboard.SetTargetName(opacityAnimation, LoadButton.Name);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(OpacityProperty));

            DoubleAnimation boxAnimation1 = new DoubleAnimation(20, new Duration(TimeSpan.FromMilliseconds(300)));
            DoubleAnimation boxAnimation2 = new DoubleAnimation(20, new Duration(TimeSpan.FromMilliseconds(300)));

            storyboard.Children.Add(boxAnimation1);
            storyboard.Children.Add(boxAnimation2);

            Storyboard.SetTargetName(boxAnimation1, ManualInsertionTextBox.Name);
            Storyboard.SetTargetName(boxAnimation2, ManualInsertionSubmitButton.Name);

            Storyboard.SetTargetProperty(boxAnimation1, new PropertyPath(HeightProperty));
            Storyboard.SetTargetProperty(boxAnimation2, new PropertyPath(HeightProperty));

            ThicknessAnimation borderAnimation = new ThicknessAnimation(new Thickness(0), new Duration(TimeSpan.FromMilliseconds(300)));
            storyboard.Children.Add(borderAnimation);
            Storyboard.SetTargetName(borderAnimation, ManualInsertionButton.Name);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(BorderThicknessProperty));

            BeginStoryboard(storyboard);
        }
    }
}
