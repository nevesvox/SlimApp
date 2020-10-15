using HandSmartSlim.Models;
using HandSmartSlim.Services;
using HandSmartSlim.Views;
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
    public partial class ItemSelecionadoPopUpView : PopupPage
    {
        CompraService compraService;
        int _IdProduto;
        int _quantidade;
        Compra pageCompra;

        public ItemSelecionadoPopUpView(string Produto, int Quantidade, int IdProduto, Compra compra)
        {
            InitializeComponent();
         
            // Recupera os valores
            compraService        = new CompraService();
            lblProduto.Text      = Produto;
            _quantidade          = Quantidade;
            entryQuantidade.Text = Quantidade.ToString();
            _IdProduto           = IdProduto;
            pageCompra           = compra;
        }
        protected override void OnAppearing()
        {
            // Seta o foco inicial para o campo
            entryQuantidade.Focus();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // Fecha o PopUp
            PopupNavigation.Instance.PopAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            // Fecha o PopUp
            await PopupNavigation.Instance.PopAsync();
            // Chama a função da View Compra que exclui o Item e atualiza o listView
            pageCompra.ExcluiItemVenda(_IdProduto);
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            var quantidadeItens = entryQuantidade.Text;

            int valor;
            var resultado = int.TryParse(quantidadeItens, out valor);
            // Verifica se o valor inserido é igual ao valor atual
            if (valor == _quantidade)
            {
                // Quebra a função
                return;
            }
            // Verifica resultado da conversão e se o valor é positivo
            if (resultado && valor > 0)
            {
                // Fecha o PopUp
                await PopupNavigation.Instance.PopAsync();

                // Chama a função da view Compras que atualiza a quantidade de produto e o List
                pageCompra.AtualizaQuantidadeItem(_IdProduto, valor);
                return;
            }
            else
            {
                // Se não for um valor válido exibe o alerta
                await DisplayAlert("Atenção", "Valor inválido, tente novamente!", "Aceitar");
                return;
            }
        }
    }
}