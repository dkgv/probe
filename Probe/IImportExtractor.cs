using System.Collections.Generic;

namespace Probe
{
    public interface IImportExtractor
    {
        public List<ImportStatement> Extract(RawCode code);
    }
}