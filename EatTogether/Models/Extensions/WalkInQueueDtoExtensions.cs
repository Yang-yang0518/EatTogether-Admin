using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;

namespace EatTogether.Models.Extensions
{
    public static class WalkInQueueDtoExtensions
    {
        // EFModel → DTO
        public static WalkInQueueDto ToDto(this WalkInQueue q, string? tableName = null)
        {
            return new WalkInQueueDto
            {
                Id           = q.Id,
                QueueNumber  = q.QueueNumber,
                Name         = q.Name,
                Phone        = q.Phone,
                AdultsCount  = q.AdultsCount,
                ChildrenCount = q.ChildrenCount,
                Status       = q.Status,
                Remark       = q.Remark,
                RegisteredAt = q.RegisteredAt,
                CalledAt     = q.CalledAt,
                SeatedAt     = q.SeatedAt,
                LeftAt       = q.LeftAt,
                MemberId     = q.MemberId,
                TableId      = q.TableId,
                TableName    = tableName
            };
        }
    }
}
