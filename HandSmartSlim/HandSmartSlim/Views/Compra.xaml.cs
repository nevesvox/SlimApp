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

        // Verifica o click do botão voltar do Android
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();

            //new thread
            Device.BeginInvokeOnMainThread(async () => {
                return;
            });
            // Quebra a função
            return true;
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
                        IdCompra      = produto.IdCompra,
                        Id            = produto.Id,
                        Qtde          = produto.Qtde,
                        Descricao     = produto.Descricao,
                        ValorUnitario = produto.ValorUnitario,
                        ValorTotal    = produto.ValorTotal,
                        ValorCompra   = produto.ValorCompra
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
                                IdCompra      = produto.IdCompra,
                                Id            = produto.Id,
                                Qtde          = produto.Qtde,
                                Descricao     = produto.Descricao,
                                ValorUnitario = produto.ValorUnitario,
                                ValorTotal    = produto.ValorTotal,
                                ValorCompra   = produto.ValorCompra
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

        public async void AtualizaQuantidadeItem(int IdProduto, int Quantidade)
        {
            // Chama o Popup de Loading
            await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());

            // Recebe a resposta da requisição
            var resposta = compraService.AtualizaQtdeProdutoVenda(IdProduto, Quantidade);

            if (resposta.Tipo == "ok")
            {
                // Recupera os Registros
                var arrayProdutos = resposta.Registros;

                // Cria o list de Compra
                List<CompraModel> compra = new List<CompraModel>();

                // Percorre o array
                foreach (var produto in arrayProdutos)
                {
                    // Adiciona os dados do array no List
                    compra.Add(new CompraModel()
                    {
                        IdCompra      = produto.IdCompra,
                        Id            = produto.Id,
                        Qtde          = produto.Qtde,
                        Descricao     = produto.Descricao,
                        ValorUnitario = produto.ValorUnitario,
                        ValorTotal    = produto.ValorTotal,
                        ValorCompra   = produto.ValorCompra
                    });
                }

                // Atualiza os registros do list
                listaCompras.ItemsSource = compra;

                // Recupera o valor total da compra
                ValorCompra.Text = arrayProdutos[0].ValorCompra.ToString("C");

                // Fecha o loading
                await PopupNavigation.Instance.PopAsync();

                // Exibe o alerta
                await DisplayAlert("Tudo Certo", "Produto atualizado!", "Aceitar");
            }
            else
            {
                // Fecha o loading
                await PopupNavigation.Instance.PopAsync();

                // Exibe o alerta
                await DisplayAlert("Ops...", "Ocorreram erros na atualização do Produto. Tente Novamente!", "Aceitar");
            }
        }

        public async void ExcluiItemVenda(int IdProduto)
        {
            // Chama o Popup de Loading
            await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());

            // Recebe a resposta da requisição
            var respostaExclusao = compraService.ExcluiProdutoVenda(IdProduto);

            if (respostaExclusao.Tipo == "ok")
            {
                // Recupera os Registros
                var arrayProdutos = respostaExclusao.Registros;

                // Cria o list de Compra
                List<CompraModel> compra = new List<CompraModel>();

                // Percorre o array
                foreach (var produto in arrayProdutos)
                {
                    // Adiciona os dados do array no List
                    compra.Add(new CompraModel()
                    {
                        IdCompra      = produto.IdCompra,
                        Id            = produto.Id,
                        Qtde          = produto.Qtde,
                        Descricao     = produto.Descricao,
                        ValorUnitario = produto.ValorUnitario,
                        ValorTotal    = produto.ValorTotal,
                        ValorCompra   = produto.ValorCompra
                    });
                }

                // Atualiza os registros do list
                listaCompras.ItemsSource = compra;

                // Recupera o valor total da compra
                ValorCompra.Text = arrayProdutos[0].ValorCompra.ToString("C");
                

            }
            else
            {
                // Atualiza os registros do list
                listaCompras.ItemsSource = null;
                // Recupera o valor total da compra
                ValorCompra.Text = "";
            }

            // Fecha o loading
            await PopupNavigation.Instance.PopAsync();

            // Exibe o alerta
            await DisplayAlert(
                "Tudo Certo!",
                "Produto removido da Venda!",
                "Aceitar"
            ); 
        }

        public void Clique_Item(Object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;
            // Recupera o Item Clicado
            var itemSelecionado = e.Item as CompraModel;

            // Chama o Popup de alteração do Produto
            PopupNavigation.Instance.PushAsync(new ItemSelecionadoPopUpView(
                itemSelecionado.Descricao,
                itemSelecionado.Qtde,
                itemSelecionado.Id,
                this
            ));
        }

        public async void ExibeAlertaClique(CompraModel itemSelecionado)
        {
            Console.WriteLine(itemSelecionado.Id);
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
                // Chama o Popup de Loading
                await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());

                // Recebe a resposta da requisição
                var respostaExclusao = compraService.ExcluiProdutoVenda(itemSelecionado.Id);
                
                if (respostaExclusao.Tipo == "ok")
                {
                    // Recupera os Registros
                    var arrayProdutos = respostaExclusao.Registros;

                    // Cria o list de Compra
                    List<CompraModel> compra = new List<CompraModel>();

                    // Percorre o array
                    foreach (var produto in arrayProdutos)
                    {
                        // Adiciona os dados do array no List
                        compra.Add(new CompraModel()
                        {
                            IdCompra      = produto.IdCompra,
                            Id            = produto.Id,
                            Qtde          = produto.Qtde,
                            Descricao     = produto.Descricao,
                            ValorUnitario = produto.ValorUnitario,
                            ValorTotal    = produto.ValorTotal,
                            ValorCompra   = produto.ValorCompra
                        });
                    }

                    // Atualiza os registros do list
                    listaCompras.ItemsSource = compra;

                    // Recupera o valor total da compra
                    ValorCompra.Text = arrayProdutos[0].ValorCompra.ToString("C");

                    // Fecha o loading
                    await PopupNavigation.Instance.PopAsync();
                    // Exibe o alerta
                    await DisplayAlert(
                        "Tudo Certo!",
                        "Produto removido da Venda!",
                        "Aceitar"
                    );
                } else
                {
                    // Fecha o loading
                    await PopupNavigation.Instance.PopAsync();
                    // Exibe o alerta
                    await DisplayAlert(
                        "Ops...",
                        "Não foi possivel remover o Produto da Venda, verifique com repositor ou caixa!",
                        "Aceitar"
                    );
                }
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // Verifica se o List possui itens
            if (listaCompras.ItemsSource != null)
            {
                // Recupera os itens da Lista de Compras
                var arrayCompra = (listaCompras.ItemsSource as ICollection<CompraModel>).ToArray();

                // Chama o Popup de Loading
                await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());

                // Chama a página de Cartoes
                // Passando o tipo de operação 1 - Escolha de cartão
                await Navigation.PushAsync(new Cartoes(1, arrayCompra[0].IdCompra, arrayCompra[0].ValorCompra));

            }
            else
            {
                // Se não possuir nenhum item na lista
                await DisplayAlert("Atenção", "Nenhum item na Lista de Compras", "Aceitar");
            }
        }
    }
}