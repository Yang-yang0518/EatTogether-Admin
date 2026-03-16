using EatTogether.Models.EfModels;

namespace EatTogether.Models.DTOs
{
	public class UserDto
	{
		public int Id { get; set; }

		public string Account { get; set; } = "";

		public string HashedPassword { get; set; } = "";

		public string Name { get; set; } = "";

		public bool IsActive { get; set; }

		public bool IsDeleted { get; set; }

		public bool MustChangePassword { get; set; }

		public List<int> RoleIds { get; set; } = new();
	}
}
