using HandSmartSlim.Models;
using HandSmartSlim.Util;
using Java.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HandSmartSlim.Services
{
    class CompraService
    {
        // Função responsável por encaminhar as rotas e os parâmetros para a ApiService
        public string EnviaRequisicao(string caminho, string post)
        {
            ApiService apiserv = new ApiService();
            return apiserv.FazRequisicaoPOST("https://slimws.tk/mobile/" + caminho, post);
        }

        // Função responsável por buscar o Produto
        public dynamic InsereProdutoVenda(string codProduto)
        {
            // Envia a requisição para realizaLogin
            var json = EnviaRequisicao("/insereProdutoVenda",
                "codProduto=" + codProduto +
                "&idCliente= " + ClienteLogado.id +
                "&cpfCliente= " + ClienteLogado.cpf
            );

            // Transforma Json em um Array
            dynamic resposta = JsonConvert.DeserializeObject(json);
            Console.WriteLine(resposta);
            // Retorna a requisição
            return resposta;
        }

        public dynamic BuscaVendaAberta()
        {
            // Envia a requisição para realizaLogin
            var json = EnviaRequisicao("/buscaVendaAbertaCliente",
                "idCliente=" + ClienteLogado.id
            );
            
            // Transforma Json em um Array 
            dynamic resposta = JsonConvert.DeserializeObject(json);

            // Retorna a requisição
            return resposta;
        }

    }
}
