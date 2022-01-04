namespace LunchGuide.Models
{
    public class AddDailyMenuModel
    {
        public List<DishModel> DishInfo { get; set; }
        public IEnumerable<SpecialDietModel> SpecialDietList { get; set; }
    }
}
