namespace LinkitAir.PerformanceLogger
{
    public interface ILogOptions
    {
        IOptions Configure();
        void Default();
    }
}