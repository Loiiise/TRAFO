using System.Linq.Expressions;
using TRAFO.Logic.Dto;
using TRAFO.Repositories.Entities;
using TRAFO.Repositories.Repositories;

namespace TRAFO.Repositories;
internal static class QueryableExtensions
{
    internal static IQueryable<Account> ToDto(this IQueryable<AccountBalanceDatabaseEntry> databaseEntries) => databaseEntries.Select(dbEntity => AccountRepository.FromDatabaseEntry(dbEntity));
    internal static IQueryable<Balance> ToDto(this IQueryable<BalanceDatabaseEntry> databaseEntries) => databaseEntries.Select(dbEntity => BalanceRepository.FromDatabaseEntry(dbEntity));
    internal static IQueryable<Label> ToDto(this IQueryable<LabelDatabaseEntry> databaseEntries) => databaseEntries.Select(dbEntity => LabelRepository.FromDatabaseEntry(dbEntity));
    internal static IQueryable<LabelCategory> ToDto(this IQueryable<LabelCategoryDatabaseEntry> databaseEntries) => databaseEntries.Select(dbEntity => LabelCategoryRepository.FromDatabaseEntry(dbEntity));
    internal static IQueryable<Transaction> ToDto(this IQueryable<TransactionDatabaseEntry> databaseEntries) => databaseEntries.Select(dbEntity => TransactionRepository.FromDatabaseEntry(dbEntity));
    
    internal static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        => condition ? source.Where(predicate) : source;
}
