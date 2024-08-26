using System;

namespace TurnBasedCombatSystem
{
    public class CombatSystem
    {
        public Combatant Player { get; private set; }
        public Combatant Enemy { get; private set; }

        public CombatSystem(Combatant player, Combatant enemy)
        {
            Player = player;
            Enemy = enemy;
        }

        public void StartBattle()
        {
            Console.WriteLine("Battle begins!");

            while (Player.IsAlive() && Enemy.IsAlive())
            {
                PlayerTurn();
                if (!Enemy.IsAlive()) break;
                EnemyTurn();
            }

            Combatant winner = GetWinner();
            Console.WriteLine($"{winner.Name} wins the battle!");
        }

        private void PlayerTurn()
        {
            Console.WriteLine($"\n{Player.Name}'s turn.");
            Player.ProcessStatusEffects();
            if (!Player.IsAlive()) return;

            Console.WriteLine("Choose an action:");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Apply Poison");
            Console.WriteLine("3. Apply Regeneration");
            Console.WriteLine("4. Skip Turn");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Enemy.ApplyDamage(Player.AttackPower);
                    break;
                case "2":
                    Enemy.AddStatusEffect(StatusEffectsLibrary.Poison);
                    break;
                case "3":
                    Player.AddStatusEffect(StatusEffectsLibrary.Regeneration);
                    break;
                case "4":
                    Console.WriteLine($"{Player.Name} skips the turn.");
                    break;
                default:
                    Console.WriteLine("Invalid choice! Turn skipped.");
                    break;
            }
        }

        private void EnemyTurn()
        {
            Console.WriteLine($"\n{Enemy.Name}'s turn.");
            Enemy.ProcessStatusEffects();
            if (!Enemy.IsAlive()) return;

            Random rand = new Random();
            int action = rand.Next(1, 4);

            switch (action)
            {
                case 1:
                    Console.WriteLine($"{Enemy.Name} attacks!");
                    Player.ApplyDamage(Enemy.AttackPower);
                    break;
                case 2:
                    Console.WriteLine($"{Enemy.Name} applies Burning!");
                    Player.AddStatusEffect(StatusEffectsLibrary.Burning);
                    break;
                case 3:
                    Console.WriteLine($"{Enemy.Name} applies Regeneration!");
                    Enemy.AddStatusEffect(StatusEffectsLibrary.Regeneration);
                    break;
            }
        }

        public Combatant GetWinner()
        {
            return Player.IsAlive() ? Player : Enemy;
        }
    }
}
