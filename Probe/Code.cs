using System.Linq;
using System.Text;

namespace Probe
{
    public class Code
    {
        private string[] _lines;

        public Code()
        {
        }

        public string FilePath { get; set; }

        public string[] Lines
        {
            get => _lines;
            set => _lines = value.Select(x => x.Replace("\r", "")).ToArray();
        }

        public string Join(CodeSegment segment) => Join(segment.LineStartIndex, segment.LineEndIndex);

        public string Join(int lineStart, int lineEnd)
        {
            if (lineStart == lineEnd)
            {
                return Lines[lineStart];
            }

            var sb = new StringBuilder();
            
            for (var i = lineStart; i < lineEnd; i++)
            {
                sb.Append(Lines[i]).Append("\n");
            }

            return sb.ToString();
        }

        public void Replace(int lineFrom, int lineTo, string[] replacements)
        {
            if (lineFrom == lineTo)
            {
                Lines[lineFrom] = replacements[0];
            }

            for (int i = lineFrom, j = 0; i < lineTo; i++, j++)
            {
                Lines[i] = replacements[j];
            }
        }

        public string GetContent() =>  string.Join("\n", Lines);
    }
}