using System;

namespace CoronaAppZim.Api
{
    public class Result
    {
        protected Result(bool isSuccess, string failureReason)
        {
            if (isSuccess && !String.IsNullOrWhiteSpace(failureReason))
                throw new InvalidOperationException("operation is invalid");

            if (!isSuccess && String.IsNullOrWhiteSpace(failureReason))
                throw new InvalidOperationException("operation is invalid");

            IsSuccess = isSuccess;
            FailureReason = failureReason;
        }

        public string FailureReason { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public static Result Fail(string failureReason)
        {
            return new Result(false, failureReason);
        }

        public static Result<T> Fail<T>(string failureReason)
        {
            return new Result<T>(false, default(T), failureReason);
        }

        public static Result Success()
        {
            return new Result(true, String.Empty);
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(true, value, String.Empty);
        }

        public static implicit operator bool(Result result)
        {
            return result.IsSuccess;
        }
    }

    public class Result<T> : Result
    {
        protected internal Result(bool isSuccess, T value, string failureReason)
            : base(isSuccess, failureReason)
        {
            Value = value;
        }

        public T Value { get; }
    }
}