namespace Probe
{
    public interface IMethodDeclarationIdentifier
    {
        MethodDeclaration Find(int index, Code code);
    }
}