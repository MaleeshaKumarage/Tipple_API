
using api.Tipple.APIClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Tipple.APIClient.Model;

namespace Tipple.APIClient
{
    public class BoozeApiClient : IBoozeApiClient
    {
        public readonly string _BaseUrl;
        public HttpClient _HttpClient { get; private set; }
        private const string _DefaultExceptionMessage = "Unknown error occurred";

        public BoozeApiClient(ApiClientConfiguration configuration, HttpClient httpClient = null)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (string.IsNullOrWhiteSpace(configuration.BaseUrl))
                throw new ArgumentNullException(nameof(configuration.BaseUrl));

            _BaseUrl = configuration.BaseUrl;
            _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (httpClient == null)
            {
                _HttpClient = httpClient;
            }
            else
            {
                _HttpClient = new HttpClient();
            }
        }

        //Method to get Random Cocktail
        public async Task<CocktailDTO> GetRandomCocktail(CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{_BaseUrl}/json/v1/1/random.php"),
                Method = HttpMethod.Get
            };
            var Dto = await InvokeApiAsync<DrinkDetailRoot>(httpRequest, cancellationToken: cancellationToken);
            Type t = Dto.drinks[0].GetType();
            PropertyInfo[] props = t.GetProperties();
            List<string> ingrediance = new List<string>();
            //System.reflection to Iterate through properties
            foreach (var prop in props)
            {
                if (prop.GetValue(Dto.drinks[0]) != null && prop.Name.Contains("strIngredient"))
                {
                    ingrediance.Add(prop.GetValue(Dto.drinks[0]).ToString());
                }
            }
            return new CocktailDTO { Id = Dto.drinks[0].idDrink, Name = Dto.drinks[0].strDrink, Instructions = Dto.drinks[0].strInstructions, Ingredients = ingrediance, ImageURL = Dto.drinks[0].strDrinkThumb };
        }
        //Method to get List of Cocktails by Ingredient
        public async Task<CocktailListDTO> SearchByIngredient(string str, CancellationToken cancellationToken = default)
        {
            CocktailListDTO cocktailListDTO = new CocktailListDTO();

            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{_BaseUrl}/json/v1/1/filter.php?i={str}"),
                Method = HttpMethod.Get
            };
            var drinkList = await InvokeApiAsync<DrinkRoot>(httpRequest, cancellationToken: cancellationToken);

            if (drinkList != null && drinkList.drinks.Count > 0)
            {
                var tasks = drinkList.drinks.Select(a => SearchById(a.idDrink));
                //Creates a task that will complete when all of the Task objects in an array have completed.
                //Can be increase the process by using the API Batches - Not Implemented here
                var res = await Task.WhenAll(tasks.ToArray());
                cocktailListDTO.Cocktails = res.ToList();
                cocktailListDTO.meta = new ListMetaDTO() { count = cocktailListDTO.Cocktails.Count, firstId = cocktailListDTO.Cocktails.OrderBy(a => a.Id).FirstOrDefault().Id, lastId = cocktailListDTO.Cocktails.OrderBy(a => a.Id).LastOrDefault().Id, medianIngredientCount = cocktailListDTO.Cocktails.Sum(a => a.Ingredients.Count) / cocktailListDTO.Cocktails.Count };
            }

            return cocktailListDTO;
        }
        //Method to get List of Cocktails by Id
        public async Task<CocktailDTO> SearchById(int id, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{_BaseUrl}/json/v1/1/lookup.php?i={id}"),
                Method = HttpMethod.Get
            };
            var Dto = await InvokeApiAsync<DrinkDetailRoot>(httpRequest, cancellationToken: cancellationToken);
            Type t = Dto.drinks[0].GetType();
            PropertyInfo[] props = t.GetProperties();
            List<string> ingrediance = new List<string>();
            foreach (var prop in props)
            {
                if (prop.GetValue(Dto.drinks[0]) != null && prop.Name.Contains("strIngredient"))
                {
                    ingrediance.Add(prop.GetValue(Dto.drinks[0]).ToString());
                }
            }

            return new CocktailDTO { Id = Dto.drinks[0].idDrink, Name = Dto.drinks[0].strDrink, Instructions = Dto.drinks[0].strInstructions, Ingredients = ingrediance, ImageURL = Dto.drinks[0].strDrinkThumb };
        }
        //Method to all API Endpoints
        private async Task<T> InvokeApiAsync<T>(HttpRequestMessage httpRequest, CancellationToken cancellationToken = default)
        {

            HttpResponseMessage response = await _HttpClient.SendAsync(httpRequest, cancellationToken);


            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.InternalServerError:
                    {
                        if (response.Content.Headers.ContentType.MediaType == "application/json")
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var content = System.Text.Json.JsonSerializer.Deserialize<CommonError>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = false });
                            throw new ApiException((int)response.StatusCode, content?.Message, content);
                        }
                        else
                            throw new ApiException((int)response.StatusCode, _DefaultExceptionMessage);
                    }
                    case System.Net.HttpStatusCode.Unauthorized:
                    {
                        throw new ApiException((int)response.StatusCode, "Access is denied due to invalid credentials.");
                    }
                    default:
                    {
                        throw new ApiException((int)response.StatusCode, _DefaultExceptionMessage);
                    }
                }
            }
        }


    }
}
