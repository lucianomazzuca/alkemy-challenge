using AlkemyChallenge.Data;
using AlkemyChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.Repositories
{
    interface IRepository<T> where T : IModel
    {
        Task<T> GetAll();
        Task<T> GetById();
        Task<T> Add();
        Task<T> Delete();
    }
}
