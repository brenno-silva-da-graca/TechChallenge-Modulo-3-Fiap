using Domain.Entities;

namespace Application.Interfaces
{
    public interface IContatoCadastro
    {
        public IEnumerable<ContatoDDD> ListarContatos();

        public IEnumerable<ContatoDDD> ListarPorDDD(int NumDDD);

        public Contato CriarContato(Contato dadosContato, out Retorno ret);

        public void AtualizarContato(ContatoDTO dadosContato);

        public void DeletarContato(int Id);

    }
}
