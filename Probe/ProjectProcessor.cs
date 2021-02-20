using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Probe
{
    public class ProjectProcessor
    {
        public ProjectProcessor(SourceReplacer replacer)
        {
            Replacer = replacer;
        }

        public SourceReplacer Replacer { get; }

        public void Process(string projectRootPath)
        {
            foreach (var entry in Directory.GetFiles(projectRootPath, "*.cs", SearchOption.AllDirectories))
            {
                Console.WriteLine($"Processing {entry}");

                var code = new Code
                {
                    FilePath = entry,
                    Lines = File.ReadAllLines(entry).ToList()
                };

                // Replace content
                Replacer.Replace(code);

                // Overwrite existing content
                File.WriteAllLines(code.FilePath, code.Lines);
            }
        }
    }
}