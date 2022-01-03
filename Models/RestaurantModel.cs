using System.ComponentModel.DataAnnotations;

namespace LunchGuide.Models
{
    public class RestaurantModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Website { get; set; }

        public string OpeningHours { get; set; }

        public int Price { get; set; }

        public int University { get; set; }
    }
}
