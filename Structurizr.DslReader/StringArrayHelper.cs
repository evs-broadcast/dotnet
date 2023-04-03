namespace Structurizr.DslReader
{
    public static class StringArrayHelper
    {
        public static string? GetValueAtOrDefault(this string[] array, int ndx)
        {
            return array.Length > ndx ? array[ndx] : null;
        }
    }
}