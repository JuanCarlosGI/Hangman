using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    internal class TrieNode
    {
        public char Character;
        public bool IsEnd;
        public int Words;
        public int[] WordsWithChar = new int[26];

        public List<TrieNode> Children = new List<TrieNode>();

        public TrieNode(char character)
        {
            Character = character;
            if (character != '\0') WordsWithChar[character - 'a'] = 1;
        }

        public TrieNode(char character, List<TrieNode> children) : this(character)
        {
            Children = children;
            Words = children.Sum(child => child.Words);
            WordsWithChar = children.Select(child => child.WordsWithChar).Aggregate(AggregateArrays);
            if (character != '\0') WordsWithChar[Character - 'a'] = Words;
        }

        public TrieNode Filter(char?[] chars, int start)
        {
            if (start == chars.Length) return null;

            if (chars[start] != null && chars[start] != Character)
                return null;
            
            if (start == chars.Length - 1)
            {
                if (!IsEnd) return null;
                return new TrieNode(Character)
                {
                    IsEnd = true,
                    Words = 1
                };
            }
            
            var filteredChildren = Children
                .Select(child => child.Filter(chars, start + 1))
                .Where(child => child != null)
                .ToList();

            if (filteredChildren.Count == 0) return null;

            return new TrieNode(Character, filteredChildren);
        }

        public TrieNode Filter(int wordLength)
        {
            if (wordLength <= 0) return null;
            if (wordLength == 1)
            {
                if (IsEnd)
                {
                    return new TrieNode(Character)
                    {
                        IsEnd = true,
                        Words = 1
                    };
                }
                    
                return null;
            }

            var filteredChildren = Children
                .Select(child => child.Filter(wordLength - 1))
                .Where(child => child != null)
                .ToList();

            if (filteredChildren.Count == 0) return null;

            return new TrieNode(Character, filteredChildren);
        }

        private int[] AggregateArrays(int[] arr1, int[] arr2)
        {
            var result = new int[26];
            for (var i = 0; i < 26; i++)
            {
                result[i] = arr1[i] + arr2[i];
            }
            return result;
        }
    }
}
