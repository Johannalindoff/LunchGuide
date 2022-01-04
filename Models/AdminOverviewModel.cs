namespace LunchGuide.Models
{
    public class AdminOverviewModel
    {
        public RestaurantModel RestaurantInfo { get; set; }
        public List<DishModel> DishInfo { get; set; }
    }
}
