using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace GameManager
{
    class Program
    {
        private static Stats player;
        private static Stats enemy;
        static GamePhase State;
        
        static void Main(string[] args)
        {
            Console.WriteLine(Title());
            UpdatingGameState(GamePhase.SelectCharacter);
        }


        public static string Title()
        {
            return @"
______                _                   _____                      
| ___ \              | |                 |  __ \                     
| |_/ /__ _ _ __   __| | ___  _ __ ___   | |  \/ __ _ _ __ ___   ___ 
|    // _` | '_ \ / _` |/ _ \| '_ ` _ \  | | __ / _` | '_ ` _ \ / _ \
| |\ \ (_| | | | | (_| | (_) | | | | | | | |_\ \ (_| | | | | | |  __/
\_| \_\__,_|_| |_|\__,_|\___/|_| |_| |_|  \____/\__,_|_| |_| |_|\___|
                                                                     
Welcome to my first c# project. Don't try to break anything. Thanks.
                                                                    ";
        }
        public static void UpdatingGameState(GamePhase newState)
        {
            State = newState;
            switch (newState)
            {
                case GamePhase.SelectCharacter:
                    IntroductionToTheGame();
                    break;
                case GamePhase.PlayerTurn:
                    PlayerTurn();
                    break;
                case GamePhase.DamageCalc:
                    break;
                case GamePhase.EnemyTurn:
                    EnemyTurn();
                    break;
                case GamePhase.Win:
                    YouWin();
                    break;
                case GamePhase.Lose:
                    YouLose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
        private static void IntroductionToTheGame()
        {
            string[] devilNames = {"Abaddon", "Adramalech", "Ahpuch", "Ahriman", "Apollyon", "Asmodeus"};
            var rngEnemyName = new Random();
            var selectedEnemy = rngEnemyName.Next(devilNames.Length);
            
            int answer;
            Console.WriteLine("What's your name?");
            string playerName = Console.ReadLine();
            Console.WriteLine($"It's {playerName} the name you want?");
            Console.WriteLine("1 - Yes / 2 - No");
            answer = Int32.Parse(Console.ReadLine());
            while (answer != 1)
            {
                Console.WriteLine("Then what's your name?");
                playerName = Console.ReadLine();
                Console.WriteLine($"It's {playerName} the name you want?");
                Console.WriteLine("1 - Yes / 2 - No");
                answer = Int32.Parse(Console.ReadLine());
            }
            
            player = new Stats(playerName);
            Console.Clear();
            Console.WriteLine($"Welcome, {player.GetName()}.");
            enemy = new Stats(devilNames[selectedEnemy]);
            Console.WriteLine("You're about to fight against: " + enemy.GetName());
            Console.WriteLine("");
            Console.WriteLine("Accept the fight?");
            var lastAnswer = Console.ReadLine();
            if (lastAnswer == "no") Environment.Exit(1);

            UpdatingGameState(GamePhase.PlayerTurn);
        }
        private static void PlayerTurn()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"{player.GetName()}'s Level: {player.GetLevel().ToString()} VS {enemy.GetName()}'s  Level: {enemy.GetLevel().ToString()}");
            Console.WriteLine("");
            Console.WriteLine("It's your turn.");
            Console.WriteLine("");
            Console.WriteLine($"{player.GetName()}\n{player.GetHealth().ToString()}/{player.GetMaxHealth().ToString()} HP");
            Console.WriteLine("1 - Attack    2 - Raise defense");
            int choice = Int32.Parse(Console.ReadLine());
            DamageCalc(choice,"player");
            
        }

        private static void DamageCalc(int inputchoice, string damageDealer)
        {
            UpdatingGameState(GamePhase.DamageCalc);
            Stats damager = null;
            Stats damaged = null;
            if (damageDealer == "enemy")
            {
                 damager = enemy;
                 damaged = player;
            }
            else if (damageDealer == "player")
            {
                 damager = player;
                 damaged = enemy;
            }
                
            switch (inputchoice)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine($"{damager.GetName()} attacked {damaged.GetName()}");
                    damaged.TakeDamage(damager.DoDamage());
                    if (damaged.GetHealth() <= 0) damaged.SetHealth(0);
                    Console.WriteLine("");
                    Console.WriteLine($"{damaged.GetName()} received {(int)(damager.GetAttack() * damaged.GetArmor())} damage points");
                    Console.WriteLine($"[{damaged.GetHealth().ToString()}/{damaged.GetMaxHealth().ToString()} HP] left");
                    break;
                case 2:
                    Console.Clear();
                    if (damager.GetMaxArmor() == false)
                    {
                        Console.WriteLine(damager.GetName() + "'s Defense raised by 10%.");
                        damager.SetArmor(0.1f);
                        Console.WriteLine("");
                        Console.WriteLine($"{player.GetName()}: [{player.GetHealth().ToString()}/{player.GetMaxHealth().ToString()}]");
                        Console.WriteLine($"{enemy.GetName()}: [{enemy.GetHealth().ToString()}/{enemy.GetMaxHealth().ToString()}]");
                    }
                    else{
                        Console.WriteLine("");
                        Console.WriteLine(">>> Max Armor Reached. <<<");
                    } 
                    break;
                
                default:
                    throw new IndexOutOfRangeException("Dude plz.");
            }
            Console.ReadLine();
            if (enemy.GetHealth() <= 0) UpdatingGameState(GamePhase.Win);
            else if (player.GetHealth() <= 0) UpdatingGameState(GamePhase.Lose);
            else if (damageDealer == "enemy" && damaged.GetHealth() >= 0) UpdatingGameState(GamePhase.PlayerTurn);
            else if (damageDealer == "player" && damaged.GetHealth() >= 0) UpdatingGameState(GamePhase.EnemyTurn);
        }

        private static void EnemyTurn()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"{player.GetName()}'s Level: {player.GetLevel().ToString()} VS {enemy.GetName()}'s  Level: {enemy.GetLevel().ToString()}");
            Console.WriteLine("");
            Console.WriteLine($"It's {enemy.GetName()} turn.");
            Console.WriteLine("");
            Console.WriteLine($"{enemy.GetName()}\n{enemy.GetHealth().ToString()}/{enemy.GetMaxHealth().ToString()} HP");
            Console.WriteLine("1 - Attack    2 - Raise defense");
            var rng = new Random();
            int choiceRng;
            if (enemy.GetMaxArmor()) choiceRng = 1;
            else choiceRng = rng.Next(1, 3);
            Console.WriteLine("");
            Console.WriteLine($"{enemy.GetName()} Choose: {choiceRng.ToString()}");
            Console.ReadLine();
            DamageCalc(choiceRng,"enemy");
        }

        private static void YouWin()
        {
            Console.Clear();
            string youWin = @"
__  ______  __  __   _       _______   __
\ \/ / __ \/ / / /  | |     / /  _/ | / /
 \  / / / / / / /   | | /| / // //  |/ / 
 / / /_/ / /_/ /    | |/ |/ // // /|  /  
/_/\____/\____/     |__/|__/___/_/ |_/ 
        Thanks for playing!";
            Console.WriteLine(youWin);
            Console.ReadLine();
        }

        private static void YouLose()
        {
            Console.Clear();
            string youLose = @"
__  ______  __  __   __    ____  _____ ______
\ \/ / __ \/ / / /  / /   / __ \/ ___// ____/
 \  / / / / / / /  / /   / / / /\__ \/ __/   
 / / /_/ / /_/ /  / /___/ /_/ /___/ / /___   
/_/\____/\____/  /_____/\____//____/_____/
        Thanks for playing!";
            Console.WriteLine(youLose);
            Console.ReadLine();
        }
    }
}