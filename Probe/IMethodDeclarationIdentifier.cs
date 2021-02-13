namespace Probe
{
    public interface IMethodDeclarationIdentifier
    {
        MethodVariant Find(string line, string nextLine);
    }
}