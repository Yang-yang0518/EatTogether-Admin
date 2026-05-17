using EatTogether.Models.EfModels;

namespace EatTogether.Models.ViewModels
{
    public class SchedulerDetailViewModel
    {
        public SchedulerLog Log { get; set; }
        public List<string> DishesEnabled { get; set; }
        public List<string> DishesDisabled { get; set; }
        public List<string> MealsEnabled { get; set; }
        public List<string> MealsDisabled { get; set; }
    }
}
