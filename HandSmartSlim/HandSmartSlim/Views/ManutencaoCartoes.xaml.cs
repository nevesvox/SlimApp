using HandSmartSlim.Models;
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
        public ManutencaoCartoes(int tipoOperacao, dynamic cartao = null)
        {
            InitializeComponent();

            // Verifica se o tipo de operação é Inclusão de Cartão
            if (tipoOperacao == 1)
            {
                btnSalvarCartao.IsVisible = true;
            }

            // Verifica se o tipo de operação é Edição de Cartão
            if (tipoOperacao == 2)
            {
                // Recupera o cartão
                cartao = cartao as CartaoModel;
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

                // Trava os campos
                entryNumeroCartao.IsReadOnly = true;
                entryNomeCartao.IsReadOnly   = true;
                entryValidade.IsReadOnly     = true;
                entryCodigo.IsReadOnly       = true;

                // Exibe o botão de excluir
                btnExcluirCartao.IsVisible = true;
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
    }
}