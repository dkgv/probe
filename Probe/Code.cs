using System.Text;

namespace Probe
{
    public class Code
    {
        public Code()
        {
        }

        public Code(CodeSegment[] segments)
        {
            var sb = new StringBuilder();
            foreach (var codeSegment in segments)
            {
                sb.Append(codeSegment.Content);
            }

            Lines = sb.ToString().Split("\n");
        }

        public string[] Lines { get; set; }

        public string Join(CodeSegment segment)
        {
            var sb = new StringBuilder();
            
            for (var i = segment.LineStart; i < segment.LineEnd; i++)
            {
                sb.Append(Lines[i]).Append("\n");
            }

            return sb.ToString();
        }

        public void Replace(int lineFrom, int lineTo, string replace)
        {
            for (var i = lineFrom; i < lineTo; i++)
            {
                Lines[i] = replace;
            }
        }

        public string GetContent() =>  string.Join("\n", Lines);
    }
}