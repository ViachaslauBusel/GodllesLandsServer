using Game.Resources;
using NetworkGameEngine.Debugger;
using Newtonsoft.Json;
using Protocol.Data.Recipes;

namespace Godless_Lands_Game.Recipes
{
    internal class RecipesDataStorageService
    {
        private Dictionary<int, RecipeData> _recipes = new Dictionary<int, RecipeData>();

        public RecipesDataStorageService()
        {
            Load();
        }
        public void Load()
        {
            string fullPath = Path.Combine(ResourceFile.Folder, ResourceFile.Recipes);
            if (File.Exists(fullPath))
            {
                string text = File.ReadAllText(fullPath);
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };
                List<RecipeData> recipes = JsonConvert.DeserializeObject<List<RecipeData>>(text, settings);
                for (int i = 0; i < recipes.Count; i++)
                {
                    RecipeData recipe = recipes[i];
                    if (recipe == null)
                    {
                        Debug.Log.Error("ItemsDataManager", "Item with index {0} is null", i);
                        continue;
                    }
                    if (_recipes.ContainsKey(recipe.id))
                    {
                        Debug.Log.Error("RecipesDataStorageService", "Recipe with id {0} already exists", recipe.id);
                        continue;
                    }
                    _recipes.Add(recipe.id, recipe);
                }
            }
            else Debug.Log.Fatal("RecipesDataStorageService", $"{fullPath} not found");
        }

        internal RecipeData GetRecipe(int recipeID)
        {
            if (_recipes.TryGetValue(recipeID, out RecipeData data))
            {
                return data;
            }
            else
            {
                Debug.Log.Error("RecipesDataStorageService", "Recipe with id {0} not found", recipeID);
                return null;
            }
        }
    }
}
