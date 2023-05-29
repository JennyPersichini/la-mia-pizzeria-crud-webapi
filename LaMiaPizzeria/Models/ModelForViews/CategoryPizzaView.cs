using LaMiaPizzeria.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaMiaPizzeriaEfPost.Models
{
    public class CategoryPizzaView
    {
        public Pizza pizza { get; set; }

        public List<PizzaCategory>? PizzaCategories { get; set; }
    }

}
