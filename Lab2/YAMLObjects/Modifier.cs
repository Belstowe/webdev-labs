namespace YAMLObjects
{
    public sealed record Modifier
    {
        public string Stat = "";
        public int Val = 0;
        public List<Condition> Conditions = new List<Condition>();
    }
}
