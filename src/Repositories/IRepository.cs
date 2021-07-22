using AlkemyChallenge.Data;
using AlkemyChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.Repositories
{
    interface IRepository<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAll();
        Task<TModel> GetById(int id);
        Task Add();
        Task Delete();
        Task Update();
    }
}
