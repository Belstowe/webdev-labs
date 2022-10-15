namespace YAMLObjects
{
    public sealed record GameOverCondition {
        public string Message = "";
        public List<Condition> Conditions = new List<Condition>();
    }
}
