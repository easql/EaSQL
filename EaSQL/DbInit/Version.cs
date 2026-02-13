namespace EaSQL.DbInit;

internal sealed class Version(int versionNumber)
{
    internal int VersionNumber { get; } = versionNumber;

    internal List<IOperation> Operations { get; } = [];
}