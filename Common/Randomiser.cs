using System;

namespace PokeApiTechDemo.Common
{
    public static class Randomiser
    {
        public static int GetNumberBetweenOneAndTen()
        {
            var random = new Random();
            return random.Next(1,10);
        }
    }
}