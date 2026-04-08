using EstudoApi.Context;
using EstudoApi.Domains;
using EstudoApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace EstudoApi.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _db;

        public ProdutoRepository(AppDbContext db) 
        { 
            _db = db; 
        }

        public async Task<List<Produto>> GetAllAsync()
        {
            return await _db.Produtos.ToListAsync();
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            return await _db.Produtos.FindAsync(id);
        }

        public async Task<Produto> CreateAsync(Produto produto)
        {
            _db.Produtos.Add(produto);
            await _db.SaveChangesAsync();
            return produto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var produto = await _db.Produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null)
            {
                return false;
            }

            _db.Produtos.Remove(produto);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByNomeAsync(string nome)
        {
            var produto = await _db.Produtos
                .FirstOrDefaultAsync(p => p.Nome == nome);

            if (produto == null)
                return false;

            _db.Produtos.Remove(produto);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
