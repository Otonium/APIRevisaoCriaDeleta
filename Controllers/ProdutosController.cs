using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstudoApi.Domains;
using EstudoApi.Context;
using EstudoApi.Exceptions;
using EstudoApi.Interface;
using EstudoApi.DTOs;

namespace EstudoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repo;
        public ProdutosController(IProdutoRepository repo)
        {
            _repo = repo; 
        }

        [HttpGet]
        public async Task<ActionResult<List<ProdutoLerDto>>> GetAll()
        {
            var produtos = await _repo.GetAllAsync();

            var dto = produtos.Select(p => new ProdutoLerDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Preco = p.Preco
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoLerDto>> GetById(int id)
        {
            var produto = await _repo.GetByIdAsync(id);

            if (produto == null)
            {
                throw new NaoEncontrouException($"Produto {id} não encontrado.");
            }

            var dto = new ProdutoLerDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoLerDto>> Create(ProdutoCriarDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Nome))
            {
                return BadRequest("Nome é obrigatorio.");
            }

            if (input.Preco <= 0)
            {
                return BadRequest("Preço precisa ser maior que zero.");
            }

            var produto = new Produto
            {
                Nome = input.Nome.Trim(),
                Preco = input.Preco
            };

            var criado = await _repo.CreateAsync(produto);

            var dto = new ProdutoLerDto
            {
                Id = criado.Id,
                Nome = criado.Nome,
                Preco = criado.Preco
            };

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var apagou = await _repo.DeleteAsync(id);

            if (!apagou)
                return NotFound($"Produto {id} não encontrado.");

            return NoContent();
        }

        [HttpDelete("nome/{nome}")]
        public async Task<IActionResult> DeleteByNome(string nome)
        {
            var apagou = await _repo.DeleteByNomeAsync(nome);

            if (!apagou)
                return NotFound($"Produto {nome} não encontrado.");

            return NoContent();
        }
    }
}
