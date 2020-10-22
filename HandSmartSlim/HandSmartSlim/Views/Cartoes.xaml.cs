using HandSmartSlim.Models;
using HandSmartSlim.Services;
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
        ClienteService clienteService;
        public Cartoes()
        {
            InitializeComponent();
            clienteService = new ClienteService();
            AtualizaListaCartoes();
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
            await Navigation.PushAsync(new ManutencaoCartoes(1, null , this));
        }

        private void listaCartoes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;

            // Recupera o Cartão Clicado
            var cartaoClicado = e.Item as CartaoModel;

            // Chama a Pagina de Manutenção de Cartão
            // passando o tipo de Operação - 2 (Edição de Cartão)
            Navigation.PushAsync(new ManutencaoCartoes(2, cartaoClicado, this));
        }

        public void AtualizaListaCartoes()
        {
            // Chama a função que realiza a busca de cartões
            var resultBuscaCartao = clienteService.BuscaCartaoCreditoCliente();

            // Verifica o resultado da busca
            if (resultBuscaCartao.Tipo == "ok")
            {
                // Recupera os Registros
                var arrayCartoes = resultBuscaCartao.Registros;

                // Cria o list de cartoes
                List<CartaoModel> listCartoes = new List<CartaoModel>();

                // Percorre o array
                foreach (var cartao in arrayCartoes)
                {
                    // Adiciona os dados do array no List
                    listCartoes.Add(new CartaoModel()
                    {
                        IdCartao = cartao.id,
                        NumeroCartao = cartao.numero,
                        NomeCartao = cartao.nome,
                        Imagem = cartao.imagem,
                        Validade = cartao.validade
                    });
                }
                // Insere dados na Lista
                listaCartoes.ItemsSource = listCartoes;
            }
        }
    }
}