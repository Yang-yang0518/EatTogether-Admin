using EatTogether.Models.DTOs;
using System.Collections.Generic;
using System.Text.Json;

namespace EatTogether.Models.Infra
{
    public static class OrderNoteHelper
    {
        private static readonly JsonSerializerOptions _opts = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        /// <summary>
        /// 安全解析 Note 欄位。
        /// 舊格式純字串 → 當作 Order 備註，Items 為空。
        /// 新格式 JSON → 正常反序列化。
        /// </summary>
        public static OrderNoteDto Parse(string? note)
        {
            if (string.IsNullOrWhiteSpace(note))
                return new OrderNoteDto();

            var trimmed = note.Trim();
            if (!trimmed.StartsWith("{"))
                return new OrderNoteDto { Order = note };   // 舊格式相容

            try
            {
                return JsonSerializer.Deserialize<OrderNoteDto>(trimmed, _opts)
                       ?? new OrderNoteDto();
            }
            catch
            {
                return new OrderNoteDto { Order = note };   // 解析失敗也不爆錯
            }
        }

        /// <summary>
        /// 將整筆備註＋個別餐點備註組成 JSON 字串。
        /// 若兩者皆空則回傳 null（不存空 JSON）。
        /// </summary>
        public static string? Build(string? orderNote, Dictionary<string, string>? itemNotes)
        {
            var dto = new OrderNoteDto
            {
                Order = string.IsNullOrWhiteSpace(orderNote) ? null : orderNote.Trim(),
                Items = itemNotes ?? new()
            };

            // 兩者都空時不存任何資料
            if (dto.Order == null && dto.Items.Count == 0)
                return null;

            return JsonSerializer.Serialize(dto, _opts);
        }
    }
}
