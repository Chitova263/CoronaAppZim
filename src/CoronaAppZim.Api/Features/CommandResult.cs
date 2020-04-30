namespace CoronaAppZim.Api
{
    public class CommandResult
    {
        private CommandResult() { }

        private CommandResult(string failureReason)
        {
            FailureReason = failureReason;
        }

        private CommandResult(object metadata)
        {
            Metadata = metadata;
        }



        public object Metadata { get; }
        public string FailureReason { get; }
        public bool IsSuccess => string.IsNullOrEmpty(FailureReason);

        #region Static Factory Methods

        public static CommandResult Success()
        {
            return new CommandResult();
        }

        public static CommandResult Fail(string reason)
        {
            return new CommandResult(reason);
        }

        public static CommandResult Fail(object metadata)
        {
            return new CommandResult(metadata);
        }

        #endregion

        public static implicit operator bool(CommandResult result)
        {
            return result.IsSuccess;
        }

    }
}