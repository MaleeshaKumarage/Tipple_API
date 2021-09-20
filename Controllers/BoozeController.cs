using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tipple.APIClient;

namespace api.Controllers
{
    [Route("api")]
    [ApiController]
    public class BoozeController : ControllerBase
    {
        // We will use the public CocktailDB API as our backend
        // https://www.thecocktaildb.com/api.php
        //
        // Bonus points
        // - Speed improvements
        // - Unit Tests

        private readonly IBoozeApiClient _BoozeApiClient;
        public BoozeController(IBoozeApiClient BoozeApiClient)
        {
            _BoozeApiClient = BoozeApiClient;
        }



        [HttpGet]
        [Route("search-ingredient/{ingredient}")]
        public async Task<IActionResult> GetIngredientSearch([FromRoute] string ingredient)
        {
            var cocktailList = new CocktailList();
            var cocktailListDTO = await _BoozeApiClient.SearchByIngredient(ingredient);
            if (cocktailListDTO.Cocktails.Count < 1)
            {
                return NotFound();
            }
            else
            {
                return Ok(cocktailListDTO);

            }
        }

        [HttpGet]
        [Route("random")]
        public async Task<IActionResult> GetRandom()
        {
            var cocktail = new Cocktail();
            var cocktailDTO = await _BoozeApiClient.GetRandomCocktail();
            if (cocktailDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cocktailDTO);

            }
            //USE AUTOMAPPER

            // TODO - Go and get a random cocktail
            // https://www.thecocktaildb.com/api/json/v1/1/random.php

        }
    }
}