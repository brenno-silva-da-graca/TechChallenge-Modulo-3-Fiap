using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : ControllerBase
    {
        private readonly IContatoCadastro _contatoCadastro;

        public ContatosController(IContatoCadastro contatoCadastro)
        {
            _contatoCadastro = contatoCadastro;
        }

        [HttpGet("Listar")]
        public IActionResult GetContato()
        {
            return Ok(_contatoCadastro.ListarContatos());
        }

        [HttpGet("ListarPorDDD")]
        public IActionResult ListarPorDDD(int NumDDD)
        {
            return Ok(_contatoCadastro.ListarPorDDD(NumDDD));
        }

        [HttpPost("Inserir")]
        public IActionResult PostContato([FromBody] Contato dadosContato)
        {
            Retorno retornoVal = new Retorno();

            dadosContato = _contatoCadastro.CriarContato(dadosContato, out retornoVal);

            if (retornoVal.Codigo != 200)
            {
                return StatusCode(retornoVal.Codigo, retornoVal);
            }

            return Ok(dadosContato);

        }
        [HttpPut("Atualizar")]
        public IActionResult PutContato([FromBody] Contato dadosContato, int Id)
        {
            dadosContato.Id = Id;
            _contatoCadastro.AtualizarContato(dadosContato);
            return Ok();
        }
        [HttpDelete("Deletar")]
        public IActionResult DeleteContato(int Id)
        {
            _contatoCadastro.DeletarContato(Id);
            return Ok();
        }
    }
}
