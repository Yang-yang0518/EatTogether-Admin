using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;
using EatTogether.Models.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Repositories
{
	public class ArticleRepository : IArticleRepository
	{
		private readonly EatTogetherDBContext _context;

		public ArticleRepository(EatTogetherDBContext context)
		{
			_context = context;
		}


		public async Task CreateAsync(ArticleCreateDto dto)
		{
			var article = dto.ToEntity();

			_context.Articles.Add(article);
			await _context.SaveChangesAsync();
		}


		public async Task<List<ArticleDto>> GetAllAsync()
		{

			var entity = await _context.Articles
						   .AsNoTracking()
						   .Include(e => e.Category)
						   .Include(e => e.Event)
						   .ToListAsync();

			return entity.Select(e => e.ToArticleDto()).ToList();
		}

		public async Task<ArticleEditDto> GetEditByIdAsync(int id)
		{

			var entity = await _context.Articles
						   .Include(a => a.Category)
						   .Include(a => a.Event)
						   .FirstOrDefaultAsync(a => a.Id == id);

			if (entity == null) return null;
			return entity.ToEditDto();

		}

		public async Task EditAsync(ArticleEditDto dto)
		{
			var entity = await _context.Articles.FindAsync(dto.Id);
			if (entity == null) return;

			entity.CategoryId = dto.CategoryId.GetValueOrDefault();
			entity.EventId = dto.EventId;
			entity.Title = dto.Title;
			entity.Description = dto.Description;
			entity.CoverImageUrl = dto.CoverImageUrl;
			entity.PublishDate = dto.PublishDate;
			entity.ExpiryDate = dto.ExpiryDate;   
			entity.IsPinned = dto.IsPinned;       
			entity.Status = dto.Status;           

			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await _context.Articles.FindAsync(id);
			if (entity == null) return;
			_context.Articles.Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Article>> GetAllForStatsAsync()
		{
			return await _context.Articles
						.Include(e => e.Category)
						.Where(e => e.Status != 0)
						.OrderByDescending(e => e.ViewCount)
						.ToListAsync();
		}

		public async Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync()
		{
			return await _context.ArticleCategories
				.Where(x => x.IsEnabled) // 撈啟用的
				.OrderBy(x => x.SortOrder)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.Name
				}).ToListAsync();
		}

		public async Task<IEnumerable<SelectListItem>> GetEventSelectListAsync()
		{
			return await _context.Events
				.Where(x => x.Status == 1 || x.Status == 0) // 取進行中及未開始活動
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.Title
				}).ToListAsync();
		}
	}
}
