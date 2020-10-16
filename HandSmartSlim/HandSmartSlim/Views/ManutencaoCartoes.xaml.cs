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
        public ManutencaoCartoes(int tipoOperacao)
        {
            InitializeComponent();

            if (tipoOperacao == 1)
            {
                btnSalvarCartao.IsVisible = true;
            }

            if (tipoOperacao == 2)
            {
                lblNomeCartao.Text = "GABRIEL NEVES MACIEL";
                lblNumeroCartao.Text = "4444-4444-4444-4444";
                lblValidadeCartao.Text = "10/21";
                imgTipoCartao.Source = "visaCard";
            }
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