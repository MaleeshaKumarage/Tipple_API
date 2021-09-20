using api.Models.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipple.APIClient.Model;

namespace api.Profiles
{
    public class CocktailProfile : Profile
    {
        public CocktailProfile()
        {
            CreateMap<CocktailDTO, Cocktail>();
            CreateMap<CocktailListDTO, CocktailList>();
            CreateMap<ListMetaDTO, ListMeta>();
        }
    }
}
