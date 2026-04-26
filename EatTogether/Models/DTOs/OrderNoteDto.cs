using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EatTogether.Models.DTOs
{
    public class OrderNoteDto
    {
        [JsonPropertyName("order")]
        public string? Order { get; set; }

        [JsonPropertyName("items")]
        public Dictionary<string, string> Items { get; set; } = new();
    }
}
