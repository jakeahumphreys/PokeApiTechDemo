using System.Linq;
using System.Text.RegularExpressions;

namespace PokeApiToolWPF.Common
{
    public static class ValidationHelper
    {
        public static bool IsSearchTextValid(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return false;

            if (searchText.Length < 3) //No pokemon with less than 3 character names, I think
                return false;

            if (searchText.Any(c => char.IsDigit(c)))
                return false;

            var regex = new Regex(@"[a-zA-Z]");
            if (!regex.IsMatch(searchText))
                return false;

            return true;
        }
    }
}