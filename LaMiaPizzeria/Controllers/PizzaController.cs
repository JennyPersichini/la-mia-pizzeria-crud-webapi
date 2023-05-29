using LaMiaPizzeria.Database;
using LaMiaPizzeria.Models;
using LaMiaPizzeriaEfPost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Composition;

namespace LaMiaPizzeria.Controllers
{
    public class PizzaController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

        [Authorize]
        public IActionResult Index()
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                List<Pizza> pizzas = db.Pizze.ToList();
                return View("Index", pizzas);
            }

        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Dettagli(string nome)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza pizza = db.Pizze.Where(p => p.Nome == nome).Include(c => c.Categoria).FirstOrDefault();

                if (pizza != null)
                {
                    return View(pizza);
                }

                return NotFound("Pizza non trovata!!");
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {

                List<PizzaCategory> pizzaCategory = db.PizzaCategories.ToList();

                CategoryPizzaView model = new CategoryPizzaView();
                model.pizza = new Pizza();
                model.PizzaCategories = pizzaCategory;


                return View("Create", model);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryPizzaView formData)
        {
            if (!ModelState.IsValid)
            {
                using PizzeriaContext db = new();

                List<PizzaCategory> pizzaCategory = db.PizzaCategories.ToList();
                formData.PizzaCategories = pizzaCategory;

                return View("Create", formData);
            }

            using (PizzeriaContext db = new PizzeriaContext())
            {
                db.Pizze.Add(formData.pizza);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Update(string nome)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza pizza = db.Pizze.Where(p => p.Nome == nome).FirstOrDefault();

                if (pizza == null)
                {
                    return NotFound("Pizza non trovata!!");
                }

                List<PizzaCategory> pizzaCategory = db.PizzaCategories.ToList();
                CategoryPizzaView Modello = new();
                Modello.pizza = pizza;
                Modello.PizzaCategories = pizzaCategory;

                return View("Update", Modello);
            }

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(CategoryPizzaView formData)
        {
            if (!ModelState.IsValid)
            {
                using PizzeriaContext db = new();

                List<PizzaCategory> pizzaCategory = db.PizzaCategories.ToList();
                formData.PizzaCategories = pizzaCategory;

                return View("Update", formData);
            }

            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza pizza = db.Pizze.Where(p => p.Nome == formData.pizza.Nome).FirstOrDefault();

                if (pizza != null)
                {
                    pizza.Descrizione = formData.pizza.Descrizione;
                    pizza.Prezzo = formData.pizza.Prezzo;
                    pizza.Immagine = formData.pizza.Immagine;
                    pizza.PizzaCategoryId = formData.pizza.PizzaCategoryId;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Pizza non trovata!!");
                }

            }

        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {

            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaToDelete = db.Pizze.Where(Pizza => Pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    db.Remove(pizzaToDelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Pizza non trovata!!");
                }
            }

        }
    }
}
