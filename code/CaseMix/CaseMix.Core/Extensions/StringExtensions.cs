namespace CaseMix.Extensions
{
    public static class StringExtensions
    {
        public static bool CaseInsensitiveContains(this string word, string compare)
        {
            return word.ToLower().Contains(compare.ToLower());
        }
    }
}
