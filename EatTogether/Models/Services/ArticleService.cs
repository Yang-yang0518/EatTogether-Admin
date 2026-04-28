using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;
using EatTogether.Models.Extensions;
using EatTogether.Models.Repositories;
using EatTogether.Models.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Services
{
	public class ArticleService
	{
		private readonly IArticleRepository _repo;
		private readonly EatTogetherDBContext _context;
		private readonly HtmlSanitizer _sanitizer;

		public ArticleService(IArticleRepository repo, EatTogetherDBContext context)
		{
			_repo = repo;
			_context = context;

			// 初始化並設定html白名單，以防止XSS攻擊
			_sanitizer = new HtmlSanitizer();
			_sanitizer.AllowedTags.Clear();
			_sanitizer.AllowedTags.UnionWith(new[]
			{
				"p", "br",
				"strong", "b",
				"em", "i",
				"u",
				"span",
				"a"
			});

			_sanitizer.AllowedAttributes.Clear();
			_sanitizer.AllowedAttributes.UnionWith(new[]
			{
				"href",
				"style",
				"class"
			});

			// 限制 style（字色用）
			_sanitizer.AllowedCssProperties.Clear();
			_sanitizer.AllowedCssProperties.UnionWith(new[]
			{
				"color",
				"background-color"
			});

			// 限制 Quill 字體大小 class
			_sanitizer.AllowedClasses.Clear();
			_sanitizer.AllowedClasses.UnionWith(new[]
			{
				"ql-size-small",
				"ql-size-large",
				"ql-size-huge"
			});
		}

		public async Task CreateAsync(ArticleCreateDto dto)
		{
			dto.Description = _sanitizer.Sanitize(dto.Description); // 過濾後再存
			await _repo.CreateAsync(dto);

		}


		// 取得類別
		public async Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync()
		{
			return await _repo.GetCategorySelectListAsync();
		}

		// 取得活動
		public async Task<IEnumerable<SelectListItem>> GetEventSelectListAsync()
		{
			return await _repo.GetEventSelectListAsync();
		}

		// 取得首頁列表
		public async Task<List<ArticleDto>> GetAllForIndexAsync()
		{
			return await _repo.GetAllAsync();

		}

		// 取得資料
		public async Task<ArticleEditDto> GetByIdAsync(int id)
		{
			var result = await _repo.GetEditByIdAsync(id);
			if (result == null)
			{
				throw new Exception("找不到此文章");

			}
			return result;
		}

		public async Task EditAsync(ArticleEditDto dto)
		{
			dto.Description = _sanitizer.Sanitize(dto.Description); // 過濾後再存
			await _repo.EditAsync(dto);
		}


		// 強制下架
		public async Task UnpublishAsync(int id)
		{
			var dto = await _repo.GetEditByIdAsync(id);
			if (dto == null) return;
			dto.Status = 2; // 2 = 已結束/已下架
			await _repo.EditAsync(dto);
		}

		// 刪除草稿
		public async Task DeleteDraftAsync(int id)
		{
			await _repo.DeleteAsync(id);
		}

		//點閱統計數字
		public async Task<ArticleViewStatsViewModel> GetViewStatsAsync()
		{
			//計算統計數字（Sum、Max、Average）				
			//逐筆轉成 ItemViewModel

			var entity = await _repo.GetAllForStatsAsync();

			var vm = new ArticleViewStatsViewModel
			{
				TotalViewCount = entity.Sum(e => e.ViewCount),
				MaxViewCount = entity.Max(e => e.ViewCount),
				MaxViewTitle = entity.OrderByDescending(e => e.ViewCount).First().Title,
				AverageViewCount = entity.Where(e => e.Status == 1).Any()
						  ? entity.Where(e => e.Status == 1).Average(e => e.ViewCount)
						  : 0,
				Articles = entity.Select(e => e.ToViewStatsItemVm()).ToList()
			};

			return vm;
		}

		public async Task<IEnumerable<ArticleViewStatsDto>> GetViewStatsJsonAsync()
		{
			var entity = await _repo.GetAllForStatsAsync();
			return entity.Select(a => a.ToViewStatsJsonDto());
		}

	}
}
