using System.Reflection;
using System.Text.Json;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Business.Utilities;

public static class SetupConfiguration
{
    public static SetupConfigurationContext ReadConfiguration()
    {
        var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var fileName = Path.Join(workingDirectory, "Configuration", "Configuration.json");
        var jsonString = File.ReadAllText(fileName);
        var configuration = JsonSerializer.Deserialize<SetupConfigurationContext>(jsonString);

        if (configuration is null)
        {
            Console.WriteLine("Configuration empty? ");
            Environment.Exit(-1);
        }

        return configuration;
    }
}
