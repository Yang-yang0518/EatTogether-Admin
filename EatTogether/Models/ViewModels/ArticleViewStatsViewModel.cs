namespace EatTogether.Models.ViewModels
{
	public class ArticleViewStatsViewModel
	{
		public int TotalViewCount { get; set; }          // 所有文章總點閱

		public int MaxViewCount { get; set; }            // 最高點閱數

		public string MaxViewTitle { get; set; }         // 最高點閱的文章標題

		public int ZeroViewCount { get; set; }           // 零點閱文章篇數

		public double AverageViewCount { get; set; }     // 已發佈文章平均點閱

		public IEnumerable<ArticleViewStatsItemViewModel> Articles { get; set; }

	}
}
