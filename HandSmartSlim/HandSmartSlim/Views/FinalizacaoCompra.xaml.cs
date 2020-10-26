using HandSmartSlim.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandSmartSlim.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FinalizacaoCompra : ContentPage
    {
        // Declara as variáveis
        ClienteService clienteService;
        dynamic IdCartao;
        string  NumeroCartao;
        string  ImagemCartao;
        int     IdCompra;
        float   ValorCompra;

        public FinalizacaoCompra(dynamic idCartao, string numeroCartao, string imagemCartao, int idCompra, float valorCompra)
        {
            InitializeComponent();
            clienteService = new ClienteService();

            // Atualiza as variáveis
            IdCartao     = idCartao;
            NumeroCartao = numeroCartao;
            ImagemCartao = imagemCartao;
            IdCompra     = idCompra;
            ValorCompra  = valorCompra;
            
            // Atualiza valores na View
            lblValorCompra.Text  = ValorCompra.ToString("C");
            lblNumeroCartao.Text = NumeroCartao;
            imgCartao.Source     = ImagemCartao;
        }

        protected override void OnAppearing()
        {
            // Fecha o Popup de Loading
            try
            {
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception) { };
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // Chama o Popup de Loading
            await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());
            
            // Chama a requisição de Finalização de Compra
            var result = clienteService.FinalizaCompraCliente(IdCartao, IdCompra, ValorCompra);

            // Remove o loading
            await PopupNavigation.Instance.PopAsync();

            // Verifica a resposta da requisição
            if (result.Tipo == "ok")
            {
                // Exibe o alerta
                await DisplayAlert("Compra realizada com Sucesso!", "Obrigado por sua Compra, você pode verificar suas compras realizadas atráves do seu Extrato", "Aceitar");

                // Retorna para a pagina Home
                await Navigation.PushAsync(new Home());
            } else
            {
                // Em caso de erro Exibe o alerta
                await DisplayAlert("Ops...", "Não foi possivel finalizar sua compra. Tente novamente!", "Aceitar");
            }
        }
    }
}