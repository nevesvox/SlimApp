using System;
using System.Collections.Generic;
using System.Text;

namespace HandSmartSlim.Models
{
    public class CompraModel
    {
        public string Valor { get; set; }

        public string Data { get; set; }

        public string Pagamento { get; set; }

        public int Qtde { get; set; }

        public string Descricao { get; set; }

        public float ValorUnitario { get; set; }

        public float ValorCompra { get; set; }

        public float ValorTotal { get; set; }

        public string Tipo { get; set; }

        public string Mensagem { get; set; }

    }

}
