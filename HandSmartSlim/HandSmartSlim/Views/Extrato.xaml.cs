using HandSmartSlim.Models;
using HandSmartSlim.Popups;
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
    public partial class Extrato : ContentPage
    {
        ClienteService clienteService;
        public Extrato()
        {
            InitializeComponent();
            clienteService = new ClienteService();
            BuscaVendasCliente();
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

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();

            //new thread
            Device.BeginInvokeOnMainThread(async () => { });
            // Quebra a função
            return true;
        }

        public void BuscaVendasCliente() {

            // Realiza a busca das Ultimas Compras
            var result = clienteService.buscaUltimasComprasCliente();
            
            // Verifica a resposta da requisição
            if (result.Tipo == "ok")
            {
                // Recupera os registros
                var ultimasCompras = result.Registros;

                // Cria uma lista de Extrato.
                List<ExtratoModel> Extrato = new List<ExtratoModel>();

                // Percorre o array
                foreach (var Compra in ultimasCompras)
                {
                    Extrato.Add(new ExtratoModel() { 
                        IdVenda      = Compra.IdCompra,
                        DataCompra   = Compra.Data,
                        Pagamento    = Compra.Imagem,
                        ValorTotal   = Compra.TotalCompra,
                        NumeroCartao = Compra.NumeroCartao
                    });
                }
                // Atualiza a Lista
                ListaUltimasCompras.ItemsSource = Extrato;
            }
        }

        private void ListaUltimasCompras_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;

            // Recupera o Cartão Clicado
            var compraClicada = e.Item as ExtratoModel;

            // Chama o Popup de alteração do Produto
            PopupNavigation.Instance.PushAsync(new CompraSelecionadaPopUpView(
                compraClicada
            ));
        }
    }
}