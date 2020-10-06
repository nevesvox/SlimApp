using HandSmartSlim.Models;
using HandSmartSlim.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace HandSmartSlim.Services
{
    class ClienteService
    {
        // Função responsável por encaminhar as rotas e os parâmetros para a ApiService
        public string EnviaRequisicao(string caminho, string post)
        {
            ApiService apiserv = new ApiService();
            return apiserv.FazRequisicaoPOST("https://slimws.tk/mobile/"+caminho, post);
        }

        // Função responsável por realizar o Login do Cliente
        public Cliente RealizaLogin(string email, string senha)
        {
            // Envia a requisição para realizaLogin
            var json = EnviaRequisicao("/realizaLogin",
                "email=" +email+
                "&senha="+senha
            );
            
            // Formata o Json
            Cliente cliente = JsonConvert.DeserializeObject<Cliente>(json);

            // Atualiza as váriaveis do Cliente Logado
            ClienteLogado.id       = cliente.Id;
            ClienteLogado.nome     = cliente.Nome;
            ClienteLogado.cpf      = cliente.Cpf;
            ClienteLogado.telefone = cliente.Telefone;
            
            //Retorna o cliente
            return cliente;
        }
        
        // Função responsável por cadastrar um novo Cliente
        public RespostaApi CadastraCliente(
            string nome,
            string cpf,
            string telefone,
            string email,
            string senha
            )
        {
            // Envia a requisição para cadastraCliente
            var json = EnviaRequisicao("/cadastraCliente",
                "nome="      + nome +
                "&cpf="      + cpf +
                "&telefone=" + telefone +
                "&email="    + email +
                "&senha="    + senha
            );

            // Formata o Json
            RespostaApi result = JsonConvert.DeserializeObject<RespostaApi>(json);

            return result;
        }

    }

}
