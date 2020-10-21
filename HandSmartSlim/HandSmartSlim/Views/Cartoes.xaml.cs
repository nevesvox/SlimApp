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
        public Cartoes()
        {
            InitializeComponent();
            // Inicializa o array de imagens de propaganda
            var cartoes = new List<CartaoModel>
            {
                new CartaoModel {NumeroCartao = "4564 4564 4564 4564", Bandeira = "Mastercard", NomeCartao = "GABRIEL NEVES", Imagem = "masterCard", Validade = "01/25"},
                new CartaoModel {NumeroCartao = "1111 1111 1111 1111", Bandeira = "Mastercard", NomeCartao = "GABRIEL NEVES", Imagem = "visaCard", Validade = "03/25"},
                new CartaoModel {NumeroCartao = "1111 1111 1111 1111", Bandeira = "Mastercard", NomeCartao = "GABRIEL NEVES", Imagem = "visaCard", Validade = "03/25"},
                new CartaoModel {NumeroCartao = "1111 1111 1111 1111", Bandeira = "Mastercard", NomeCartao = "GABRIEL NEVES", Imagem = "visaCard", Validade = "03/25"}
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
            // Chama a página de Manutenção de Cartoes 
            // passando o tipo de Operação( 1 - Inclusão de Cartão)
            await Navigation.PushAsync(new ManutencaoCartoes(1));
        }

        private void listaCartoes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;

            // Recupera o Cartão Clicado
            var cartaoClicado = e.Item as CartaoModel;

            // Chama a Pagina de Manutenção de Cartão
            // passando o tipo de Operação - 2 (Edição de Cartão)
            Navigation.PushAsync(new ManutencaoCartoes(2, cartaoClicado));
        }
    }
}