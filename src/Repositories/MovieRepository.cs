using AlkemyChallenge.Data;
using AlkemyChallenge.Models;
using Microsoft.EntityFrameworkCore;
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

        public override async Task<Movie> GetById(int id)
        {
            var movie = await _context.Movies
                .Include(movie => movie.Characters)
                .Include(movie => movie.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie;
        }

        public async Task Add(Movie movie, int[] characterIds = null, int[] genresIds = null)
        {
            var newMovie = movie;

            if (characterIds != null)
            {
                foreach (int characterId in characterIds)
                {
                    Character character = await _context.Characters.FindAsync(characterId);
                    newMovie.Characters.Add(character);
                }
            }

            if (genresIds != null)
            {
                foreach (int genreId in genresIds)
                {
                    Genre genre = await _context.Genres.FindAsync(genreId);
                    newMovie.Genres.Add(genre);
                }
            }

            _context.Movies.Add(newMovie);

            await _context.SaveChangesAsync();
        }

        public async Task Update(Movie movie, int[] characterIds = null, int[] genresIds = null)
        {
            if (characterIds != null)
            {
                foreach (int characterId in characterIds)
                {
                    movie.Characters.Clear();
                    Character character = await _context.Characters.FindAsync(characterId);
                    movie.Characters.Add(character);
                }
            }

            if (genresIds != null)
            {
                foreach (int genreId in genresIds)
                {
                    movie.Genres.Clear();
                    Genre genre = await _context.Genres.FindAsync(genreId);
                    movie.Genres.Add(genre);
                }
            }

            _context.Entry(movie).State = EntityState.Modified;
            //_context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

    }
}
