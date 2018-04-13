using System.Linq;

namespace Core
{
    class Game
    {
        private string _word;

        public char?[] CurrentStatus;
        public int Guesses { get; private set; }
        public int Misses { get; private set; }

        public Game(string word)
        {
            _word = word;
            CurrentStatus = new char?[word.Length];
        }

        public char?[] ApplyGuess(char character)
        {
            Guesses++;

            var missed = true;
            for (var i = 0; i < _word.Length; i++)
            {
                if (_word[i] == character)
                {
                    if (CurrentStatus[i] == null)
                    {
                        missed = false;
                        CurrentStatus[i] = character;
                    }
                }
            }

            Misses += missed ? 1 : 0;

            return (char?[])CurrentStatus.Clone();
        }

        public bool IsWon => CurrentStatus.All(ch => ch != null);
    }
}
