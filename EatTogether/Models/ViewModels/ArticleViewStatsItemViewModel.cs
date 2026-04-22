namespace EatTogether.Models.ViewModels
{
	public class ArticleViewStatsItemViewModel
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string CategoryName { get; set; }

		public int Status { get; set; }

		public DateTime PublishDate { get; set; }

		public int ViewCount { get; set; }
	}
}
