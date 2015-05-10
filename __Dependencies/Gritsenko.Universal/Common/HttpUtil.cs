using System.Collections.Generic;
using System.Linq;

namespace Gritsenko.Universal.Common
{
    public static class HttpUtil
    {
        public static Dictionary<string, string> ParseQueryString(string uri)
        {
            var substring = uri.Substring(((uri.LastIndexOf('?') == -1) ? 0 : uri.LastIndexOf('?') + 1));
            var pairs = substring.Split('&');
            return pairs.Select(piece => piece.Split('=')).ToDictionary(pair => pair[0], pair => pair[1]);
        }
    }
}
