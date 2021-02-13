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

        public string FilePath { get; set; }

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

        public void Replace(int lineFrom, int lineTo, string[] replacements)
        {
            for (int i = lineFrom, j = 0; i < lineTo; i++, j++)
            {
                Lines[i] = replacements[j];
            }
        }

        public string GetContent() =>  string.Join("\n", Lines);
    }
}