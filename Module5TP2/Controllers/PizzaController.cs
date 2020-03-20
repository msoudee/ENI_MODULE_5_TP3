using BO;
using Module5TP3.Models;
using Module5TP3.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModuleModule5TP35TP2.Controllers
{
    public class PizzaController : Controller
    {
        // GET: Pizza
        public ActionResult Index()
        {
            return View(FakeDb.Instance.Pizzas);
        }

        // GET: Pizza/Details/5
        public ActionResult Details(int id)
        {
            var pizza = FakeDb.Instance.Pizzas.FirstOrDefault(p => p.Id == id);

            return View(pizza);
        }

        // GET: Pizza/Create
        public ActionResult Create()
        {
            PizzaViewModel vm = new PizzaViewModel();

            vm.Pates = FakeDb.Instance.PatesDisponible.Select(x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() }).ToList();

            vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() }).ToList();

            return View(vm);
        }

        // POST: Pizza/Create
        [HttpPost]
        public ActionResult Create(PizzaViewModel vm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Pizza pizza = vm.Pizza;

                    pizza.Pate = FakeDb.Instance.PatesDisponible.FirstOrDefault(x => x.Id == vm.IdPate);

                    pizza.Ingredients = FakeDb.Instance.IngredientsDisponible.Where(
                        x => vm.IdsIngredients.Contains(x.Id))
                        .ToList();

                    pizza.Id = FakeDb.Instance.Pizzas.Count == 0 ? 1 : FakeDb.Instance.Pizzas.Max(x => x.Id) + 1;

                    // Test unicité nom pizza
                    if (FakeDb.Instance.Pizzas.Any(p => p.Nom.ToUpper() == pizza.Nom.ToUpper() && p.Id != pizza.Id))
                    {
                        ModelState.AddModelError("", "Il existe déjà une pizza avec ce nom");

                        vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        return View(vm);
                    }
                    // Test unicité ingrédients
                    else if(FakeDb.Instance.Pizzas.Any(p => p.Ingredients.SequenceEqual(pizza.Ingredients)))
                    {
                        ModelState.AddModelError("", "Il existe déjà une pizza avec cette liste d'ingrédients");

                        vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        return View(vm);
                    }
                    // Test nombres ingrédients entre 2 et 5
                    else if (pizza.Ingredients.Count() < 2 || pizza.Ingredients.Count() > 5)
                    {
                        ModelState.AddModelError("", "Le nombre d'ingrédient que compose une pizza doit être en 2 et 5");

                        vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        return View(vm);
                    }
                    else
                    {
                        FakeDb.Instance.Pizzas.Add(pizza);
                    }

                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            PizzaViewModel vm = new PizzaViewModel();

            vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);

            if (vm.Pizza.Pate != null)
            {
                vm.IdPate = vm.Pizza.Pate.Id;
            }

            if (vm.Pizza.Ingredients.Any())
            {
                vm.IdsIngredients = vm.Pizza.Ingredients.Select(x => x.Id).ToList();
            }

            return View(vm);
        }

        // POST: Pizza/Edit/5
        [HttpPost]
        public ActionResult Edit(PizzaViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pizza pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == vm.Pizza.Id);

                    // Test unicité nom pizza
                    if (FakeDb.Instance.Pizzas.Any(p => p.Nom.ToUpper() == pizza.Nom.ToUpper() && p.Id != pizza.Id))
                    {
                        ModelState.AddModelError("", "Il existe déjà une pizza avec ce nom");

                        vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        return View(vm);
                    }
                    // Test unicité ingrédients
                    else if (FakeDb.Instance.Pizzas.Any(p => p.Ingredients.SequenceEqual(pizza.Ingredients) && p.Id != pizza.Id))
                    {
                        ModelState.AddModelError("", "Il existe déjà une pizza avec cette liste d'ingrédients");

                        vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        return View(vm);
                    }
                    else
                    {
                        pizza.Nom = vm.Pizza.Nom;
                        pizza.Pate = FakeDb.Instance.PatesDisponible.FirstOrDefault(x => x.Id == vm.IdPate);
                        pizza.Ingredients = FakeDb.Instance.IngredientsDisponible.Where(x => vm.IdsIngredients.Contains(x.Id)).ToList();

                        return RedirectToAction("Index");
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            return View(FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id));
        }

        // POST: Pizza/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Pizza pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);
                FakeDb.Instance.Pizzas.Remove(pizza);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
