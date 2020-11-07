using HandSmartSlim.Models;
using HandSmartSlim.Services;
using HandSmartSlim.Util;
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
    public partial class Configuracao : ContentPage
    {
        ClienteService clienteService;
        Home           homePage;
        // Cria um List de Estados
        List<EstadoModel> Estados = new List<EstadoModel>();
        // Cria o List de Cidades
        List<CidadeModel> Cidades = new List<CidadeModel>();
        public Configuracao(Home homepg)
        {
            InitializeComponent();

            // Inicializa as Váriaveis
            clienteService = new ClienteService();
            homePage       = homepg;

            // Realiza a Busca dos Estados
            BuscaEstados();

            // Atualiza as váriaveis
            lblNomeUsuario.Text  = ClienteLogado.nome;
            lblCpfUsuario.Text   = ClienteLogado.cpf;
            lblEmailUsuario.Text = ClienteLogado.email;
            lblTelefone.Text     = ClienteLogado.telefone;
            entryCep.Text        = ClienteLogado.cep;
            entryLogradouro.Text = ClienteLogado.numero;
            lblRua.Text          = ClienteLogado.rua;

            // Verifica se o usuario possui endereço cadastrado
            if (ClienteLogado.estado != null)
            {
                // Inicializa variaveis do estado
                var countEstado = 0;
                var indexEstado = 0;
                // Percorre o List de Estados para encontrar o index do Estado selecionado
                Estados.ForEach(delegate (EstadoModel estado)
                {
                    if (estado.EstadoId.ToString() == ClienteLogado.estado)
                    {
                        indexEstado = countEstado;
                    }
                    countEstado++;
                });

                // Atualiza o Estado Selecionado
                pickerEstados.SelectedIndex = indexEstado;

                // Chama a função que carrega a lista de Cidades
                CarregaCidades(ClienteLogado.estado);

                // Inicializa as variaveis da cidade
                var countCidade = 0;
                var indexCidade = 0;
                // Percorre o List de Cidade para encontrar o index da Cidade selecionada
                Cidades.ForEach(delegate (CidadeModel cidade)
                {
                    if (cidade.CidadeId.ToString() == ClienteLogado.cidade)
                    {
                        indexCidade = countCidade;
                    }
                    countCidade++;
                });

                // Atualiza os Index selecionados
                pickerEstados.SelectedIndex = indexEstado;
                pickerCidades.SelectedIndex = indexCidade;
            }


            // Insere ao picker Estados a ação
            pickerEstados.SelectedIndexChanged += pickerEstados_SelectedIndexChanged;

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

        public void BuscaEstados()
        {
            // Realiza a Busca dos Estados
            var result = clienteService.buscaEstados();

            // Verifica a resposta
            if (result.Tipo == "ok")
            {
                // Recupera os Registros
                var arrayEstados = result.Registros;
                
                // Percorre o array
                foreach (var estado in arrayEstados)
                {
                    Estados.Add(new EstadoModel()
                    {
                        EstadoId   = estado.Id,
                        EstadoNome = estado.Nome,
                        EstadoUF   = estado.UF
                    });
                }
                // Atualiza o Picker
                pickerEstados.ItemsSource = Estados;

            }
        }

        private async void pickerEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chama o Popup de Loading
            await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());
            // Recupera o Estado Selecionado
            var EstadoSelecionado = pickerEstados.SelectedItem as EstadoModel;

            // Busca as cidades do estado
            var result = clienteService.buscaCidadesEstado(EstadoSelecionado.EstadoId.ToString());

            // Verifica a resposta da requisição
            if (result.Tipo == "ok")
            {
                // Limpa o List de Cidades e o Picker
                Cidades.Clear();
                pickerCidades.ItemsSource = null;

                // Recupera o array de registros
                var arrayCidades = result.Registros;
                
                // Percorre o array adicionando ao List
                foreach (var cidade in arrayCidades)
                {
                    Cidades.Add(new CidadeModel()
                    {
                        CidadeId   = cidade.Id,
                        CidadeNome = cidade.Nome
                    });
                }
                // Atualiza o Picker
                pickerCidades.ItemsSource = Cidades;
                // Ativa os campos
                pickerCidades.IsEnabled   = true;
                lblRua.IsEnabled          = true;
                entryLogradouro.IsEnabled = true;
                entryCep.IsEnabled        = true;
            }

            // Fecha o Popup de Loading
            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception) { };
        }

        private void CarregaCidades(string EstadoId)
        {
            // Busca as cidades do estado
            var result = clienteService.buscaCidadesEstado(EstadoId);

            if (result.Tipo == "ok")
            {
                // Recupera o array de registros
                var arrayCidades = result.Registros;

                // Percorre o array adicionando ao List
                foreach (var cidade in arrayCidades)
                {
                    Cidades.Add(new CidadeModel()
                    {
                        CidadeId = cidade.Id,
                        CidadeNome = cidade.Nome
                    });
                }
                // Atualiza o Picker
                pickerCidades.ItemsSource = Cidades;
                // Ativa os campos
                pickerCidades.IsEnabled   = true;
                lblRua.IsEnabled          = true;
                entryLogradouro.IsEnabled = true;
                entryCep.IsEnabled        = true;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // Inicializa as Váriaveis
            string idEstado = null;
            string idCidade = null;
            // Verifica se foi selecionado algum estado
            if (pickerEstados.SelectedIndex != -1)
            {
                // Recupera o Estado Selecionado
                var EstadoSelecionado = pickerEstados.SelectedItem as EstadoModel;
                idEstado = EstadoSelecionado.EstadoId.ToString();
            }
            // Verifica se foi selecionado alguma cidade
            if (pickerCidades.SelectedIndex != -1)
            {
                // Recupera a Cidade Selecionada
                var CidadeSelecionada = pickerCidades.SelectedItem as CidadeModel;
                idCidade = CidadeSelecionada.CidadeId.ToString();
            }

            // Verifica o Campo de Nome e Telefone (Obrigatórios serem preenchidos)
            if (string.IsNullOrEmpty(lblNomeUsuario.Text))
            {
                // Exibe o alerta
                await DisplayAlert("Atenção!", "Nome em branco. Verifique!", "Aceitar");
                lblNomeUsuario.Focus();
                return;
            }

            if (string.IsNullOrEmpty(lblTelefone.Text))
            {
                // Exibe o alerta
                await DisplayAlert("Atenção!", "Telefone em branco. Verifique!", "Aceitar");
                lblTelefone.Focus();
                return;
            }

            // Verifica se o Estado foi preenchido e válida os demais campos (Torna eles Obrigatórios)
            if (pickerEstados.SelectedIndex != -1)
            {
                if (pickerCidades.SelectedIndex == -1)
                {
                    // Exibe o alerta
                    await DisplayAlert("Atenção!", "Cidade não escolhida. Verifique!", "Aceitar");
                    return;
                }

                if (string.IsNullOrEmpty(lblRua.Text))
                {
                    // Exibe o alerta
                    await DisplayAlert("Atenção!", "Endereço em branco. Verifique!", "Aceitar");
                    lblRua.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(entryLogradouro.Text))
                {
                    // Exibe o alerta
                    await DisplayAlert("Atenção!", "Número em branco. Verifique!", "Aceitar");
                    entryLogradouro.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(entryCep.Text))
                {
                    // Exibe o alerta
                    await DisplayAlert("Atenção!", "CEP em branco. Verifique!", "Aceitar");
                    entryCep.Focus();
                    return;
                }

            }

            bool resposta = await DisplayAlert("Atenção", "Deseja salvar as alterações realizadas?", "Sim", "Não");

            if (resposta)
            {
                // Chama o Popup de Loading
                await PopupNavigation.Instance.PushAsync(new LoadingPopUpView());
                // Envia os dados para serem atualizados
                var result = clienteService.atualizaDadosCliente(
                    lblNomeUsuario.Text,
                    lblTelefone.Text,
                    lblRua.Text,
                    entryCep.Text,
                    idEstado,
                    idCidade,
                    entryLogradouro.Text
                );

                // Fecha o Popup de Loading
                try
                {
                    await PopupNavigation.Instance.PopAsync();
                }
                catch (Exception) { };

                if (result.Tipo == "ok")
                {
                    // Atualiza os dados do Cliente Logado
                    ClienteLogado.nome     = lblNomeUsuario.Text;
                    ClienteLogado.telefone = lblTelefone.Text;
                    ClienteLogado.rua      = lblRua.Text;
                    ClienteLogado.cep      = entryCep.Text;
                    ClienteLogado.estado   = idEstado;
                    ClienteLogado.cidade   = idCidade;
                    ClienteLogado.numero   = entryLogradouro.Text;

                    homePage.AtualizaNome();

                    // Exibe o alerta
                    await DisplayAlert("Tudo Certo!", "Seus dados foram atualizados com Sucesso", "Aceitar");
                } else
                {
                    // Exibe o alerta
                    await DisplayAlert("Ops...", "Não foi possivel atualizar os dados, tente novamente!", "Aceitar");
                }

            }


        }
    }
}