namespace Probe
{
    public class RemovedCommentEmitStrategy : IEmitStrategy
    {
        public string Emit { get; } = "// removed";
    }
}