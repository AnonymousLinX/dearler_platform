using System.Web;

namespace DearlerPlatform.Api.Tools;

public static class UrlEncoded
{
    public static Dictionary<string, List<string>> UrlEncodedStringToListDictionary(this string queryString)
    {
        if (queryString == null) return new Dictionary<string, List<string>>();
        var urlDict = new Dictionary<string, List<string>>(); ;
        var parsed = HttpUtility.ParseQueryString(queryString);
        foreach (string key in parsed.AllKeys)
        {
            var values = parsed.GetValues(key);
            if (values != null)
            {

                urlDict[key] = values.ToList();
            }

        }
        return urlDict;
    }
}
