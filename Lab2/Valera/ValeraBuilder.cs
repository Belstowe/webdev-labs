namespace Valera
{
    public sealed class ValeraBuilder
    {
        private ValeraMan _valera = new ValeraMan();

        public ValeraBuilder() {
            _valera = new ValeraMan();
        }
        
        private Func<bool> TranslateCondition(YAMLObjects.Condition condition) {
            switch (condition.Operand) {
                case YAMLObjects.ComparisonOperand.Greater:
                    return () => _valera.Stats[condition.Stat].Value > condition.Val;
                case YAMLObjects.ComparisonOperand.Less:
                    return () => _valera.Stats[condition.Stat].Value < condition.Val;
                case YAMLObjects.ComparisonOperand.GreaterOrEqual:
                    return () => _valera.Stats[condition.Stat].Value >= condition.Val;
                case YAMLObjects.ComparisonOperand.LessOrEqual:
                    return () => _valera.Stats[condition.Stat].Value <= condition.Val;
                case YAMLObjects.ComparisonOperand.Equal:
                    return () => _valera.Stats[condition.Stat].Value == condition.Val;
                case YAMLObjects.ComparisonOperand.NotEqual:
                    return () => _valera.Stats[condition.Stat].Value != condition.Val;
            }
            return () => false;
        }
        private Action TranslateModifier(YAMLObjects.Modifier modifier) {
            if (modifier.Conditions.Count != 0) {
                var conditionFunctions = modifier.Conditions.Select(cond => TranslateCondition(cond)).ToList();
                return () => _valera.Stats[modifier.Stat] = _valera.Stats[modifier.Stat] with {
                    Value = _valera.Stats[modifier.Stat].Value + (ValeraMan.AllConditionsApply(conditionFunctions) ? modifier.Val : 0)
                };
            }
            else return () =>
                _valera.Stats[modifier.Stat] = _valera.Stats[modifier.Stat] with { Value = _valera.Stats[modifier.Stat].Value + modifier.Val };
        }
        public void AddStat(YAMLObjects.Stat stat) {
            _valera.Stats.Add(stat.Codename, (stat.Name ?? stat.Codename, stat.Start));
            if (stat.Min != null) {
                _valera.Events.Add((
                    Conditions: new List<Func<bool>>{ () => _valera.Stats[stat.Codename].Value < stat.Min },
                    Consequences: new List<Action>{ () =>
                        _valera.Stats[stat.Codename] = _valera.Stats[stat.Codename] with { Value = stat.Min ?? 0 } }
                ));
            }
            if (stat.Max != null) {
                _valera.Events.Add((
                    Conditions: new List<Func<bool>>{ () => _valera.Stats[stat.Codename].Value > stat.Max },
                    Consequences: new List<Action>{ () =>
                        _valera.Stats[stat.Codename] = _valera.Stats[stat.Codename] with { Value = stat.Max ?? 0 } }
                ));
            }
        }
        public void AddAction(YAMLObjects.Action action) {
            _valera.Actions.Add(action.Codename, (
                Name: action.Name ?? action.Codename,
                Conditions: action.Conditions.Select(cond => TranslateCondition(cond)).ToList(),
                Consequences: action.Result.Select(conseq => TranslateModifier(conseq)).ToList()
            ));
        }
        public ValeraMan Build() {
            return _valera;
        }
    }
}
