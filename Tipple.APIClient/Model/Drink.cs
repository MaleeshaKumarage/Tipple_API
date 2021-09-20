using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tipple.APIClient.Model
{
    public class Drink
    {
        public string strDrink { get; set; }
        public string strDrinkThumb { get; set; }
        public int idDrink { get; set; }
    }

    public class DrinkRoot
    {
        public List<Drink> drinks { get; set; }
    }
}
