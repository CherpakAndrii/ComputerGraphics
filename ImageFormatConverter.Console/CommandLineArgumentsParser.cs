namespace ImageFormatConverter.Console;

public class CommandLineArgumentsParser
{
    public Dictionary<string, string> GetFlagsValues(IEnumerable<string> args, params string[] flags)
    {
        var flagValues = new Dictionary<string, string>();
        foreach (var arg in args)
        {
            foreach (var flag in flags)
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

        return flagValues;
    }
}