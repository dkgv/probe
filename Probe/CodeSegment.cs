using System;

namespace Probe
{
    public class CodeSegment
    {
        public string Content { get; set; }
        
        public int LineStartIndex { get; set; }
        
        public int LineEndIndex { get; set; }

        public int NumLines => Math.Max(1, LineEndIndex - LineStartIndex);
    }
}