using Valera;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YAMLObjects;

class Program
{
    public static void Main(string[] args)
    {
        var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        GameConfig configObject;
        using (var reader = new StreamReader(@"config.yaml")) {
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
        if (File.Exists(@"save.yaml")) {
            using (var reader = new StreamReader(@"save.yaml")) {
                var yamlParser = new Parser(reader);
                var saveStatsObject = yamlDeserializer.Deserialize<List<SaveState>>(yamlParser);
                foreach (var saveStat in saveStatsObject) {
                    valeraBuilder.ModifyStat(saveStat.Name, saveStat.Value);
                }
            }
        }
        ValeraMan valera = valeraBuilder.Build();

        Console.CancelKeyPress += delegate {
            var yamlSerializer = new SerializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();
            var statsToSave = valera.Stats.Select(x => new SaveState(x.Key, x.Value.Value)).ToList();
            using (var writer = new StreamWriter(@"save.yaml")) {
                yamlSerializer.Serialize(writer, statsToSave);
            }
        };

        while (true) {
            foreach (var stat in valera.Stats) {
                Console.WriteLine($"{stat.Key} : {stat.Value}");
            }
            string userInput = Console.ReadLine() ?? "";
            if (!valera.Do(userInput)) {
                Console.WriteLine($"no such action '{userInput}'!");
            }
        }
    }
}
