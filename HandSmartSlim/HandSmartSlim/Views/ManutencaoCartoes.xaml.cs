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

namespace HandSmartSlim.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManutencaoCartoes : ContentPage
    {
        // Declara as váriaveis
        ClienteService clienteService;
        CartaoModel    BandeiraSelecionada;
        Cartoes        cartoesPage;
        int            idCartao;

        public ManutencaoCartoes(int tipoOperacao, dynamic cartao = null, Cartoes cartoesPg = null)
        {
            InitializeComponent();
            // Inicializa as Váriaveis
            clienteService      = new ClienteService();
            BandeiraSelecionada = new CartaoModel();
            cartoesPage         = cartoesPg;

            // Inicializa o array de bandeiras aceitas
            var bandeiras = new List<CartaoModel>
            {
                new CartaoModel {Bandeira = "Mastercard",       Imagem = "masterCard"},
                new CartaoModel {Bandeira = "Visa",             Imagem = "visaCard"},
                new CartaoModel {Bandeira = "American Express", Imagem = "americanCard"},
                new CartaoModel {Bandeira = "Hipercard",        Imagem = "hiperCard"},
                new CartaoModel {Bandeira = "Elo",              Imagem = "eloCard"}
            };

            pickerBandeiras.ItemsSource = bandeiras;

            // Verifica se o tipo de operação é Inclusão de Cartão
            if (tipoOperacao == 1)
            {
                btnSalvarCartao.IsVisible  = true;
                btnExcluirCartao.IsEnabled = false;
            }

            // Verifica se o tipo de operação é Edição de Cartão
            if (tipoOperacao == 2)
            {
                // Recupera o cartão
                cartao = cartao as CartaoModel;
                // Recupera o Id do Cartão
                idCartao               = cartao.IdCartao;
                // Atualiza card do cartão
                lblNomeCartao.Text     = cartao.NomeCartao;
                lblNumeroCartao.Text   = cartao.NumeroCartao;
                lblValidadeCartao.Text = cartao.Validade;
                imgTipoCartao.Source   = cartao.Imagem;

                // Atualiza campos
                entryNomeCartao.Text   = cartao.NomeCartao;
                entryNumeroCartao.Text = cartao.NumeroCartao;
                entryValidade.Text     = cartao.Validade;
                entryCodigo.Text       = "***";

                // Esconde o picker de bandeira e os Labels
                pickerBandeiras.IsVisible = false;
                lblBandeira.IsVisible     = false;

                // Trava os campos
                entryNumeroCartao.IsReadOnly = true;
                entryNomeCartao.IsReadOnly   = true;
                entryValidade.IsReadOnly     = true;
                entryCodigo.IsReadOnly       = true;

                // Exibe o botão de excluir
                btnExcluirCartao.IsVisible = true;
                btnSalvarCartao.IsEnabled = false;
            }
        }

        protected override void OnAppearing()
        {
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblNumeroCartao.Text = entryNumeroCartao.Text;
        }

        private void entryNomeCartao_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblNomeCartao.Text = entryNomeCartao.Text.ToUpper();
        }

        private void entryValidade_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblValidadeCartao.Text = entryValidade.Text;
        }

        private void pickerBandeiras_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Recupera a Bandeira Selecionada
            BandeiraSelecionada = pickerBandeiras.SelectedItem as CartaoModel;

            imgTipoCartao.Source = BandeiraSelecionada.Imagem;
        }

        private async void btnSalvarCartao_Clicked(object sender, EventArgs e)
        {
            if (VerificaCamposCartao())
            {
                // Chama o Popup de Loading
                await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());

                // Recupera a Bandeira Selecionada
                BandeiraSelecionada = pickerBandeiras.SelectedItem as CartaoModel;
                var result = clienteService.SalvaCartao(
                    BandeiraSelecionada.Bandeira,
                    BandeiraSelecionada.Imagem,
                    entryNumeroCartao.Text,
                    entryNomeCartao.Text,
                    entryValidade.Text,
                    int.Parse(entryCodigo.Text)
                );

                // Fecha o Popup de Loading
                try
                {
                    await PopupNavigation.Instance.PopAsync();
                }
                catch (Exception) { };

                // Verifica a resposta da requisição
                if (result.Tipo == "ok")
                {
                    // Exibe o alerta
                    await DisplayAlert("Tudo Certo!", "Cartão salvo com Sucesso!", "Aceitar");
                    // Chama a função que atualiza a Lista de Cartões
                    cartoesPage.AtualizaListaCartoes();
                    // Retorna a página anterior
                    await Application.Current.MainPage.Navigation.PopAsync();
                } else
                {
                    // Exibe o alerta
                    await DisplayAlert("Ops...", "Não foi possivel salvar o Cartão. Tente novamente!", "Aceitar");
                }
            }
        }

        private bool VerificaCamposCartao()
        {
            if (pickerBandeiras.SelectedIndex < 0)
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Selecione a Bandeira do Cartão para prosseguir!", "Aceitar");
                // Sai da função
                return false;
            }

            if (string.IsNullOrEmpty(entryNumeroCartao.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Insira o Número do Cartão para prosseguir!", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryNumeroCartao.Focus();
                // Sai da função
                return false;
            }

            if (entryNumeroCartao.Text.Length < 19)
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Número de Cartão Inválido. Verifique!", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryNumeroCartao.Focus();
                // Sai da função
                return false;
            }

            if (string.IsNullOrEmpty(entryNomeCartao.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Insira o Nome do Cartão para prosseguir!", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryNomeCartao.Focus();
                // Sai da função
                return false;
            }

            if (string.IsNullOrEmpty(entryValidade.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Insira a Validade do Cartão para prosseguir!", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryValidade.Focus();
                // Sai da função
                return false;
            }

            if (entryValidade.Text.Length < 5)
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Válidade do Cartão incorreta. Verifique!", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryValidade.Focus();
                // Sai da função
                return false;
            }

            if (string.IsNullOrEmpty(entryCodigo.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Insira o Código CVV do Cartão para prosseguir!", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryCodigo.Focus();
                // Sai da função
                return false;
            }

            return true;
        }

        private async void btnExcluirCartao_Clicked(object sender, EventArgs e)
        {
            bool resposta = await DisplayAlert("Atenção", "Deseja continuar com a exclusão do Cartão?", "Sim", "Não");

            if (resposta)
            { 
                // Chama o Popup de Loading
                await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());

                var result = clienteService.ExcluiCartao(idCartao);
            
                // Fecha o Popup de Loading
                try
                {
                    await PopupNavigation.Instance.PopAsync();
                }
                catch (Exception) { };

                // Verifica a resposta da requisição
                if (result.Tipo == "ok")
                {
                    // Exibe o alerta
                    await DisplayAlert("Tudo Certo!", "Cartão Excluido com Sucesso!", "Aceitar");
                    // Chama a função que atualiza a Lista de Cartões
                    cartoesPage.AtualizaListaCartoes();
                    // Retorna a página anterior
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    // Exibe o alerta
                    await DisplayAlert("Ops...", "Não foi possivel Excluir o Cartão. Tente novamente!", "Aceitar");
                }
            }
        }
    }
}