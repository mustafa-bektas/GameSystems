using System;

namespace TurnBasedCombatSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Combatant player = new Combatant("Player", 100, 15, 5);
            Combatant enemy = new Combatant("Enemy", 80, 12, 4);

            CombatSystem combatSystem = new CombatSystem(player, enemy);
            combatSystem.StartBattle();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}