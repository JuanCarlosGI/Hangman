using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
    public interface IPlayer
    {
        Task<char> GuessAsync(char?[] word, IEnumerable<char> usedChars);
    }
}
