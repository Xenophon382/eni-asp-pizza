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

        private Ingredient GetIngredientByName(string name)
        {
            return ListIngredientDispo.FirstOrDefault(i => i.Nom == name);
        }

        private Ingredient GetIngredientById(int id)
        {
            return ListIngredientDispo.FirstOrDefault(i => i.Id == id);
        }

        private Pate GetPateByName(string name)
        {
            return ListePatesDispo.FirstOrDefault(p => p.Nom == name);
        }

        private Pate GetPateById(int id)
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
                   Pate=GetPateByName("Pate fine, base tomate"),
                   Ingredients= new List<Ingredient>
                   {
                       GetIngredientByName("Mozzarella"),
                       GetIngredientByName("Jambon"),
                       GetIngredientByName("Champignon")
                   }


               },
               new Pizza
               {
                   Id=2,
                   Nom="Saumon",
                   Pate=GetPateByName("Pate fine, base crême"),
                   Ingredients= new List<Ingredient>
                   {
                       GetIngredientByName("Mozzarella"),
                       GetIngredientByName("Saumon")
                   }
               }

            };
            }
            
        }

        private bool validationPizza(PizzaVM pizzaVM)
        {
            bool error = false;
            if(ListePizzas.Any(p => p.Nom.ToUpper() == pizzaVM.pizza.Nom.ToUpper() && p.Id != pizzaVM.pizza.Id))
            {
                error = true;
                ModelState.AddModelError("", "Une pizza portant ce nom existe déjà.");

            }
            if (pizzaVM.selectedIngredients.Count() < 2 || pizzaVM.selectedIngredients.Count() > 5)
            {
                error = true;
                ModelState.AddModelError("", "Une pizza doit avoir au minimum 2 et au maximum 5 ingrédients.");
            }
            foreach (var pizza in ListePizzas)
            {
                if (pizza.Ingredients.Select(p => p.Id).SequenceEqual(pizzaVM.selectedIngredients))
                {
                    ModelState.AddModelError("", "Une pizza comportant les mêmes ingredients existe déjà.");
                    error = true;
                }
            }

            return error;
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

                if (ModelState.IsValid)
                {
                     if(!validationPizza(pizzaVM))
                     {
                        Pizza pizzaDB = new Pizza();
                        Pizza lastPizza = ListePizzas.LastOrDefault();
                        pizzaDB.Id = lastPizza.Id + 1;
                        pizzaDB.Nom = pizzaVM.pizza.Nom;
                        pizzaDB.Pate = GetPateById(pizzaVM.selectedPate);
                        foreach (var ingredient in pizzaVM.selectedIngredients)
                        {
                            pizzaDB.Ingredients.Add(ListIngredientDispo.FirstOrDefault(i => i.Id == ingredient));
                        }
                        ListePizzas.Add(pizzaDB);
                        return RedirectToAction("Index");
                     }

                }
                pizzaVM.ingredients = Pizza.IngredientsDisponibles;
                pizzaVM.pates = Pizza.PatesDisponibles;
                return View(pizzaVM);
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
                vm.selectedIngredients = vm.pizza.Ingredients.Select(i => i.Id).ToList();
                vm.selectedPate = vm.pizza.Pate.Id;
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
                if (ModelState.IsValid)
                {
                    if (!validationPizza(pizzaVM))
                    {

                        Pizza pizzaDB = GetPizzaById(pizzaVM.pizza.Id);
                        pizzaDB.Nom = pizzaVM.pizza.Nom;
                        pizzaDB.Pate = GetPateById(pizzaVM.selectedPate);

                        pizzaDB.Ingredients.Clear();
                        foreach (var ingredient in pizzaVM.selectedIngredients)
                        {
                            pizzaDB.Ingredients.Add(ListIngredientDispo.FirstOrDefault(i => i.Id == ingredient));
                        }
                        return RedirectToAction("Index");
                    }
                }
                pizzaVM.ingredients = Pizza.IngredientsDisponibles;
                pizzaVM.pates = Pizza.PatesDisponibles;
                return View(pizzaVM);
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
