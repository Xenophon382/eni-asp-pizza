using Pizzas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizzas.Controllers
{
    public class PizzaController : Controller
    {
        private static List<Pizza> ListePizzas;
        private static List<Pate> ListePatesDispo;
        private static List<Ingredient> ListIngredientDispo;

        private Ingredient getIngredientByName(string name)
        {
            return ListIngredientDispo.FirstOrDefault(i => i.Nom == name);
        }

        private Ingredient getIngredientById(int id)
        {
            return ListIngredientDispo.FirstOrDefault(i => i.Id == id);
        }

        private Pate getPateByName(string name)
        {
            return ListePatesDispo.FirstOrDefault(p => p.Nom == name);
        }

        private Pate getPateById(int id)
        {
            return ListePatesDispo.FirstOrDefault(p => p.Id == id);
        }

        private Pizza GetPizzaById(int id)
        {
            return ListePizzas.FirstOrDefault(p => p.Id == id);
        }


        public PizzaController()
        {
            ListePatesDispo = Pizza.PatesDisponibles;
            ListIngredientDispo = Pizza.IngredientsDisponibles;

            if(ListePizzas == null)
            {
                ListePizzas = new List<Pizza>
            {
               new Pizza
               {
                   Id=1,
                   Nom="Reine",
                   Pate=getPateByName("Pate fine, base tomate"),
                   Ingredients= new List<Ingredient>
                   {
                       getIngredientByName("Mozzarella"),
                       getIngredientByName("Jambon"),
                       getIngredientByName("Champignon")
                   }


               },
               new Pizza
               {
                   Id=2,
                   Nom="Saumon",
                   Pate=getPateByName("Pate fine, base crême"),
                   Ingredients= new List<Ingredient>
                   {
                       getIngredientByName("Mozzarella"),
                       getIngredientByName("Saumon")
                   }
               }

            };
            }
            
        }

        // GET: Pizza
        public ActionResult Index()
        {
            return View(ListePizzas);
        }

        // GET: Pizza/Details/5
        public ActionResult Details(int id)
        {
            var pizza = GetPizzaById(id);
            if (pizza != null)
            {
                return View(pizza);
            }
            return RedirectToAction("Index");
        }

        // GET: Pizza/Create
        public ActionResult Create()
        {
            var vm = new PizzaVM { pates = ListePatesDispo, ingredients = ListIngredientDispo };
            return View(vm);
        }

        // POST: Pizza/Create
        [HttpPost]
        public ActionResult Create(PizzaVM pizzaVM)
        {
            try
            {
                Pizza pizzaDB = new Pizza();
                Pizza lastPizza = ListePizzas.LastOrDefault();
                pizzaDB.Id = lastPizza.Id + 1;
                pizzaDB.Nom = pizzaVM.pizza.Nom;
                pizzaDB.Pate = getPateById(pizzaVM.selectedPate);
                foreach (var ingredient in pizzaVM.selectedIngredients)
                {
                    pizzaDB.Ingredients.Add(ListIngredientDispo.FirstOrDefault(i => i.Id == ingredient));
                }
                ListePizzas.Add(pizzaDB);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            var pizza = GetPizzaById(id);
            if(pizza != null)
            {
                var vm = new PizzaVM { pizza=pizza, pates = ListePatesDispo, ingredients = ListIngredientDispo };
                return View(vm);
            }
            return RedirectToAction("Index");
        }

        // POST: Pizza/Edit/5
        [HttpPost]
        public ActionResult Edit(PizzaVM pizzaVM)
        {
            try
            {
                Pizza pizzaDB = GetPizzaById(pizzaVM.pizza.Id);
                pizzaDB.Nom = pizzaVM.pizza.Nom;
                pizzaDB.Pate = getPateById(pizzaVM.selectedPate);

                pizzaDB.Ingredients.Clear();
                foreach (var ingredient in pizzaVM.selectedIngredients)
                {
                    pizzaDB.Ingredients.Add(ListIngredientDispo.FirstOrDefault(i => i.Id == ingredient));
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            var pizza = GetPizzaById(id);
            if (pizza != null)
            {
                return View(pizza);
            }
            return RedirectToAction("Index");
        }

        // POST: Pizza/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var pizza = GetPizzaById(id);
                ListePizzas.Remove(pizza);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
