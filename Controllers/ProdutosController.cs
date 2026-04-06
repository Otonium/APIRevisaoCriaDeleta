using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstudoApi.Models;

namespace EstudoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _db; // Guarda o acesso ao banco

        public ProdutosController(AppDbContext db) //O ASP.NET entrega o AppDbContext automaticamente aqui
        { 
            _db = db; 
        }

        [HttpGet]
        public async Task<ActionResult<List<Produto>>>GetAll()
        {
            var lista = await _db.Produtos.ToListAsync(); //Busca todos os produtos no banco
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>>GetById(int id)
        {
            var produto = await _db.Produtos.FindAsync(id); //Procura pelo Id

            if(produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>>Create(Produto produto)
        {
            if(string.IsNullOrWhiteSpace(produto.Nome))
            {
                return BadRequest("Nome é obrigatório.");
            }
            _db.Produtos.Add(produto);//Pedido de inserção
            await _db.SaveChangesAsync();//Confirma no banco (Faz o INSERT)
            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _db.Produtos.FindAsync(id); //Procura o produto
            if(produto == null)
            {
                return NotFound();
            }

            _db.Produtos.Remove(produto); //Marca para deletar
            await _db.SaveChangesAsync(); //Confirma no banco (faz o DELETE)
            return NoContent();
        }
    }
}
