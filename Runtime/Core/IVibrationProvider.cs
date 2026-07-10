namespace HyperCasualRunner
{
    public interface IVibrationProvider
    {
        void VibratePop();
        void VibratePeek();
    }

    public sealed class NullVibrationProvider : IVibrationProvider
    {
        public void VibratePop() { }
        public void VibratePeek() { }
    }
}
