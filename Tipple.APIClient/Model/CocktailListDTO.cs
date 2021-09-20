using System;
using System.Collections.Generic;
using System.Text;

namespace Tipple.APIClient.Model
{
    public class CocktailListDTO
    {
        public List<CocktailDTO> Cocktails { get; set; } = new List<CocktailDTO>();
        public ListMetaDTO meta { get; set; }
    }

    public class ListMetaDTO
    {
        public int count { get; set; }
        public int firstId { get; set; }
        public int lastId { get; set; }
        public int medianIngredientCount { get; set; }


    }
}

