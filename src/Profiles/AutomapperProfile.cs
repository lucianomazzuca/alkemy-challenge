﻿using AlkemyChallenge.DTOs.Movie;
using AlkemyChallenge.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Movie, MovieReadDto>();
        }
    }
}