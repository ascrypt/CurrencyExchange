namespace CurrencyExchange.Utils
{
    public static class UrlStringExtensions
    {
        public static string CombineUrl(this string baseUrl, params string[] segments)
        {
            return string.Join("/", new[] { baseUrl.TrimEnd('/') }
                .Concat(segments.Select(s => s.Trim('/'))));
        }
    }
}