namespace ImageFormatConverter.Common;

public class CommandLineArgumentsParser
{
    public static Dictionary<string, string> GetFlagsValues(IEnumerable<string> args, string[] requiredFlags, string[] optionalFlags)
    {
        var flagValues = new Dictionary<string, string>();
        var allFlags = requiredFlags.Concat(optionalFlags).ToArray();
        foreach (var arg in args)
        {
            foreach (var flag in allFlags)
            {
                if (!arg.StartsWith($"--{flag}=")) continue;

                var flagValue = arg[(arg.IndexOf('=') + 1)..];
                if (string.IsNullOrWhiteSpace(flagValue))
                {
                    throw new Exception("Program arg is empty");
                }

                flagValues.Add(flag, flagValue);
            }
        }

        foreach (var flag in requiredFlags)
        {
            if (!flagValues.ContainsKey(flag))
            {
                throw new Exception($"{flag} is not configured");
            }
        }

        return flagValues;
    }
}