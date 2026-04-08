using EstudoApi.Domains;

namespace EstudoApi.Interface
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> GetAllAsync();
        Task<Produto?> GetByIdAsync(int id);
        Task<Produto> CreateAsync(Produto produto);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByNomeAsync(string nome);
    }
}
