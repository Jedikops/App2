using System;
using App2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            
            
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new MainPageViewModel();
            Bindings.Update();
        }

        public MainPageViewModel ViewModel { get; private set; }

    }
}
