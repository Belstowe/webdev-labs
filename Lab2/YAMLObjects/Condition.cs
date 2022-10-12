using YamlDotNet.Serialization;

namespace YAMLObjects
{
    public enum ComparisonOperand {
        Greater,
        Less,
        GreaterOrEqual,
        LessOrEqual,
        Equal,
        NotEqual
    }
    public sealed record Condition
    {
        private static Dictionary<string, ComparisonOperand> OperandStringToOperand = new Dictionary<string, ComparisonOperand>{
            [">"]  = ComparisonOperand.Greater,
            ["<"]  = ComparisonOperand.Less,
            [">="] = ComparisonOperand.GreaterOrEqual,
            ["<="] = ComparisonOperand.LessOrEqual,
            ["="]  = ComparisonOperand.Equal,
            ["!="] = ComparisonOperand.NotEqual
        };
        public string Stat = "";

        [YamlMember(Alias = "op", ApplyNamingConventions = false)]
        public string OperandString
        {
            set => Operand = OperandStringToOperand[value];
        }
        public ComparisonOperand Operand;
        public int Val = 0;
    }
}
