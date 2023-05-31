using LaMiaPizzeria.Database;
using LaMiaPizzeria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaMiaPizzeria.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaAPIController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPizzas()
        {

            using (PizzeriaContext db = new PizzeriaContext())
            {
                List<Pizza> Pizze = db.Pizze.ToList();
                return Ok(Pizze);
            }

        }

        [HttpGet]
        public IActionResult SearchByName(string nome)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaToSearch = db.Pizze.Where(pizza => pizza.Nome.Contains(nome)).FirstOrDefault();

                if (pizzaToSearch != null)
                {
                    return Ok(pizzaToSearch);
                }
                else
                {
                    return NotFound();
                }

            }

        }

        [HttpGet("{id}")]
        public IActionResult SearchById(int id)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaById = db.Pizze.Where(p => p.Id == id).Include(p => p.Categoria).FirstOrDefault();
                
                if (pizzaById != null)
                {
                    return Ok(pizzaById);
                }
                else
                {
                    return NotFound();
                }

            }

        }

        [HttpPost]
        public IActionResult Create([FromBody] Pizza pizza)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                using (PizzeriaContext db = new PizzeriaContext())
                {
                    db.Pizze.Add(pizza);
                    db.SaveChanges();

                    return Ok();
                }

            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaToDelete = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    db.Remove(pizzaToDelete);
                    db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound("Pizza non trovata!!");

                }

            }

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Pizza data)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaToModify = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToModify != null)
                {
                    pizzaToModify.Immagine = data.Immagine;
                    pizzaToModify.Nome = data.Nome;
                    pizzaToModify.Descrizione = data.Descrizione;
                    pizzaToModify.Prezzo = data.Prezzo;

                    db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }

        }

    }

}