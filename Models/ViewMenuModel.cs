using System;
namespace LunchGuide.Models
{
    public class ViewMenuModel
    {
        public ViewMenuModel(){}

        public List<DishModel> DishList { get; set; }
        public RestaurantModel Restaurant { get; set; }
    }
}
