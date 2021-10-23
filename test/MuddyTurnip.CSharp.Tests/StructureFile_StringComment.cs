using MuddyTurnip.CSharp.Tests.Fixtures;
using MuddyTurnip.Metrics.Engine;
using MuddyTurnip.RulesEngine;
using System.IO;
using Xunit;

namespace MuddyTurnip.RegularExpression.Tests
{
    public class StructureFile_StringComment : IClassFixture<CommentFixture>
    {
        CommentFixture _fixture;

        public StructureFile_StringComment(CommentFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CodeContentMatches()
        {
            Assert.NotNull(_fixture.DirectoryPath);

#pragma warning disable CS8604 // Possible null reference argument.
            string path = Path.Combine(_fixture.DirectoryPath, "Files\\CSharp3Code.cs");
#pragma warning restore CS8604 // Possible null reference argument.

            string stringContent = File.ReadAllText(path);

            Assert.Equal(stringContent, _fixture.CodeContainer.CodeContent.NormaliseLineEndings());
        }

        [Fact]
        public void StringContentMatches()
        {
            Assert.NotNull(_fixture.DirectoryPath);

#pragma warning disable CS8604 // Possible null reference argument.
            string path = Path.Combine(_fixture.DirectoryPath, "Files\\CSharp3Strings.cs");
#pragma warning restore CS8604 // Possible null reference argument.

            string stringContent = File.ReadAllText(path);

            Assert.Equal(stringContent, _fixture.CodeContainer.StringContent.NormaliseLineEndings());
        }

        [Fact]
        public void CommentContentMatches()
        {
            Assert.NotNull(_fixture.DirectoryPath);

#pragma warning disable CS8604 // Possible null reference argument.
            string path = Path.Combine(_fixture.DirectoryPath, "Files\\CSharpComments.cs");
#pragma warning restore CS8604 // Possible null reference argument.

            string stringContent = File.ReadAllText(path);

            Assert.Equal(stringContent, _fixture.CodeContainer.CommentContent.NormaliseLineEndings());
        }

        [Fact]
        public void PreProcessorContentMatches()
        {
            Assert.NotNull(_fixture.DirectoryPath);

#pragma warning disable CS8604 // Possible null reference argument.
            string path = Path.Combine(_fixture.DirectoryPath, "Files\\CSharpPreProcessor.cs");
#pragma warning restore CS8604 // Possible null reference argument.

            string stringContent = File.ReadAllText(path);

            Assert.Equal(stringContent, _fixture.CodeContainer.PreProcessorContent.NormaliseLineEndings());
        }
    }
}

