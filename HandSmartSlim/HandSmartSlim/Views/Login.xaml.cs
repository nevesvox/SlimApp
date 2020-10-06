using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using HandSmartSlim.Services;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace HandSmartSlim.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        ClienteService clienteService;

        public Login()
        {
            InitializeComponent();
            // Remove a guia de navegação da página
            NavigationPage.SetHasNavigationBar(this, false);
            clienteService = new ClienteService();
        }

        protected override void OnAppearing()
        {
            // Verifica Manter Conectado
            var manterConectado = Preferences.Get("manterConectado", false);

            if (manterConectado)
            {
                // Chama o Popup de Loading
                PopupNavigation.Instance.PushAsync(new LoadingPopUpView());
                var emailConectado = Preferences.Get("email", "");
                var senhaConectado = Preferences.Get("senha", "");

                // Chama a requisição
                var cliente = clienteService.RealizaLogin(emailConectado, senhaConectado);

                // Verifica a resposta
                if (cliente.Result == "ok")
                {
                    //Chama a página Home
                    Navigation.PushAsync(new Home());
                    // Fecha o Popup de Loading
                    PopupNavigation.Instance.PopAsync();
                }
            }
        }

        private async void Button_Login(object sender, EventArgs e)
        {
            // Verifica os campos
            if (VerificaCamposLogin())
            {
                // Verifica a conexão com a internet
                if (!CrossConnectivity.Current.IsConnected)
                {
                    bool answer = await DisplayAlert("Atenção",
                        "Não foi possivel conectar-se com a internet, verifique sua conexão e tente novamente!", 
                        "Cancelar", "Tentar Novamente");
                    
                    return;
                }
                // Chama o Popup de Loading
                await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());

                // Chama a requisição
                var cliente = clienteService.RealizaLogin(EmailEntry.Text, SenhaEntry.Text);
                
                // Verifica a resposta
                if (cliente.Result == "ok")
                {
                    // Verifica definição de login automático
                    if (manterConectado.IsChecked)
                    {
                        // Salva definição
                        Preferences.Set("manterConectado", true);
                        Preferences.Set("email", EmailEntry.Text);
                        Preferences.Set("senha", SenhaEntry.Text);
                    }
                    //Chama a página Home
                    await Navigation.PushAsync(new Home());
                    // Fecha o Popup de Loading
                    await PopupNavigation.Instance.PopAsync();
                } else
                {
                    // Fecha o Popup de Loading
                    await PopupNavigation.Instance.PopAsync();
                    // Exibe o alerta
                    await DisplayAlert("Ops...", "Usúario ou Senha incorreto!", "Aceitar");
                    // Manda o foco para o campo de e-mail
                    EmailEntry.Focus();
                    // Quebra a função
                    return;
                }
            }
            
        }

        // Função responsável por válidar os campos de login
        private bool  VerificaCamposLogin()
        {
           
            if (string.IsNullOrEmpty(EmailEntry.Text) && string.IsNullOrEmpty(SenhaEntry.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha os campos para realizar o Login", "Aceitar");
                // Manda o foco para o campo de e-mail
                EmailEntry.Focus();
                // Sai da função
                return false;
            }

            // Verifica se o campo email foi preenchido
            if (string.IsNullOrEmpty(EmailEntry.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha o campo de E-mail", "Aceitar");
                // Manda o foco para o campo de e-mail
                EmailEntry.Focus();
                // Sai da função
                return false;
            }

            // Verifica se o campo de senha foi preenchido
            if (string.IsNullOrEmpty(SenhaEntry.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha o campo de Senha", "Aceitar");
                // Manda o foco para o campo de e-mail
                SenhaEntry.Focus();
                // Sai da função
                return false;
            }
            
            // Caso os campos estiverem corretos
            return true;
        }

        private async void Button_Cadastro(object sender, EventArgs args)
        {
            // Chama a página de cadastro
            await Navigation.PushAsync(new Cadastro());
        }
    }
}