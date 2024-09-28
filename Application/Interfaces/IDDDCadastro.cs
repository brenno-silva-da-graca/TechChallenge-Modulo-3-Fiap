using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDDDCadastro
    {
        public IEnumerable<DDD> ListarDDD();
    }
}
