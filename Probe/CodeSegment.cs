using System;

namespace Probe
{
    public class CodeSegment
    {
        public string Content { get; set; }
        
        public int LineStart { get; set; }
        
        public int LineEnd { get; set; }

        public int NumLines => Math.Max(1, LineEnd - LineStart);
    }
}