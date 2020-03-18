using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzas.Models
{
    public class PizzaVM
    {
        public Pizza pizza { get; set; }

        public List<Ingredient> ingredients = new List<Ingredient>();

        public List<Pate> pates = new List<Pate>();

        public List<int> selectedIngredients { get; set; } = new List<int>();

        public int selectedPate { get; set; }

    }
}