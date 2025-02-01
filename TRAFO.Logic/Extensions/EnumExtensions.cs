namespace TRAFO.Logic.Extensions;

public static class EnumExtensions
{
    public static IEnumerable<T> GetAllValues<T>()
        where T : Enum
        => Enum.GetValues(typeof(T)).Cast<T>();

    /// <summary>
    /// Some of the values in <see cref="Currency"/> are no longer in use. This function returns whether or not the currency was in use on a given moment in time.
    /// </summary>
    public static bool WasInUseAt(this Currency currency, DateTime date) => currency switch
    {
        Currency.BGL => date < new DateTime(1999, 07, 05),
        Currency.BYR => date < new DateTime(2016, 07, 01),
        Currency.CUC => date < new DateTime(2021, 01, 01),
        Currency.FIM => date < new DateTime(2002, 02, 28),
        Currency.FRF => date < new DateTime(2002, 02, 17),
        Currency.HRK => date < new DateTime(2023, 01, 01),
        Currency.IEP => date < new DateTime(2002, 02, 09),
        Currency.ITL => date < new DateTime(2002, 02, 28),
        Currency.ROL => date < new DateTime(2005, 07, 01),
        Currency.VEB => date < new DateTime(2008, 01, 01),
        Currency.YUM => date < new DateTime(2003, 07, 31),
        Currency.ZWL => date < new DateTime(2009, 04, 12),
        _ => true,
    };

    /// <summary>
    /// Returns a list of commonly used currencies according to a trivial source.
    /// </summary>
    public static IEnumerable<Currency> CommonCurrencies() => new[]
    {
        Currency.USD,
        Currency.EUR,
        Currency.JPY,
        Currency.GBP,
        Currency.AUD,
        Currency.CAD,
        Currency.CHF,
        Currency.CNY,
        Currency.HKD,
        Currency.NZD,
        Currency.SGD,
    };
}