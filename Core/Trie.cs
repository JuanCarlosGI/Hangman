using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class Trie
    {
        private TrieNode _head = new TrieNode('\0');

        private Trie(TrieNode head)
        {
            _head = head;
        }

        public Trie() { }

        public async Task AddAsync(string word)
        {
            await Task.Yield();

            var node = _head;
            foreach (var letter in word)
            {
                lock (node)
                {
                    node.Words++;
                    foreach (var ch in word)
                    {
                        node.WordsWithChar[ch - 'a']++;
                    }

                    var next = node.Children.FirstOrDefault(child => child.Character == letter);
                    if (next == null)
                    {
                        next = new TrieNode(letter);
                        node.Children.Add(next);
                    }

                    node = next;
                }
            }

            node.Words++;
            node.IsEnd = true;
        }

        public async Task<Trie> FilterAsync(char?[] filterString)
        {
            await Task.Yield();
            var filteredChildren = _head.Children
                .Select(child => child.Filter(filterString, 0))
                .Where(child => child != null)
                .ToList();
            return new Trie(new TrieNode('\0', filteredChildren));
        }

        public async Task<Trie> FilterAsync(int wordLength)
        {
            await Task.Yield();
            var filteredChildren = _head.Children
                .Select(child => child.Filter(wordLength))
                .Where(child => child != null)
                .ToList();
            return new Trie(new TrieNode('\0', filteredChildren));
        }

        public int WordsWithLetter(char character)
        {
            return _head.WordsWithChar[character - 'a'];
        }
    }
}
