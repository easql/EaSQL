using EaSQL.Query;
using System.Data;
using System.Data.Common;

namespace EaSQL.DbInit
{
    /// <summary>
    /// Allows a simple method to manage database versions.
    /// </summary>
    public class DbHandler : IVersionSetup
    {
        private readonly Dictionary<long, List<Step>> _setupSteps = [];
        private readonly List<Version> _versions = [];

        /// <summary>
        /// Defines a new version for the database.
        /// </summary>
        /// <returns>An instance allowing to define setup steps for this version.</returns>
        [Obsolete("Use AddVersion instead")]
        public IVersionSetup NewVersion()
        {
            _setupSteps.Add(_setupSteps.Count + 1, []);
            return this;
        }

        /// <summary>
        /// Adds a new database version.
        /// </summary>
        /// <param name="versionSetup">Operations to be performed in this version.</param>
        /// <returns>This instance to allow method chaining</returns>
        public DbHandler AddVersion(Action<VersionSetup> versionSetup)
        {
            Version version = new(_versions.Count + 1);
            _versions.Add(version);
            VersionSetup setup = new(version);
            versionSetup(setup);

            return this;
        }

        IVersionSetup IVersionSetup.AddStep(string command)
        {
            _setupSteps[_setupSteps.Count].Add(new() { Command = command });
            return this;
        }

        IVersionSetup IVersionSetup.AddStepUnless(string command, string executionPrevention)
        {
            _setupSteps[_setupSteps.Count].Add(new() { Command = command, GuardQuery = executionPrevention });
            return this;
        }

        /// <summary>
        /// Sets up the database. It will check the current version of the database and 
        /// will apply all steps to upgrade to the latest version, if necessary.
        /// </summary>
        /// <param name="connection">Database connection to use.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public async Task Setup(DbConnection connection, CancellationToken cancellationToken = default)
        {
            long currentVersion = 0;

            await using (DbCommand getVersion = connection.CreateCommand())
            {
                getVersion.CommandText = "select current_version from __version__";

                try
                {
                    object? getVersionResult = await getVersion.ExecuteScalarAsync(cancellationToken);
                    if (getVersionResult is long dbVersion)
                    {
                        currentVersion = dbVersion;
                    }
                }
                catch
                {
                    await CreateVersionTable(connection, cancellationToken);
                }
            }

            if (_versions.Count > 0)
            {
                long deployedVersion = currentVersion;
                foreach (Version version in _versions
                             .Where(v => v.VersionNumber > currentVersion)
                             .OrderBy(v => v.VersionNumber))
                {
                    foreach (IOperation operation in version.Operations)
                    {
                        await operation.Execute(connection, cancellationToken);
                    }

                    deployedVersion = version.VersionNumber;
                }
                await connection.RunCommandAsync($"update __version__ set current_version = {deployedVersion}", cancellationToken);
            }
            else
            {
                foreach ((long version, List<Step> steps) in _setupSteps)
                {
                    if (version <= currentVersion) continue;

                    foreach (Step step in steps)
                    {
                        if (!string.IsNullOrEmpty(step.GuardQuery))
                        {
                            using IDbCommand guardCommand = connection.CreateCommand();
                            guardCommand.CommandText = step.GuardQuery;
                            if (guardCommand.ExecuteReader().Read())
                            {
                                continue;
                            }
                        }

                        await using DbCommand executeStep = connection.CreateCommand();
                        executeStep.CommandText = step.Command;
                        await executeStep.ExecuteNonQueryAsync(cancellationToken);
                    }

                    await connection.RunCommandAsync($"update __version__ set current_version = {version}", cancellationToken);
                }
            }
        }

        private static async Task CreateVersionTable(DbConnection connection, CancellationToken cancellationToken)
        {
            await using (DbCommand createVersionTable = connection.CreateCommand())
            {
                createVersionTable.CommandText = "create table __version__ (current_version int not null);";
                await createVersionTable.ExecuteNonQueryAsync(cancellationToken);
            }

            await using DbCommand insertVersion = connection.CreateCommand();
            insertVersion.CommandText = "insert into __version__ (current_version) values (0);";
            await insertVersion.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}