using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Can only play one game.
    /// </summary>
    public class TriePlayer : IPlayer
    {
        private Trie _trie;
        private char?[] _lastGuess;
        private readonly IEnumerable<string> _validWords;

        public TriePlayer(int wordLenght, IEnumerable<string> validWords, Trie trie)
            : this(wordLenght, validWords)
        {
            _trie = trie;
        }

        public TriePlayer(int wordLength, IEnumerable<string> validWords)
        {
            _validWords = validWords.Where(word => word.Length == wordLength);
            _lastGuess = new char?[wordLength];
        }

        public async Task<char> GuessAsync(char?[] word, IEnumerable<char> usedChars)
        {
            if (_trie == null)
                _trie = await TrieGenerator.GenerateFromWords(_validWords);

            if (!Enumerable.SequenceEqual(word, _lastGuess))
                _trie = await _trie.FilterAsync(word);
            return GameMaster.validCharacters
                .Where(ch => !usedChars.Contains(ch))
                .OrderByDescending(ch => _trie.WordsWithLetter(ch))
                .First();
        }
    }
}
