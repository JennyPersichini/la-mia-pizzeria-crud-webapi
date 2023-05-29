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

        [HttpGet("{name}")]
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

    }

}
