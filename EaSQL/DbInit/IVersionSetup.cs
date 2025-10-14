namespace EaSQL.DbInit
{
    /// <summary>
    /// Allows defining a new version for the database.
    /// </summary>
    public interface IVersionSetup
    {
        /// <summary>
        /// Adds a step for the definition of this version.
        /// </summary>
        /// <param name="command">Database command to add.</param>
        /// <returns>This instance of the version setup.</returns>
        IVersionSetup AddStep(string command);
    }
}
