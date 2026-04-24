namespace EatTogether.Models.DTOs
{
	public class ArticleViewStatsDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string CategoryName { get; set; }
		public string StatusLabel { get; set; }
		public string PublishDate { get; set; }
		public int ViewCount { get; set; }
	}
}
