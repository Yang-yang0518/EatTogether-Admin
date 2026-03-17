using EatTogether.Models.EfModels;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Repositories
{
	public interface IRoleFunctionRepository
	{
		Task<IEnumerable<string>> GetFunctionNamesByRoleIdsAsync(List<int> roleIds);
	}

	public class RoleFunctionRepository : IRoleFunctionRepository
	{
		private readonly EatTogetherDBContext _context;

		public RoleFunctionRepository(EatTogetherDBContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<string>> GetFunctionNamesByRoleIdsAsync(List<int> roleIds)
		{
			return await _context.RoleFunctions
				.Where(rf => roleIds.Contains(rf.RoleId))
				.Select(rf => rf.Function.FunctionName)
				.Distinct()
				.ToListAsync();
		}
	}
}
