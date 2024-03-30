namespace Protocol.Data.Recipes
{
    public class RecipeComponent
    {
        public  int ItemID;
        public  int Amount;

        public RecipeComponent(int itemID, int amount)
        {
            ItemID = itemID;
            Amount = amount;
        }
    }
}