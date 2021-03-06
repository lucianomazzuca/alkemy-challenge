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

        public IEnumerable<Movie> GetAllWith(string title, string order, int? genreId = null)
        {
            IQueryable<Movie> movies = _context.Movies;
            
            if (title != null)
            {
                movies = movies.Where(m => m.Title.Contains(title));
            }

            if (order == "ASC")
            {
                movies = movies.OrderBy(m => m.CreatedAt);
            }

            if (order == "DESC")
            {
                movies = movies.OrderByDescending(m => m.CreatedAt);
            }

            if (genreId != null)
            {
                movies = movies.Where(m => m.Genres.Any(g => g.Id == genreId));
            }

            return movies;
        }

        public override async Task<Movie> GetById(int id)
        {
            var movie = await _context.Movies
                .Include(movie => movie.Characters)
                .Include(movie => movie.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie;
        }


        public override async Task Add(Movie movie)
        {
            var newMovie = movie;

            if (movie.Characters != null)
            {
                foreach (var character in movie.Characters)
                {
                    _context.Characters.Attach(character);
                }
            }

            if (movie.Genres != null)
            {
                foreach (var genre in movie.Genres)
                {
                    _context.Genres.Attach(genre);
                }
            }

            _context.Movies.Add(newMovie);

            await _context.SaveChangesAsync();
        }

        public override async Task Update(Movie movie)
        {
            if (movie.Characters != null)
            {
                var characters = movie.Characters;

                // Delete all related chars
                //movie.Characters.Clear();

                movie.Characters = characters;

                foreach (var character in movie.Characters)
                {
                    _context.Characters.Attach(character);
                }
            }

            if (movie.Genres != null)
            {
                var genres = movie.Genres;

                // Delete all related genres
                //movie.Genres.Clear();

                foreach (var genre in movie.Genres)
                {
                    _context.Genres.Attach(genre);
                }
            }

            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddWith(Movie movie, int[] characterIds = null, int[] genresIds = null)
        {
            var newMovie = movie;
            newMovie.CreatedAt = DateTimeOffset.UtcNow;

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

        public async Task UpdateWith(Movie movie, int[] characterIds = null, int[] genresIds = null)
        {
            var movieToUpdate = await _context.Movies
                .Include(movie => movie.Characters)
                .Include(movie => movie.Genres)
                .FirstOrDefaultAsync(m => m.Id == movie.Id);

            _context.Entry(movieToUpdate).CurrentValues.SetValues(movie);

            // Clear related data
            movieToUpdate.Characters.Clear();
            movieToUpdate.Genres.Clear();

            if (characterIds != null)
            {
                foreach (int characterId in characterIds)
                {
                    Character character = await _context.Characters.FindAsync(characterId);
                    movieToUpdate.Characters.Add(character);
                }
            }

            if (genresIds != null)
            {
                foreach (int genreId in genresIds)
                {
                    Genre genre = await _context.Genres.FindAsync(genreId);
                    movieToUpdate.Genres.Add(genre);
                }
            }

            //_context.Entry(movie).State = EntityState.Modified;
            //_context.Movies.Update(movieToUpdate);
            await _context.SaveChangesAsync();
        }

    }
}
