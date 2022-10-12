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
ValeraMan valera = valeraBuilder.Build();
valera.Do("work");
foreach (var stat in valera.Stats) {
    Console.WriteLine($"{stat.Key} : {stat.Value}");
}
