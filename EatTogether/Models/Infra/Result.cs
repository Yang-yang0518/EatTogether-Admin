namespace EatTogether.Models.Infra
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string ErrorMesssage { get; set; }

        public static Result Success()
            => new Result { IsSuccess = true };

        public static Result Fail(string msg)
                => new Result { IsSuccess = false, ErrorMesssage = msg };
    }

	// 泛型版本，多一個 Value 存回傳資料
	public class Result<T>
	{
		public bool IsSuccess { get; set; }
		public string ErrorMessage { get; set; } = "";
		public T? Value { get; set; }

		public static Result<T> Success(T value)
			=> new Result<T> { IsSuccess = true, Value = value };

		public static Result<T> Fail(string msg)
			=> new Result<T> { IsSuccess = false, ErrorMessage = msg };
	}
}
