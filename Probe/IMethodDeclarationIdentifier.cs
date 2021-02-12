namespace Probe
{
    public interface IMethodDeclarationIdentifier
    {
        bool Matches(string line, string nextLine);
    }
}