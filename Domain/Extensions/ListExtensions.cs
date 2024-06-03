using System.Text.RegularExpressions;

namespace api.Domain.Extensions;

public static class ListExtensions
{
    public static List<T> WhereIf<T>(
        this List<T> mainList,
        bool condition,
        Func<T, bool> predicate)
    {
        return condition ? mainList.Where(predicate).Distinct().ToList() : mainList;
    }

    public static List<T> WhereIf<T>(
        this List<T> mainList,
        string stringedArray,
        Func<T, string> property)
    {
        var filters = stringedArray.Split(',').Select(s => s.Trim().ToLower());

        return mainList.Where(item => filters.Any(filter => Like($"{property(item)}".Trim().ToLower(), filter))).Distinct().ToList();
    }

    public static List<T> WhereIf<T>(
        this List<T> mainList,
        bool condition,
        string stringedArray,
        Func<T, string> property)
    {
        if (condition)
        {
            var filters = stringedArray.Split(',').Select(s => s.Trim().ToLower());

            return mainList.Where(item => filters.Any(filter => Like($"{property(item)}".Trim().ToLower(), filter))).Distinct().ToList();
        }
        return mainList;
    }

    private static bool Like(string input, string pattern)
        => new Regex(pattern).IsMatch(input);
}
