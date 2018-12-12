using System;
using System.Threading.Tasks;
using MySmoothieTry2.Model;
using MySmoothieTry2.RestClients;

namespace MySmoothieTry2.ServiceHandlers
{
    public class IngredientServices
    {
        Edamam<IngredientMainModel> _edamamRest = new Edamam<IngredientMainModel>();
        Edamam<NutritionPOSTReply> _edamamPost = new Edamam<NutritionPOSTReply>();

        public async Task<IngredientMainModel> GetIngredientDetails(string ingredient)
        {
            var getIngredientDetails = await _edamamRest.GetAllIngredients(ingredient).ConfigureAwait(false);
            return getIngredientDetails;
        }

        //public async Task<NutritionPOSTReply> GetNutritionDetails(NutritionModelPOST ingModelPOST)
        public async Task<NutritionPOSTReply> GetNutritionDetails(NutritionModelPOST nutrModelPOST)
        {
            var getNutritionDetails = await _edamamPost.GetSmoothieNutritionInfo(nutrModelPOST).ConfigureAwait(false);
            var a = getNutritionDetails;
            return getNutritionDetails;
        }
    }
}
