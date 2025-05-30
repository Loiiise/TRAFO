using TRAFO.LocalApp.CLI.Command.MetaData;

namespace TRAFO.LocalApp.CLI.Tests.Command;
internal record MockConfiguration(string Name, string Tag, string Description) : CommandOrFlagConfiguration(Name, Tag, Description);

internal class MockMetaData : MetaData<MockConfiguration>, ICommandMetaData, IFlagMetaData
{
    protected override MockConfiguration[] _commandConfigurations => new[]
    {
        new MockConfiguration("name", "tag", "description"),
        new MockConfiguration("this is a name", "tagtagtag", "describe me"),
        new MockConfiguration("john doe", "pewpew", "blablabla"),
    };
}
