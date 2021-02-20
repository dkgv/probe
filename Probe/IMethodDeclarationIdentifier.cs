namespace Probe
{
    public interface IMethodDeclarationIdentifier
    {
        MethodDeclaration TryFind(int index, Code code);
    }
}