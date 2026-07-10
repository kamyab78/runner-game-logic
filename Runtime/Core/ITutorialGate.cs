namespace HyperCasualRunner
{
    public interface ITutorialGate
    {
        bool IsBlockingInput { get; }
        bool CanUseVerticalFormation { get; }
        bool CanUseHorizontalFormation { get; }
        void NotifyFormationUsed(bool vertical);
    }

    public sealed class NullTutorialGate : ITutorialGate
    {
        public bool IsBlockingInput => false;
        public bool CanUseVerticalFormation => true;
        public bool CanUseHorizontalFormation => true;
        public void NotifyFormationUsed(bool vertical) { }
    }
}
