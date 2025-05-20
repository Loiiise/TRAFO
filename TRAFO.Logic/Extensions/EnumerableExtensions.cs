using System.Diagnostics.CodeAnalysis;

namespace TRAFO.Logic.Extensions;
public static class EnumerableExtensions
{
    public static T? GetFirstOrDefault<T>(this IEnumerable<object> items)
    {
        items.TryGetFirstOrDefault<T>(out var result);
        return result;
    }

    public static bool TryGetFirstOrDefault<T>(this IEnumerable<object> items, [MaybeNullWhen(false), NotNullWhen(true)] out T result)
    {
        var item = items.FirstOrDefault(i => i is T);
        if (item is T correctTypeItem)
        {
            result = correctTypeItem;
            return true;
        }

        result = default;
        return false;
    }
}
