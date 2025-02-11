using Shouldly;
using TRAFO.IO.Command;

namespace TRAFO.IO.Tests.Command;
public class CommandMetaDataTests
{
    [Fact]
    public void GetNameFromTagShouldBeSymmetricalToGetTagFromName()
    {
        foreach ((var name, var tag, var _) in CommandMetaData.AllCommandNamesTagsAndDescriptions())
        {
            CommandMetaData.GetNameFromTag(tag).ShouldBe(name);
            CommandMetaData.GetTagFromName(name).ShouldBe(tag);

            CommandMetaData.GetTagFromName(CommandMetaData.GetNameFromTag(tag)).ShouldBe(tag);
            CommandMetaData.GetNameFromTag(CommandMetaData.GetTagFromName(name)).ShouldBe(name);
        }
    }

    [Fact]
    public void GetDescriptionFromTagOrNameShouldBeIdenticalToEachotherAndActualDescription()
    {
        foreach ((var name, var tag, var description) in CommandMetaData.AllCommandNamesTagsAndDescriptions())
        {
            CommandMetaData.GetDescriptionFromTag(tag).ShouldBe(description);
            CommandMetaData.GetDescriptionFromName(name).ShouldBe(description);
        }
    }
}
