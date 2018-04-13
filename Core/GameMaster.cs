using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace Core
{
    public class GameMaster
    {
        public static readonly IReadOnlyList<char> validCharacters = new List<char>(){ 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private static Random Random = new Random((int)DateTime.Now.Ticks);

        public delegate void TurnHandler(char guess, char?[] status, int turns, int misses, bool hasWon);

        public struct GameResult
        {
            public string Word { get; set; }
            public int Turns { get; set; }
            public int Misses { get; set; }
            public Collection<char> Guesses { get; set; }
        }

        private static readonly string[] Words;
        static GameMaster()
        {
            Words = GetWords(@"C:\Users\juguzman\source\repos\Hangman\Core\words_dictionary.json").Result.ToArray();
        }

        public static async Task<GameResult> PlayGame(TurnHandler handler = null)
        {
            var word = Words[(int)(Random.NextDouble() * Words.Length)];
            return await PlayGame(word, Words, handler);
        }

        public static async Task<GameResult> PlayGame(string word, TurnHandler handler = null)
        {
            return await PlayGame(word, Words, handler);
        }

        public static async Task<GameResult> PlayGame(string word, IEnumerable<string> possibleWords, TurnHandler handler = null)
        {
            return await PlayGame(word, possibleWords, new TriePlayer(word.Length, possibleWords), handler);
        }

        public static async Task<GameResult> PlayGame(string word, IEnumerable<string> possibleWords, IPlayer player, TurnHandler handler = null)
        {
            var game = new Game(word);
            var usedChars = new Collection<char>();
            while (!game.IsWon)
            {
                var guess = await player.GuessAsync(game.CurrentStatus, usedChars).ConfigureAwait(false);
                usedChars.Add(guess);
                game.ApplyGuess(guess);
                handler?.Invoke(guess, game.CurrentStatus, game.Guesses, game.Misses, game.IsWon);
            }
            return new GameResult() {
                Turns = game.Guesses,
                Guesses = usedChars,
                Misses = game.Misses,
                Word = word
            };
        }

        private static async Task<IEnumerable<string>> GetWords(string filePath)
        {
            var file = await ReadFileAsync(filePath).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(file).Keys;
        }

        private static async Task<string> ReadFileAsync(string filePath)
        {
            using (var reader = File.OpenText(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
