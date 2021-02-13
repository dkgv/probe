using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
            var queue = new Queue<string>(Directory.GetFileSystemEntries(projectRootPath));

            while (queue.Count > 0)
            {
                var entry = queue.Dequeue();

                Console.WriteLine($"Processing {entry}");

                if (Directory.Exists(entry))
                {
                    // Enqueue all entries in this directory (nesting)
                    foreach (var childEntry in Directory.GetFiles(entry))
                    {
                        queue.Enqueue(childEntry);
                    }
                }
                else
                {
                    var code = new Code
                    {
                        FilePath = entry,
                        Lines = File.ReadAllLines(entry)
                    };

                    // Replace content
                    Replacer.Replace(code);

                    // Overwrite existing content
                    File.WriteAllLines(code.FilePath, code.Lines);
                }
            }
        }
    }
}