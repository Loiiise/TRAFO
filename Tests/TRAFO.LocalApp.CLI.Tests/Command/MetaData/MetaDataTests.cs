using Shouldly;

namespace TRAFO.LocalApp.CLI.Tests.Command;
public class MetaDataTests
{
    [Fact]
    public void GetNameFromTagShouldBeSymmetricalToGetTagFromName()
    {
        var commandMetaData = new MockMetaData();
        foreach ((var name, var tag, var _) in commandMetaData.AllNamesTagsAndDescriptions())
        {
            commandMetaData.GetNameFromTag(tag).ShouldBe(name);
            commandMetaData.GetTagFromName(name).ShouldBe(tag);

            commandMetaData.GetTagFromName(commandMetaData.GetNameFromTag(tag)).ShouldBe(tag);
            commandMetaData.GetNameFromTag(commandMetaData.GetTagFromName(name)).ShouldBe(name);
        }
    }

    [Fact]
    public void GetDescriptionFromTagOrNameShouldBeIdenticalToEachotherAndActualDescription()
    {
        var commandMetaData = new MockMetaData();
        foreach ((var name, var tag, var description) in commandMetaData.AllNamesTagsAndDescriptions())
        {
            commandMetaData.GetDescriptionFromTag(tag).ShouldBe(description);
            commandMetaData.GetDescriptionFromName(name).ShouldBe(description);
        }
    }
}
