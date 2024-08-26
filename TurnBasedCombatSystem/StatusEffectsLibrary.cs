namespace TurnBasedCombatSystem
{
    public static class StatusEffectsLibrary
    {
        public static StatusEffect Poison => new StatusEffect(
            "Poison",
            duration: 3,
            effectAction: (combatant) =>
            {
                int poisonDamage = 5;
                combatant.ApplyDamage(poisonDamage);
            },
            value: 5);

        public static StatusEffect Stun => new StatusEffect(
            "Stun",
            duration: 1,
            effectAction: (combatant) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                DisplayMessage($"{combatant.Name} is stunned and cannot act this turn.");
                Console.ResetColor();
            });

        public static StatusEffect Regeneration => new StatusEffect(
            "Regeneration",
            duration: 3,
            effectAction: (combatant) =>
            {
                int healAmount = 5;
                combatant.Heal(healAmount);
            },
            value: 5);

        public static StatusEffect Burning => new StatusEffect(
            "Burning",
            duration: 3,
            effectAction: (combatant) =>
            {
                int burnDamage = 7;
                combatant.ApplyDamage(burnDamage);
            },
            value: 7);

        public static StatusEffect Frozen => new StatusEffect(
            "Frozen",
            duration: 2,
            effectAction: (combatant) =>
            {
                int defenseReduction = 3;
                combatant.Defense -= defenseReduction;

                Console.ForegroundColor = ConsoleColor.Cyan;
                DisplayMessage($"{combatant.Name}'s defense is reduced by {defenseReduction} due to being frozen.");
                Console.ResetColor();
            });

        private static void DisplayMessage(string message, int delay = 50)
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
