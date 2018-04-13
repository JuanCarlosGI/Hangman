using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core
{
    internal static class TrieGenerator
    {
        public static async Task<Trie> GenerateFromFileAsync(string filePath)
        {
            var trie2 = new Trie();
            await trie2.AddAsync("cat");
            await trie2.AddAsync("cats");
            var file = await ReadFileAsync(filePath).ConfigureAwait(false);
            var words = JsonConvert.DeserializeObject<Dictionary<string, object>>(file).Keys;
            return await GenerateFromWords(words);
        }

        private static async Task<string> ReadFileAsync(string filePath)
        {
            using (var reader = File.OpenText(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static async Task<Trie> GenerateFromWords(IEnumerable<string> words)
        {
            var trie = new Trie();
            await Task.WhenAll(
                words
                    .Where(w => w.All(ch => char.IsLetter(ch)))
                    .Select(w => trie.AddAsync(w.ToLower())));
            return trie;
        }
    }
}
