using System.Windows;
using Prism.ViewModels;

namespace Prism.Views
{
    /// <summary>
    /// Main view.
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }


        #region Controls events

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            App.CloseApp();
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        #endregion

        private void Color_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte r, g, b;
            r = (byte)red.Value;
            g = (byte)green.Value;
            b = (byte)blue.Value;
            MainViewModel.devManager.SetEverything(r, g, b);
        }
    }
}