using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;
using EatTogether.Models.Extensions;
using EatTogether.Models.Repositories;
using EatTogether.Models.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EatTogether.Models.Services
{
	public class ArticleService
	{
		private readonly IArticleRepository _repo;
		private readonly EatTogetherDBContext _context;
		private readonly NotificationService _notifyService;
		private readonly HtmlSanitizer _sanitizer;

		public ArticleService(IArticleRepository repo, EatTogetherDBContext context, NotificationService notifyService)
		{
			_repo = repo;
			_context = context;
			_notifyService = notifyService;

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



		/// <summary>
		/// 新增文章，若狀態為「發佈」，則同時發送通知給所有會員
		/// </summary>
		public async Task CreateAsync(ArticleCreateDto dto)
		{
			dto.Description = _sanitizer.Sanitize(dto.Description); // 過濾後再存
			var articleId = await _repo.CreateAsync(dto);

			// 僅通知指定類別：活動介紹(1)、餐廳公告(2)、季節限定(3)、新品上市(5)
			var notifyCategories = new List<int> { 1, 2, 3, 5 };

			if (dto.Status == 1 && notifyCategories.Contains(dto.CategoryId))
			{
				await _notifyService.SendToAllMembersAsync(
					type: "NEWS",
					referenceType: "Article",
					referenceId: articleId,
					title: $"親愛的會員，{dto.Title}",
					scheduledAt: dto.PublishDate
				);
			}

		}


		/// <summary>
		/// 取得文章類別選單
		/// </summary>
		public async Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync()
		{
			return await _repo.GetCategorySelectListAsync();
		}

		/// <summary>
		/// 取得活動選單
		/// </summary>
		public async Task<IEnumerable<SelectListItem>> GetEventSelectListAsync()
		{
			return await _repo.GetEventSelectListAsync();
		}

		/// <summary>
		/// 取得首頁列表
		/// </summary>
		public async Task<List<ArticleDto>> GetAllForIndexAsync()
		{
			return await _repo.GetAllAsync();

		}

		/// <summary>
		/// 取得文章資料
		/// </summary>
		public async Task<ArticleEditDto> GetByIdAsync(int id)
		{
			var result = await _repo.GetEditByIdAsync(id);
			if (result == null)
			{
				throw new Exception("找不到此文章");

			}
			return result;
		}

		/// <summary>
		/// 文章編輯
		/// </summary>
		public async Task EditAsync(ArticleEditDto dto)
		{
			dto.Description = _sanitizer.Sanitize(dto.Description); // 過濾後再存

			var original = await _repo.GetEditByIdAsync(dto.Id);
			var wasPublished = original?.Status == 1;

			await _repo.EditAsync(dto);

			// 僅通知指定類別：活動介紹(1)、餐廳公告(2)、季節限定(3)、新品上市(5)
			var notifyCategories = new List<int> { 1, 2, 3, 5 };

			// 原本不是發佈狀態，現在才改成發佈 → 才發通知
			if (!wasPublished && dto.Status == 1 && dto.CategoryId.HasValue && notifyCategories.Contains(dto.CategoryId.Value))
			{
				await _notifyService.SendToAllMembersAsync(
					type: "NEWS",
					referenceType: "Article",
					referenceId: dto.Id,
					title: $"親愛的會員，{dto.Title}",
					scheduledAt: dto.PublishDate
				);
			}
		}


		/// <summary>
		/// 將發佈且上架狀態的文章強制下架
		/// </summary>
		public async Task UnpublishAsync(int id)
		{
			var dto = await _repo.GetEditByIdAsync(id);
			if (dto == null) return;
			dto.Status = 2; // 2 = 已結束/已下架
			await _repo.EditAsync(dto);
		}

		/// <summary>
		/// 刪除草稿
		/// </summary>
		public async Task DeleteDraftAsync(int id)
		{
			await _repo.DeleteAsync(id);
		}

		/// <summary>
		/// 取得文章點閱數字
		/// </summary>
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
