using System.Collections.Generic;
using System.Linq;

namespace Probe
{
    public class CSharpImportExtractor : IImportExtractor
    {
        public List<ImportStatement> Extract(RawCode code)
        {
            return code.Lines
                .Select(line => line.Trim())
                .SkipWhile(line => !line.StartsWith("using"))
                .TakeWhile((line, index) => line.StartsWith("using") && line.EndsWith(";"))
                .Select(line => new ImportStatement
                {
                    Import = line,
                    LineIndex = code.Lines.IndexOf(line)
                })
                .ToList();
        }
    }
}