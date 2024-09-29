using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Repositories;
using System.Data;

namespace TechChallenge_Contatos.Repository
{
    public class ContatoRepository : IContatoCadastro
    {
        private readonly IDbConnection _dbConnection;
        public ContatoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<ContatoDDD> ListarContatos()
        {
            var ComandoSql = @"SELECT * FROM Contatos inner join DDD on DDD.id = Contatos.DDDID";

            return _dbConnection.Query<ContatoDDD, DDD, ContatoDDD>
            (ComandoSql, map: (ContatoDDD, DDD) => { ContatoDDD.DDD = DDD; return ContatoDDD; }
            , splitOn: "Id,DDDID");
        }

        public IEnumerable<ContatoDDD> ListarPorDDD(int NumDDD)
        {
            var ComandoSql = @"SELECT * FROM Contatos inner join DDD on DDD.id = Contatos.DDDID WHERE DDD.NumDDD = @NUMDDD";

            return _dbConnection.Query<ContatoDDD, DDD, ContatoDDD>
            (ComandoSql, map: (ContatoDDD, DDD) => { ContatoDDD.DDD = DDD; return ContatoDDD; },
            new { NUMDDD = NumDDD }, splitOn: "Id,DDDID");

        }

        public Contato CriarContato(Contato dadosContato, out Retorno ret)
        {
            ret = new Retorno();
            ret.Codigo = 200;
            ret.Mensagem = "Sucesso!";

            try
            {
                var telefone = dadosContato.Telefone;
                var email = dadosContato.Email;
                
                bool mailOK = ContatoRepositoryDomain.EmailIsOk(email);

                if (mailOK == false)
                {
                    ret.Mensagem = "E-mail inválido";
                    ret.Codigo = 400;
                    return dadosContato;
                }
                
                bool TeleOk = ContatoRepositoryDomain.TelefoneIsOk(telefone);

                if (TeleOk == false)
                {
                    ret.Mensagem = "Telefone inválido";
                    ret.Codigo = 400;
                    return dadosContato;
                }
                else
                {
                    var DDD = ContatoRepositoryDomain.GetDDDFromStringTelefone(dadosContato.Telefone);


                    var RecuperarIdDDD = _dbConnection.Query("Select Id from DDD where NumDDD = @NUMDDD", new { NUMDDD = DDD }).SingleOrDefault();

                    if (RecuperarIdDDD != null)
                    {
                        dadosContato.DDDID = RecuperarIdDDD.Id;
                    }
                    else
                    {
                        ret.Mensagem = "DDD Inexistente";
                        ret.Codigo = 400;
                        return dadosContato;
                    }
                }
                var ComandoSQL = @"insert into contatos (nome, telefone,email,DDDID) values (@nome,@telefone,@Email,@DDDID)";
                _dbConnection.Execute(ComandoSQL, dadosContato);
            }
            catch (Exception ex)
            {
                ret.Mensagem = ex.Message;
                ret.Codigo = 500;
            }
            return dadosContato;
        }

        public void AtualizarContato(Contato dadosContato)
        {
            var DDD = ContatoRepositoryDomain.GetDDDFromStringTelefone(dadosContato.Telefone);

            dadosContato.DDDID = _dbConnection.Query("Select Id from DDD where NumDDD = @NUMDDD", new { NUMDDD = DDD }).SingleOrDefault().Id;

            var ComandoSQL = @"update contatos Set nome = @Nome, telefone = @Telefone, email = @Email,DDDID = @DDDID where ID = @ID";
            _dbConnection.Execute(ComandoSQL, dadosContato);
        }

        public void DeletarContato(int Id)
        {
            var comandoSQL = @"DELETE FROM CONTATOS WHERE ID = @ID";
            _dbConnection.Execute(comandoSQL, new { ID = Id });
        }
    }
}
