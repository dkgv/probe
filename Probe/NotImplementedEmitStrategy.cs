namespace Probe
{
    public class NotImplementedEmitStrategy : IEmitStrategy
    {
        public string Emit { get; } = "throw new NotImplementedException();";
    }
}