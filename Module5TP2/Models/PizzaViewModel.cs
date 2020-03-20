using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Module5TP3.Models
{
    public class PizzaViewModel
    {
        public Pizza Pizza { get; set; }
        public List<SelectListItem> Pates { get; set; }
        public List<SelectListItem> Ingredients { get; set; }

        public int IdPate { get; set; }
        public List<int> IdsIngredients { get; set; } = new List<int>();
    }
}