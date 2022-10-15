namespace YAMLObjects
{
    public sealed record GameConfig
    {
        public List<Stat> Stats = new List<Stat>();
        public List<Action> Actions = new List<Action>();
        public List<GameOverCondition> GameOverConditions = new List<GameOverCondition>();
    }
}
