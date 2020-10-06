using System;
using HandSmartSlim.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandSmartSlim
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var mainPage = new NavigationPage(new Login());
            NavigationPage.SetHasNavigationBar(this, false);
            MainPage = mainPage;
            //MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
