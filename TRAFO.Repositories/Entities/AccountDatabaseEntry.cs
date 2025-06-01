using Microsoft.EntityFrameworkCore;
using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.Entities;

[PrimaryKey(nameof(AccountId))]
internal sealed class AccountDatabaseEntry
{
    public required string AccountId { get; set; }
    public string? AccountName { get; set; }
    public required Currency Currency { get; set; }
}
