using TRAFO.Logic.Dto;
using TRAFO.Repositories.Entities;

namespace TRAFO.Repositories.Repositories;

public sealed class LabelRepository : ILabelRepository
{
    private readonly EntityFrameworkDatabaseContext _context;

    public LabelRepository(EntityFrameworkDatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void CreateIfNotExists(IEnumerable<string> labelNames)
    {
        throw new NotImplementedException();
    }

    public void CreateIfNotExists(IEnumerable<Label> label)
    {
        throw new NotImplementedException();
    }

    // todo #71
    public IEnumerable<Label> GetAllLabels() => _context.Label.Select(FromDatabaseEntry);

    public void SetLabel(Transaction transaction, Label label)
    {
        // todo #86
        throw new NotImplementedException();
    }

    public Label? TryGetLabelById(Guid labelId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Label> TryGetLabelById(IEnumerable<Guid> labelIds)
    {
        throw new NotImplementedException();
    }

    public void UpdateLabels(Transaction transaction)
    {
        // todo #86
        throw new NotImplementedException();
    }

    // todo #86
    internal static Label FromDatabaseEntry(LabelDatabaseEntry label) => throw new NotImplementedException();
}
