namespace TRAFO.Logic.Extensions;

public static class EnumExtensions
{
    public static IEnumerable<T> GetAllValues<T>()
        where T : Enum
        => Enum.GetValues(typeof(T)).Cast<T>();
}
