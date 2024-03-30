using Protocol.Data.Workbenches;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Recipes
{
    public class RecipeData
    {
        public  int id;
        public  WorkbenchType workbench;
        public  int ResultItemID;
        public  int Profession;
        public  int Experience;
        public  int Stamina;
        public  List<RecipeComponent> Components;
        public  List<RecipeComponent> Fuel;

        public RecipeData(int id, WorkbenchType workbench, int resultItemID, int profession, int experience, int stamina, List<RecipeComponent> components, List<RecipeComponent> fuel)
        {
            this.id = id;
            this.workbench = workbench;
            ResultItemID = resultItemID;
            Profession = profession;
            Experience = experience;
            Stamina = stamina;
            Components = components;
            Fuel = fuel;
        }
    }
}
