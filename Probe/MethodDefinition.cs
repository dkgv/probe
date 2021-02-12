namespace Probe
{
    public class MethodDefinition
    {
        public string Signature { get; set; }
        
        public CodeSegment FullMethod { get; set; }
        
        public CodeSegment MethodBody { get; set; }
    }
}