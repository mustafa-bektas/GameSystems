namespace TurnBasedCombatSystem
{
    public class Combatant
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int AttackPower { get; private set; }
        public int Defense { get; set; }
        public List<StatusEffect> ActiveStatusEffects { get; private set; }

        public Combatant(string name, int maxHealth, int attackPower, int defense)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
            AttackPower = attackPower;
            Defense = defense;
            ActiveStatusEffects = new List<StatusEffect>();
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public void ApplyDamage(int amount)
        {
            int damage = Math.Max(0, amount - Defense);
            Health = Math.Max(0, Health - damage);
            
            Console.ForegroundColor = ConsoleColor.Red;
            DisplayMessage($"{Name} takes {damage} damage! Health is now {Health}/{MaxHealth}");
            Console.ResetColor();

            Thread.Sleep(300);
        }

        public void Heal(int amount)
        {
            Health = Math.Min(MaxHealth, Health + amount);
            
            Console.ForegroundColor = ConsoleColor.Green;
            DisplayMessage($"{Name} heals for {amount} points. Health is now {Health}/{MaxHealth}");
            Console.ResetColor();

            Thread.Sleep(300);
        }

        public void AddStatusEffect(StatusEffect effect)
        {
            ActiveStatusEffects.Add(effect);
            DisplayMessage($"{Name} is now affected by {effect.Name}");
            Thread.Sleep(200);
        }
        
        public void ProcessStatusEffects()
        {
            var groupedEffects = ActiveStatusEffects
                .GroupBy(e => e.Name)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var group in groupedEffects)
            {
                string effectName = group.Key;
                int totalDamage = 0;
                int totalHealing = 0;

                foreach (var effect in group.Value)
                {
                    if (effectName == "Poison" || effectName == "Burning")
                    {
                        totalDamage += effect.GetDamageValue();
                    }
                    else if (effectName == "Regeneration")
                    {
                        totalHealing += effect.GetHealingValue();
                    }

                    effect.DecrementDuration();
                }

                if (totalDamage > 0)
                {
                    Console.ForegroundColor = effectName == "Poison" ? ConsoleColor.Magenta : ConsoleColor.Red;
                    DisplayMessage($"{Name} is affected by {effectName} and takes {totalDamage} total damage.");

                    ApplyDamage(totalDamage);
                    DisplayMessage($"{Name}'s health is now {Health}/{MaxHealth}");
                }
                else if (totalHealing > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    DisplayMessage($"{Name} is affected by {effectName} and heals {totalHealing} total health.");

                    Heal(totalHealing);
                    DisplayMessage($"{Name}'s health is now {Health}/{MaxHealth}");
                }

                Console.ResetColor();
            }

            ActiveStatusEffects.RemoveAll(e => e.IsExpired());
        }


        private void DisplayMessage(string message, int delay = 20)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }
}
