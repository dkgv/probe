using System.Collections.Generic;

namespace Probe
{
    public interface IDependencyExtractor
    {
        IEnumerable<string> ExtractDependencies(RawCode code);
    }
}