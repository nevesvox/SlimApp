using HandSmartSlim.Models;
using HandSmartSlim.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandSmartSlim.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompraSelecionadaPopUpView : PopupPage
    {
        ClienteService clienteService;
        ExtratoModel   CompraClicada;
        public CompraSelecionadaPopUpView(ExtratoModel compraClicada)
        {
            InitializeComponent();
            clienteService  = new ClienteService();
            // Recupera dados da Compra
            CompraClicada   = compraClicada;
            // Atualiza o valor total da Compra
            ValorTotal.Text         = CompraClicada.ValorTotal.ToString("C");
            CartaoUtilizado.Text    = CompraClicada.NumeroCartao;
            imgTipoPagamento.Source = CompraClicada.Pagamento;
            // Chama a função que carrega o List
            BuscaItensCompraSelecionada();
        }

        public void BuscaItensCompraSelecionada()
        {
            var resultBusca = clienteService.buscaItensCompra(CompraClicada.IdVenda);

            if (resultBusca.Tipo == "ok")
            {
                // Recupera array de Registros
                var arrayItens = resultBusca.Registros;
                // Cria o List de Itens
                List<ExtratoModel> ListaItens = new List<ExtratoModel>();
                // Percorre o array
                foreach (var item in arrayItens)
                {
                    // Adiciona itens no List
                    ListaItens.Add(new ExtratoModel() {
                        Qtde          = item.Qtde,
                        Descricao     = item.Descricao,
                        ValorUnitario = item.ValorUnitario,
                        ValorTotal    = item.ValorTotal
                    });
                }

                // Atualiza os itens do List
                listaItensCompra.ItemsSource = ListaItens;
            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // Fecha o PopUp
            PopupNavigation.Instance.PopAsync();
        }

        private async void listaItensCompra_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;

            // Recupera o Item Clicado
            var itemSelecionado = e.Item as ExtratoModel;

            await DisplayAlert("Descrição", itemSelecionado.Descricao, "OK");
        }
    }
}