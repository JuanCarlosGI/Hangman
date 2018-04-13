using System.Threading.Tasks;
using Core;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var tasks = new List<Task<GameMaster.GameResult>>();
            var attempts = 50.0m;
            for (var i = 0; i < attempts; i++)
            {
                tasks.Add(GameMaster.PlayGame());
            }
            await Task.WhenAll(tasks);
            var totalMisses = tasks
                .Select(task => task.Result)
                .Sum(result => result.Misses);
            Console.WriteLine($"Average misses: {totalMisses / attempts}");

            /*
            var words = new string[] { "cosmonautics", "sphacelus", "outwalking", "gibleh", "emraud" };

            var tasks = words.Select(word => GameMaster.PlayGame(word).ContinueWith(res => Console.WriteLine($"{word}: {res.Result.Turns}, {res.Result.Misses} misses")));
            await Task.WhenAll(tasks);
            */
        }

        private class Handler
        {
            public void HandleTurn(char guess, char?[] status, int turns, int misses, bool hasWon)
            {
                var word = new String(status.Select(ch => ch ?? '_').ToArray());
                Console.WriteLine($"Turn {turns}: guessed '{guess}'");
                Console.WriteLine($"Status: {word}");
                Console.WriteLine($"Misses: {misses}");
                if (hasWon)
                {
                    Console.WriteLine("Player has won");
                }
                Console.WriteLine();
            }
        }
        
    }
}
