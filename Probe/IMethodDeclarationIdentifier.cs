namespace Probe
{
    public interface IMethodDeclarationIdentifier
    {
        MethodDeclaration TryFind(int index, RawCode code);
    }
}