using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;

namespace EatTogether.Models.Repositories
{
	public interface IArticleRepository {
		Task CreateAsync(ArticleCreateDto dto);
		Task<List<ArticleDto>> GetAllAsync();
		Task<ArticleEditDto> GetEditByIdAsync(int id);
		Task EditAsync(ArticleEditDto dto);
		Task DeleteAsync(int id);
		Task<IEnumerable<Article>> GetAllForStatsAsync();



	}
}
