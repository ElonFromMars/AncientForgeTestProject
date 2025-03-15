namespace Gameplay.Models.Common.Utils
{
    public static class MathUtils
    {
        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }
        
        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

        public static float Min(float a, float b)
        {
            return a < b ? a : b;
        }
    }
}