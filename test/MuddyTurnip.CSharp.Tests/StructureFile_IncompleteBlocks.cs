using MuddyTurnip.CSharp.Tests.Fixtures;
using MuddyTurnip.RulesEngine;
using Xunit;

namespace MuddyTurnip.RegularExpression.Tests
{
    public class StructureFile_IncompleteBlocks : IClassFixture<IncompleteBlockFixture>
    {
        IncompleteBlockFixture _fixture;

        public StructureFile_IncompleteBlocks(IncompleteBlockFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ErrorCount()
        {
            Assert.NotNull(_fixture.BlockStatsCache);

            int count = 0;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (BlockStats block in _fixture.BlockStatsCache.BlockStats)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                if (block.Errors is { })
                {
                    count++;
                }
            }

            Assert.Equal(10, count);
        }
    }
}

