using System;

namespace Domains.Log.Entities
{
    public class AccessLog
    {
        public AccessLog(string acao, DateTime data, string usuario, string tabelaModificada, string dados)
        {
            Id = Guid.NewGuid();
            Acao = acao;
            Data = data;
            Usuario = usuario;
            TabelaModificada = tabelaModificada;
            Dados = dados;
        }        

        public AccessLog() { }

        public Guid Id { get; private set; }
        public string Acao { get; private set; }
        public DateTime Data { get; private set; }
        public string Usuario { get; private set; }
        public string TabelaModificada { get; private set; }
        public string Dados { get; private set; }
    }
}