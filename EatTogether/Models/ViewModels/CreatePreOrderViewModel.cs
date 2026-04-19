using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EatTogether.Models.ViewModels
{
    public class CreatePreOrderViewModel
    {
        public int TableId { get; set; }
        public bool InOrOut { get; set; } = true;   // 預設內用
        public int? PeopleNum { get; set; }
        public bool IsAddOrder { get; set; }
        public List<SelectListItem> TableOptions { get; set; } = new();
        public string? Note { get; set; }
        /// <summary>前端把個別餐點備註序列化成 {"炸雞腿":"不要皮"} 存在這裡</summary>
        public string? ItemNotesJson { get; set; }
        public int DiscountAmount { get; set; }
        public int? MemberId { get; set; }
        public int? UserId { get; set; }
        public int? CouponId { get; set; }
        public int? EventId { get; set; }
        public List<CreatePreOrderItemViewModel> Items { get; set; } = new();
    }
}
