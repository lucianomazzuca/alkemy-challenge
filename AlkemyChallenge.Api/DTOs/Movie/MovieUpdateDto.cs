using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.DTOs.Movie
{
    public class MovieUpdateDto
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public int Rating { get; set; }
    }
}
