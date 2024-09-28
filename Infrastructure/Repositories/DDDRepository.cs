using Application.Interfaces;
using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Repositories
{
    public class DDDRepository : IDDDCadastro
    {
        private readonly IDbConnection _dbConnection;

        public DDDRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IList<DDD> listaDDD { get; set; }

        public IEnumerable<DDD> ListarDDD()
        {
            return _dbConnection.Query<DDD>("Select * from DDD").ToList();
        }
    }
}
