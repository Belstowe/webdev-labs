namespace YAMLObjects
{
    public sealed record Action
    {
        public string Codename = "";
        public string? Name { get; set; }

        public List<Condition> Conditions = new List<Condition>();
        public List<Modifier> Result = new List<Modifier>();
    }
}
