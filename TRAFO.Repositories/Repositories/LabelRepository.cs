using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.Repositories;

public sealed class LabelRepository : EntityFrameworkDatabase, ILabelRepository
{
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
}
