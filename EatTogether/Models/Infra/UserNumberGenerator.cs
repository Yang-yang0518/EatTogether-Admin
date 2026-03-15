using EatTogether.Models.EfModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using System.Data.Common;

namespace EatTogether.Models.Infra
{
	public class UserNumberGenerator
	{
		private readonly EatTogetherDBContext _context;

		public UserNumberGenerator(EatTogetherDBContext context)
		{
			_context = context;
		}

		public async Task<string> GenerateAsync()
		{
			int currentYear = DateTime.Now.Year;
			string prefix = $"EMP{currentYear}";


			// 確保「查詢最大值」到「寫入新資料」之間沒有其他請求插入
			// UPDLOCK：查詢時對資料列加上更新鎖，防止其他交易同時讀取並修改
			// HOLDLOCK：將鎖持續保留到交易結束，而非查詢完就釋放
			string sql = @"
SELECT MAX(EmployeeNumber)
FROM Users WITH(UPDLOCK, HOLDLOCK)
WHERE EmployeeNumber LIKE @prefix
AND LEN(EmployeeNumber) = 10
";
			var conn = _context.Database.GetDbConnection();
			await conn.OpenAsync();

			using var cmd = conn.CreateCommand();
			// 將目前的 Transaction 綁定到指令上（鎖才會生效）
			cmd.Transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
			cmd.CommandText = sql;

			var param = cmd.CreateParameter();
			param.ParameterName = "@prefix";
			param.Value = prefix + "%";
			cmd.Parameters.Add(param);

			var result = await cmd.ExecuteScalarAsync();


			int nextNumber = 1;

			if(result is string MaxNumber
				&& MaxNumber.Length == 10
				&& int.TryParse(MaxNumber.Substring(7), out int lastNumber))
			{
				nextNumber = lastNumber + 1;
			}

			// nextNumber 最少 3 位數，不足補零
			return $"{prefix}{nextNumber:D3}";

		}
	}
}
