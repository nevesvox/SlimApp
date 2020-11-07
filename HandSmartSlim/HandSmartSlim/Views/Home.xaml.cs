using HandSmartSlim.Models;
using HandSmartSlim.Util;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandSmartSlim.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            // Remove o botão de retornar da página
            NavigationPage.SetHasBackButton(this, false);

            // Inicializa o array de imagens de propaganda
            var images = new List<string>
            {
                "propaganda1","propaganda2","propaganda3"
            };

            Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
            {
                CarousselPropagandas.Position = (CarousselPropagandas.Position + 1) % images.Count;
                return true;
            }));

            // Adicionas as imagens no slide
            CarousselPropagandas.ItemsSource = images;

            nomeUsuario.Text = ClienteLogado.nome;
        }

        // Verifica o click do botão voltar do Android
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();

            //new thread
            Device.BeginInvokeOnMainThread(async () => {
                // Exibe o alerta
                var result = await this.DisplayAlert("Atenção", "Deseja sair do aplicativo?", "Sim", "Não");
                // Verifia o resultado
                if (result)
                {
                    Preferences.Clear();
                    // Retorna para a pagina de Login
                    await Navigation.PushAsync(new Login());
                }
            });
            // Quebra a função
            return true;
        }

        private async void AbrePaginaCompra(object sender, EventArgs e)
        {
            // Chama o Popup de Loading
            await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());
            // Chama a página de compra
            await Navigation.PushAsync(new Compra());
        }

        private async void AbrePaginaCartoes(object sender, EventArgs e)
        {
            // Chama o Popup de Loading
            await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());
            // Chama a página de compra
            await Navigation.PushAsync(new Cartoes());
        }

        private void SaiDoApp(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            // Chama o Popup de Loading
            await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());
            // Chama a página de compra
            await Navigation.PushAsync(new Extrato());
        }

        private async void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            // Chama o Popup de Loading
            await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());
            // Chama a página de compra
            await Navigation.PushAsync(new Configuracao(this));
        }

        // Função utilizada na atualização de Cadastro na rotina de Configurações
        public void AtualizaNome()
        {
            // Atualiza o nome do Usuario logado
            nomeUsuario.Text = ClienteLogado.nome;
        }
    }
}