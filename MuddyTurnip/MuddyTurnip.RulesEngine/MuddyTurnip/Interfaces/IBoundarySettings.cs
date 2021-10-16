namespace MuddyTurnip.RulesEngine
{
    public interface IBoundarySettings
    {
        string Name { get; }
        string Model { get; }
        string BlockType { get; }
        string Open { get; }
        string Close { get; }
        string SearchType { get; }
        bool ExplicitClose { get; }
    }
}