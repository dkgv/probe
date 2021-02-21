namespace Probe
{
    public interface IEmitStrategy
    {
        string Emit { get; }

        string[] Imports { get; }
    }
}