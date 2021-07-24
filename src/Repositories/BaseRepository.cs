using AlkemyChallenge.Data;
using AlkemyChallenge.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlkemyChallenge.Repositories
{
    public abstract class BaseRepository<TModel> : IRepository<TModel> where TModel : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TModel> _model;

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

        public virtual async Task Add(TModel item)
        {
            await _model.AddAsync(item);

            _context.SaveChanges();
        }

        public virtual async Task Delete(int id)
        {
            var item = await _model.FindAsync(id);
            if (item == null)
            {
                throw new RecordNotFoundException();
            };

            _model.Remove(item);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Update(TModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}