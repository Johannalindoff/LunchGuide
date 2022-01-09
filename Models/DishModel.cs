using System.ComponentModel.DataAnnotations;

namespace LunchGuide.Models
{
    public class DishModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public String Dish { get; set; }

        public List<String> SpecialDiet { get; set; }

        public List<SpecialDietModel> AviableSD { get; set; }
    }
}