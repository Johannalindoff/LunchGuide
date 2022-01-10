using System.ComponentModel.DataAnnotations;

namespace LunchGuide.Models
{
    public class MenuModel
    {
        public int Id { get; set; }
        public String Restaurant { get; set; }

        public DateTime Date { get; set; }

        public String Dish { get; set; }
        public String DailyMenu { get; set; }

        public List<String> SpecialDiet { get; set; }

    }
}
