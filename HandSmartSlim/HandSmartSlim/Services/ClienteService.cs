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
            ClienteLogado.id             = cliente.Id;
            ClienteLogado.nome           = cliente.Nome;
            ClienteLogado.email          = cliente.Email;
            ClienteLogado.cpf            = cliente.Cpf;
            ClienteLogado.telefone       = cliente.Telefone;
            ClienteLogado.genero         = cliente.Genero;
            ClienteLogado.dataNascimento = cliente.DataNascimento;
            ClienteLogado.cep            = cliente.Cep;
            ClienteLogado.numero         = cliente.Numero;
            ClienteLogado.cidade         = cliente.Cidade;
            ClienteLogado.estado         = cliente.Estado;
            ClienteLogado.rua            = cliente.Rua;

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
                "nome="      + nome     +
                "&cpf="      + cpf      +
                "&telefone=" + telefone +
                "&email="    + email    +
                "&senha="    + senha
            );

            // Formata o Json
            RespostaApi result = JsonConvert.DeserializeObject<RespostaApi>(json);

            return result;
        }

        // Função responsável por Buscar os Cartões do Cliente
        public dynamic BuscaCartaoCreditoCliente()
        {
            // Envia a requisição para buscaCartaoCreditoCliente
            var json = EnviaRequisicao("/buscaCartaoCreditoCliente",
                "idCliente=" + ClienteLogado.id 
            );

            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

        // Função responsável por Salvar Novo Cartão do Cliente
        public dynamic SalvaCartao(
            string Bandeira,
            string Imagem,
            string Numero,
            string Nome,
            string Validade,
            int    Codigo
        )
        {
            // Envia a requisição para salvaNovoCartao
            var json = EnviaRequisicao("/salvaNovoCartao",
                "idCliente=" + ClienteLogado.id +
                "&bandeira=" + Bandeira         +
                "&imagem="   + Imagem           +
                "&numero="   + Numero           +
                "&nome="     + Nome             +
                "&validade=" + Validade         +
                "&codigo="   + Codigo
            );

            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

        // Função responsável por Excluir o Cartão do Cliente
        public dynamic ExcluiCartao(int idCartao)
        {
            // Envia a requisição para excluiCartao
            var json = EnviaRequisicao("/excluiCartao",
                "idCliente=" + ClienteLogado.id +
                "&idCartao=" + idCartao
            );

            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

        // Função responsável por Finalizar a Compra do Cliente
        public dynamic FinalizaCompraCliente(int idCartao, int idCompra, float valorCompra)
        {
            // Envia a requisição para finalizaCompraCliente
            var json = EnviaRequisicao("/finalizaCompraCliente",
                "idCliente="    + ClienteLogado.id +
                "&idCartao="    + idCartao         +
                "&idCompra="    + idCompra         +
                "&valorCompra=" + valorCompra
            );

            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

        // Função responsável por Finalizar a Compra do Cliente
        public dynamic buscaUltimasComprasCliente()
        {
            // Envia a requisição para buscaUltimasComprasCliente
            var json = EnviaRequisicao("/buscaUltimasComprasCliente",
                "idCliente=" + ClienteLogado.id
            );

            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

        public dynamic buscaItensCompra(int IdCompra)
        {
            // Envia a requisição para buscaItensCompra
            var json = EnviaRequisicao("/buscaItensCompra",
                "idCompra=" + IdCompra
            );

            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

        public dynamic buscaEstados()
        {
            // Envia a requisição para buscaEstados
            var json = EnviaRequisicao("/buscaEstados", "");

            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

        public dynamic buscaCidadesEstado(string IdEstado)
        {
            // Envia a requisição para buscaCidadesEstado
            var json = EnviaRequisicao("/buscaCidadesEstado",
                "idEstado=" + IdEstado
            );

            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

        public dynamic atualizaDadosCliente(
            string NomeCliente,
            string Telefone,
            string NomeRua          = null,
            string Cep              = null,
            string IdEstado         = null,
            string IdCidade         = null,
            string NumeroLogradouro = null
        )
        {
            // Envia a requisição para atualizaDadosCliente
            var json = EnviaRequisicao("/atualizaDadosCliente",
                "idCliente="    + ClienteLogado.id +
                "&nomeCliente=" + NomeCliente      +
                "&telefone="    + Telefone         +
                "&nomeRua="     + NomeRua          +
                "&cep="         + Cep              +
                "&idEstado="    + IdEstado         +
                "&idCidade="    + IdCidade         +
                "&numeroLogra=" + NumeroLogradouro
            );

            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

    }

}
