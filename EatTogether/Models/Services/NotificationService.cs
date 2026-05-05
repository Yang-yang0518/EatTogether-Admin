using EatTogether.Models.EfModels;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Services
{
	public class NotificationService
	{
		private readonly EatTogetherDBContext _context;

		public NotificationService(EatTogetherDBContext context)
		{
			_context = context;
		}

		/// <summary>
		/// 通知中心服務
		/// 統一管理所有會員通知的建立，各業務模組只需呼叫此 Service，不需自行寫入資料庫。
		///
		/// ══════════════════════════════════════════════════════
		///  使用方式
		/// ══════════════════════════════════════════════════════
		///
		/// 1. 全員通知（文章發布、系統公告等）
		///    呼叫 SendToAllMembersAsync()，系統自動對所有正常會員批次建立通知。
		///
		///    範例：
		///    await _notificationService.SendToAllMembersAsync(
		///        type          : "NEWS",           // 通知類型（見下方類型列表）
		///        referenceType : "Article",        // 關聯資料類型
		///        referenceId   : articleId,        // 關聯資料的 Id
		///        title         : $"親愛的會員，{title}",
		///        scheduledAt   : dto.PublishDate   // 指定顯示時間，null 則為當下
		///    );
		///
		/// ──────────────────────────────────────────────────────
		///
		/// 2. 個人通知（訂位、外帶、優惠券等）
		///    呼叫 SendToMemberAsync()，只建立該會員的通知。
		///
		///    範例：
		///    await _notificationService.SendToMemberAsync(
		///        memberId      : reservation.MemberId,
		///        type          : "RESERVATION_CONFIRM",
		///        referenceType : "Reservation",
		///        referenceId   : reservation.Id,
		///        title         : $"您 {date} {time} 的訂位已確認，共 {people} 位"
		///    );
		///
		/// ══════════════════════════════════════════════════════
		///  通知類型（Type）一覽，由各模組定義並統一使用，前台依此類型決定跳轉頁面
		/// ══════════════════════════════════════════════════════
		///
		///  類型字串                    說明                  觸發位置
		///  ─────────────────────────────────────────────────────────
		///  NEWS                       文章 / 最新消息        ArticleService
		///  RESERVATION_CONFIRM        訂位確認               ReservationService（待接）
		///  RESERVATION_REMIND         訂位到店提醒           排程背景服務（待接）
		///  RESERVATION_CANCEL         訂位取消               ReservationService（待接）
		///  TAKEOUT_CREATED            外帶訂單成立           OrderService（待接）
		///  TAKEOUT_READY              外帶備餐完成           OrderService（待接）
		///  COUPON_RECEIVED            優惠券領取             CouponService（待接）
		///  COUPON_EXPIRING            優惠券即將到期         排程背景服務（待接）
		///
		/// ══════════════════════════════════════════════════════
		///  前台顯示規則（Vue BellNotification）
		/// ══════════════════════════════════════════════════════
		///
		///  - 只顯示近三個月的通知
		///  - NEWS 類型：文章 PublishDate 未到則不顯示（避免提前曝光）
		///  - 點擊通知後依 Type 跳轉對應會員頁面，未讀自動標為已讀
		///
		/// </summary>



		// ── 全員通知（文章發布用）──────────────────────────
		public async Task SendToAllMembersAsync(
			string type,
			string referenceType,
			int referenceId,
			string title,
			DateTime? scheduledAt = null)
		{
			var memberIds = await _context.Members
				.Where(m => !m.IsDeleted && !m.IsBlacklisted && m.IsConfirmed)
				.Select(m => m.Id)
				.ToListAsync();

			var createdAt = scheduledAt ?? DateTime.Now;

			var notifications = memberIds.Select(memberId => new UserNotification
			{
				MemberId = memberId,
				Type = type,
				ReferenceType = referenceType,
				ReferenceId = referenceId,
				Title = title,
				Message = null,
				IsRead = false,
				CreatedAt = createdAt
			}).ToList();

			await _context.UserNotifications.AddRangeAsync(notifications);
			await _context.SaveChangesAsync();
		}



		// ── 個人通知（訂位、外帶、優惠券等）───────────────
		public async Task SendToMemberAsync(
			int memberId,
			string type,
			string? referenceType,
			int? referenceId,
			string title,
			string? message = null)
		{
			var notification = new UserNotification
			{
				MemberId = memberId,
				Type = type,
				ReferenceType = referenceType,
				ReferenceId = referenceId,
				Title = title,
				Message = message,
				IsRead = false,
				CreatedAt = DateTime.Now
			};

			await _context.UserNotifications.AddAsync(notification);
			await _context.SaveChangesAsync();


		}
	}
}
