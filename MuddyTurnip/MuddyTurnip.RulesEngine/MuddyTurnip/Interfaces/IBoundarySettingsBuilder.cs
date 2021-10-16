namespace MuddyTurnip.RulesEngine
{
    public interface IBoundarySettingsBuilder
    {
        IBoundarySettings Build(GroupSettings settings);
    }
}