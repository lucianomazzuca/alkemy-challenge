using AlkemyChallenge.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlkemyChallenge.Repositories
{
    public abstract class BaseRepository<TModel> : IRepository<TModel> where TModel : class
    {
        private readonly AppDbContext _context;
        private DbSet<TModel> _model;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _model = context.Set<TModel>();
        }

        public async virtual Task<IEnumerable<TModel>> GetAll()
        {
            return await _model.ToListAsync();
        }

        public virtual async Task<TModel> GetById(int id)
        {
            return await _model.FindAsync(id);
        }

        public virtual Task Add()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task Delete()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task Update()
        {
            throw new System.NotImplementedException();
        }
    }
}