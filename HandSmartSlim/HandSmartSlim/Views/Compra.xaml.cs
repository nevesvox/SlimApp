using HandSmartSlim.Models;
using HandSmartSlim.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace HandSmartSlim.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Compra : ContentPage
    {
        CompraService compraService;
        public Compra()
        {
            InitializeComponent();
            compraService = new CompraService();
        }

        protected override void OnAppearing()
        {
            // Busca se o Cliente possui uma venda aberta
            var respostaProduto = compraService.BuscaVendaAberta();

            if (respostaProduto.Tipo == "ok")
            {
                // Recupera os Registros
                var arrayProdutos = respostaProduto.Registros;
                
                // Cria o list de Compra
                List<CompraModel> compra = new List<CompraModel>();

                // Percorre o array
                foreach (var produto in arrayProdutos)
                {  
                    // Adiciona os dados do array no List
                    compra.Add(new CompraModel() {
                        Qtde          = produto.Qtde,
                        Descricao     = produto.Descricao,
                        ValorUnitario = produto.ValorUnitario,
                        ValorTotal    = produto.ValorTotal
                    });
                }

                // Atualiza os registros do list
                listaCompras.ItemsSource = compra;

                // Recupera o valor total da compra
                ValorCompra.Text = arrayProdutos[0].ValorCompra.ToString("C");
            }

            // Fecha o Popup de Loading
            try
            {
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception) { };
        }

        private void OpenScanner(object sender, EventArgs e)
        {
            // Chama a função de do Scanner
            Scanner();
        }

        // Função responsável por escanear o produto
        public async void Scanner()
        {
            var ScannerPage = new ZXingScannerPage();

            ScannerPage.OnScanResult += (result) => {
                // Parar de escanear
                ScannerPage.IsScanning = false;

                // Alert com o código escaneado
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopAsync();
                    // Recebe a resposta da requisição
                    var respostaProduto = compraService.InsereProdutoVenda(result.Text);

                    if (respostaProduto.Tipo == "notOk")
                    {
                        // Exibe o alerta
                        DisplayAlert(
                            "Ops...",
                            "Produto não cadastrado, verifique com repositor ou caixa!",
                            "Aceitar"
                        );
                        // Quebra a função
                        return;

                    } else if (respostaProduto.Tipo == "ok")
                    {
                        // Recupera os Registros
                        var arrayProdutos = respostaProduto.Registros;

                        // Cria o list de Compra
                        List<CompraModel> compra = new List<CompraModel>();

                        // Percorre o array
                        foreach (var produto in arrayProdutos)
                        {
                            // Adiciona os dados do array no List
                            compra.Add(new CompraModel()
                            {
                                Qtde = produto.Qtde,
                                Descricao = produto.Descricao,
                                ValorUnitario = produto.ValorUnitario,
                                ValorTotal = produto.ValorTotal
                            });
                        }

                        // Atualiza os registros do list
                        listaCompras.ItemsSource = compra;

                        // Recupera o valor total da compra
                        ValorCompra.Text = arrayProdutos[0].ValorCompra.ToString("C");
                    }
                });
            };

            await Navigation.PushAsync(ScannerPage);
        }

        public void Clique_Item(Object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;

            var itemSelecionado = e.Item as CompraModel;

            // Exibe o alerta do item Selecionado
            ExibeAlertaClique(itemSelecionado);
        }

        public async void ExibeAlertaClique(CompraModel itemSelecionado)
        {
            string result = await DisplayPromptAsync(
                itemSelecionado.Descricao,
                "Altere a quantidade de produtos abaixo: ",
                "Alterar",
                "Excluir Produto",
                initialValue: itemSelecionado.Qtde.ToString(),
                keyboard: Keyboard.Numeric);

            // Verifica se o usuario clicou em Excluir Produto
            if (result == null)
            {
                return;
            }

            int valor;
            var resultado = int.TryParse(result, out valor);
            // Verifica se o valor inserido é igual ao valor atual
            if (valor == itemSelecionado.Qtde)
            {
                // Quebra a função
                return;
            }
            // Verifica resultado do alert
            if (resultado && valor > 0)
            {
                await DisplayAlert("Tudo Certo", "Valor válido", "Aceitar");
                return;
            } else
            {
                // Se não for um valor válido exibe o alerta
                await DisplayAlert("Atenção", "Valor inválido, tente novamente!", "Aceitar");
                // Chama a função novamente passando o item selecionado
                ExibeAlertaClique(itemSelecionado);
                return;
            }
        }
    }
}