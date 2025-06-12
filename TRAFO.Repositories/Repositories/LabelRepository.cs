using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.Repositories;

public sealed class LabelRepository : EntityFrameworkDatabase, ILabelRepository
{
    // todo #71
    public IEnumerable<Label> GetAllLabels() => _context.Label.Select(FromDatabaseEntry);

    public void SetLabel(Transaction transaction, Label label)
    {
        // todo #86
        throw new NotImplementedException();
    }

    public void UpdateLabels(Transaction transaction)
    {
        // todo #86
        throw new NotImplementedException();
    }
}
