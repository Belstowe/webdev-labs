using Valera;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YAMLObjects;

class Program
{
    private static string saveFilePath = @"save.yaml";
    private static string configFilePath = @"config.yaml";
    private static ValeraMan PrepareValera(string configFilePath, string saveFilePath)
    {
        var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        GameConfig configObject;
        using (var reader = new StreamReader(configFilePath)) {
            var yamlParser = new Parser(reader);
            configObject = yamlDeserializer.Deserialize<GameConfig>(yamlParser);
        }

        ValeraBuilder valeraBuilder = new ValeraBuilder();
        foreach (var stat in configObject.Stats) {
            valeraBuilder.AddStat(stat);
        }
        foreach (var action in configObject.Actions) {
            valeraBuilder.AddAction(action);
        }
        foreach (var deathCondition in configObject.GameOverConditions) {
            valeraBuilder.AddDeathCondition(deathCondition);
        }
        if (File.Exists(saveFilePath)) {
            using (var reader = new StreamReader(saveFilePath)) {
                var yamlParser = new Parser(reader);
                var saveStatsObject = yamlDeserializer.Deserialize<List<SaveState>>(yamlParser);
                foreach (var saveStat in saveStatsObject) {
                    valeraBuilder.ModifyStat(saveStat.Name, saveStat.Value);
                }
            }
        }
        return valeraBuilder.Build();
    }

    private static void SaveValera(ValeraMan valera, string saveFilePath)
    { 
            var yamlSerializer = new SerializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();
            var statsToSave = valera.Stats.Select(x => new SaveState(x.Key, x.Value.Value)).ToList();
            using (var writer = new StreamWriter(saveFilePath)) {
                yamlSerializer.Serialize(writer, statsToSave);
            }
    }
    public static void Main(string[] args)
    {
        ValeraMan valera = PrepareValera(configFilePath, saveFilePath);

        Console.CancelKeyPress += delegate {
            SaveValera(valera, saveFilePath);
        };

        try {
            while (true) {
                foreach (var stat in valera.Stats) {
                    Console.WriteLine($"{stat.Key} : {stat.Value}");
                }
                string userInput = Console.ReadLine() ?? "";
                if (!valera.Do(userInput)) {
                    Console.WriteLine($"no such action '{userInput}'!");
                }
            }
        } catch (Exception) {
            SaveValera(valera, saveFilePath);
        }
    }
}
