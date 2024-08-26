namespace TurnBasedCombatSystem
{
    public class StatusEffect
    {
        public string Name { get; private set; }
        public int Duration { get; private set; }
        public Action<Combatant> EffectAction { get; private set; }
        private readonly int _effectValue;

        public StatusEffect(string name, int duration, Action<Combatant> effectAction, int value = 0)
        {
            Name = name;
            Duration = duration;
            EffectAction = effectAction;
            _effectValue = value;
        }

        public void Apply(Combatant target)
        {
            EffectAction.Invoke(target);
        }

        public void DecrementDuration()
        {
            Duration--;
        }

        public bool IsExpired()
        {
            return Duration <= 0;
        }

        public int GetDamageValue()
        {
            if (Name == "Poison" || Name == "Burning")
            {
                return _effectValue;
            }
            return 0;
        }

        public int GetHealingValue()
        {
            if (Name == "Regeneration")
            {
                return _effectValue;
            }
            return 0;
        }
    }
}