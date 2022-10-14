namespace Valera
{
    public sealed class ValeraMan
    {
        public Dictionary<string, (string Name, int Value)> Stats
            = new Dictionary<string, (string Name, int Value)>();
        public Dictionary<string, (string Name, List<Func<bool>> Conditions, List<Action> Consequences)> Actions
            = new Dictionary<string, (string Name, List<Func<bool>> Conditions, List<Action> Consequences)>();
        public List<(List<Func<bool>> Conditions, List<Action> Consequences)> Events
            = new List<(List<Func<bool>> Conditions, List<Action> Consequences)>();
        public static bool AllConditionsApply(List<Func<bool>> conditions) {
            foreach (Func<bool> conditionComplied in conditions) {
                if (!conditionComplied())
                    return false;
            }
            return true;
        }
        public static bool AnyConditionApplies(List<Func<bool>> conditions) {
            foreach (Func<bool> conditionComplied in conditions) {
                if (conditionComplied())
                    return true;
            }
            return false;
        }
        private static void ExecuteAction(List<Func<bool>> conditions, List<Action> consequences) {
            if (AllConditionsApply(conditions)) {
                foreach (var applyConsequence in consequences)
                    applyConsequence();
            }
        }
        public bool Do(string actionCodename) {
            if (Actions.TryGetValue(actionCodename, out var action)) {
                ExecuteAction(action.Conditions, action.Consequences);
                foreach (var eventAction in Events)
                    ExecuteAction(eventAction.Conditions, eventAction.Consequences);
                return true;
            }
            return false;
        }
    }
}
