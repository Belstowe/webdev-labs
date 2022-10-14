using Valera;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YAMLObjects;

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
valera.Do("work");
foreach (var stat in valera.Stats) {
    Console.WriteLine($"{stat.Key} : {stat.Value}");
}
