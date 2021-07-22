using AlkemyChallenge.Data;
using AlkemyChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.Repositories
{
    public class MovieRepository : BaseRepository<Movie>
    {
        public MovieRepository(AppDbContext context) : base(context)
        {
        }
    }
}
