using System.Collections.Generic;

namespace Probe
{
    public interface IMethodExtractor
    {
        public IEnumerable<MethodDefinition> ExtractMethods(RawCode code);
    }
}