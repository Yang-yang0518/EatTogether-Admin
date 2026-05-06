using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EatTogether.Models.Repositories
{
	public interface IArticleRepository {
		Task<int> CreateAsync(ArticleCreateDto dto);
		Task<List<ArticleDto>> GetAllAsync();
		Task<ArticleEditDto> GetEditByIdAsync(int id);
		Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync();
		Task<IEnumerable<SelectListItem>> GetEventSelectListAsync();
		Task EditAsync(ArticleEditDto dto);
		Task DeleteAsync(int id);
		Task<IEnumerable<Article>> GetAllForStatsAsync();



	}
}
