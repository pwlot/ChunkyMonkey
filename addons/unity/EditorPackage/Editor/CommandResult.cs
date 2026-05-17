namespace ChunkyMonkey.Unity
{
    internal sealed class CommandResult
    {
        public bool Ok { get; private set; }
        public string Output { get; private set; }

        public static CommandResult Success(string output)
        {
            return new CommandResult { Ok = true, Output = output ?? string.Empty };
        }

        public static CommandResult Fail(string output)
        {
            return new CommandResult { Ok = false, Output = output ?? string.Empty };
        }
    }
}
