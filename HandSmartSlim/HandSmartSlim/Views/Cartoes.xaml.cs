using HandSmartSlim.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandSmartSlim.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cartoes : ContentPage
    {
        CartaoModel CartaoSelecionado;
        public Cartoes()
        {
            InitializeComponent();
            // Inicializa o array de imagens de propaganda
            var cartoes = new List<CartaoModel>
            {
                new CartaoModel {NumeroCartao = "4564-4564-4564", Bandeira = "Mastercard", NomeCartao = "Gabriel Neves", Imagem = "masterCard", Validade = "01/25"},
                new CartaoModel {NumeroCartao = "1111-1111-1111", Bandeira = "Mastercard", NomeCartao = "Gabriel Neves", Imagem = "visaCard", Validade = "03/25"}
            };

            listaCartoes.ItemsSource = cartoes;
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
            
            // Chama a página de Manutenção de Cartoes 
            // passando o tipo de Operação( 1 - Inclusão de Cartão)
            await Navigation.PushAsync(new ManutencaoCartoes(1));
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Console.WriteLine(CartaoSelecionado);
            //var teste = CarousselCartoes.CurrentItem as CartaoModel;

            //Console.WriteLine(teste);
        }

        public ICommand ItemChangedCommand => new Command<CartaoModel>((item) =>
        {
            Console.WriteLine(item);
        });
    }
}