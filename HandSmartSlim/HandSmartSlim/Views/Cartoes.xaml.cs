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
    public partial class Cartoes : ContentPage
    {
        public Cartoes()
        {
            InitializeComponent();
            // Inicializa o array de imagens de propaganda
            var cartoes = new List<CartaoModel>
            {
                new CartaoModel {NumeroCartao = "4564-4564-4564", Bandeira = "Mastercard"},
                new CartaoModel {NumeroCartao = "1111-1111-1111", Bandeira = "Mastercard"}
            };

            CarousselCartoes.ItemsSource = cartoes;
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
    }
}