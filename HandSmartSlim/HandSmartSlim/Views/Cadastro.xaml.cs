using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandSmartSlim.Services;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace HandSmartSlim.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cadastro : ContentPage
    {
        ClienteService clienteService;

        public Cadastro()
        {
            InitializeComponent();
            clienteService = new ClienteService();
        }

        public bool VerificaCamposCadastro()
        {
            // Verifica se o campo Nome foi preenchido
            if (string.IsNullOrEmpty(entryNome.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha o campo de Nome Completo", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryNome.Focus();
                // Sai da função
                return false;
            }
            // Verifica se o campo CPF foi preenchido
            if (string.IsNullOrEmpty(entryCPF.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha o campo de CPF", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryCPF.Focus();
                // Sai da função
                return false;
            }
            // Verifica se o campo Telefone foi preenchido
            if (string.IsNullOrEmpty(entryTelefone.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha o campo de Telefone", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryTelefone.Focus();
                // Sai da função
                return false;
            }
            // Verifica se o campo Email foi preenchido
            if (string.IsNullOrEmpty(entryEmail.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha o campo de Email", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryEmail.Focus();
                // Sai da função
                return false;
            }
            // Verifica se o campo Email Confirmacao foi preenchido
            if (string.IsNullOrEmpty(entryConfirmacaoEmail.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha o campo de Confirmação de E-mail", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryConfirmacaoEmail.Focus();
                // Sai da função
                return false;
            }
            if (entryEmail.Text != entryConfirmacaoEmail.Text)
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Os e-mails inseridos não são iguais, verifique novamente!", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryEmail.Focus();
                // Sai da função
                return false;
            }
            // Verifica se o campo Email Confirmacao foi preenchido
            if (string.IsNullOrEmpty(entrySenha.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha o campo de Senha", "Aceitar");
                // Manda o foco para o campo de e-mail
                entrySenha.Focus();
                // Sai da função
                return false;
            }
            // Verifica se o campo Email Confirmacao foi preenchido
            if (string.IsNullOrEmpty(entryConfirmacaoSenha.Text))
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "Preencha o campo de Confirmação de Senha", "Aceitar");
                // Manda o foco para o campo de e-mail
                entryConfirmacaoSenha.Focus();
                // Sai da função
                return false;
            }
            if (entrySenha.Text != entryConfirmacaoSenha.Text)
            {
                // Exibe o alerta
                DisplayAlert("Ops...", "As senhas inseridas não são iguais, verifique novamente!", "Aceitar");
                // Manda o foco para o campo de e-mail
                entrySenha.Focus();
                // Sai da função
                return false;
            }

            return true;

        }

        private async void Button_Confirmar(object sender, EventArgs e)
        {
            if (VerificaCamposCadastro())
            {
                // Chama o Popup de Loading
                await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());

                // Envia a requisição de cadastro para a API
                var result = clienteService.CadastraCliente(
                    entryNome.Text,
                    entryCPF.Text,
                    entryTelefone.Text,
                    entryEmail.Text,
                    entrySenha.Text
                );

                // Verifica o resultado da requisição
                if (result.Tipo == "ok")
                {
                    // Fecha o Popup de Loading
                    await PopupNavigation.Instance.PopAsync();
                    // Exibe o alerta
                    await DisplayAlert("Tudo Certo!", "Seu cadastro foi realizado com sucesso, agora você pode realizar o login e realizar suas compras!", "Aceitar");
                    await Navigation.PopAsync();
                    return;

                } else if (result.Tipo == "notOk")
                {
                    // Fecha o Popup de Loading
                    await PopupNavigation.Instance.PopAsync();
                    // Exibe o alerta
                    await DisplayAlert("Ops...", "Email já cadastrado no sistema!", "Aceitar");
                    // Foca o email
                    entryEmail.Focus();
                    return;
                }
            }
        }
    }
}