using EatTogether.Models.EfModels;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Repositories
{
	public interface IRoleRepository
	{
		Task<List<string>> GetRoleNamesByIdsAsync(List<int> roleIds);
	}

	public class RoleRepository : IRoleRepository
	{
		private readonly EatTogetherDBContext _context;

		public RoleRepository(EatTogetherDBContext context)
		{
			_context = context;
		}

		public async Task<List<string>> GetRoleNamesByIdsAsync(List<int> roleIds)
		{
			return await _context.Roles
				.AsNoTracking()
				.Where(r => roleIds.Contains(r.Id))
				.Select(r => r.RoleName)
				.ToListAsync();
		}
	}
}
