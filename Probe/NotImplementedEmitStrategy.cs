namespace Probe
{
    public class NotImplementedEmitStrategy : IEmitStrategy
    {
        public string Emit { get; } = "throw new NotImplementedException();";

        public string[] Imports { get; } = { "using System;" };
    }
}