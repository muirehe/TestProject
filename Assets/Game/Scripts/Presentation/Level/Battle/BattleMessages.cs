namespace Presentation.Level.Battle
{
    public readonly struct BattleEnded
    {
        public readonly bool IsVictory;

        public BattleEnded(bool isVictory)
        {
            IsVictory = isVictory;
        }
    }

    public readonly struct ExitEntered
    {
    }
}