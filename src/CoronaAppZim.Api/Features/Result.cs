namespace CoronaAppZim.Api
{
    public class Result
    {
        protected Result() { }

        protected Result(string failureReason)
        {
            FailureReason = failureReason;
        }

        public string FailureReason { get; }
        public bool IsSuccess => string.IsNullOrEmpty(FailureReason);
        public bool IsFailure => !IsSuccess;

        public static Result Success()
        {
            return new Result();
        }

        public static Result Fail(string failureReason)
        {
            return new Result(failureReason);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value) 
        {
            Value = value;
        }

        public static new Result<T> Fail(string failureReason)
        {
            return new Result<T>(default(T));
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }
    }
}