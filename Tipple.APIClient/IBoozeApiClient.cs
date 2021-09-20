using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tipple.APIClient.Model;

namespace Tipple.APIClient
{
    public interface IBoozeApiClient
    {
        Task<CocktailDTO> SearchById(int id, CancellationToken cancellationToken = default);
        Task<CocktailListDTO> SearchByIngredient(string str, CancellationToken cancellationToken = default);
        Task<CocktailDTO> GetRandomCocktail(CancellationToken cancellationToken = default);

    }
}
