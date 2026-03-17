using System.Collections.Generic;

namespace EatTogether.Models.DTOs
{
    public class BatchRequestDto
    {
        public List<int> Ids { get; set; } = new List<int>();
    }
}
