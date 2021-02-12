using System.Collections.Generic;
using System.Linq;

namespace Probe
{
    public class CSharpDependencyExtractor : IDependencyExtractor
    {
        public IEnumerable<string> ExtractDependencies(Code code)
        {
            return code.Lines.TakeWhile(line => line.StartsWith("using"));
        }
    }
}