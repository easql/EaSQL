using EaSQL.Query;
using System.Data;

namespace EaSQL.DbInit
{
    /// <summary>
    /// Allows a simple method to manage database versions.
    /// </summary>
    public class DbHandler : IVersionSetup
    {
        private readonly Dictionary<long, List<Step>> _setupSteps = [];

        /// <summary>
        /// Defines a new version for the database.
        /// </summary>
        /// <returns>An instance allowing to define setup steps for this version.</returns>
        public IVersionSetup NewVersion()
        {
            _setupSteps.Add(_setupSteps.Count + 1, []);
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
        public void Setup(IDbConnection connection)
        {
            long currentVersion = 0;

            using (IDbCommand getVersion = connection.CreateCommand())
            {
                getVersion.CommandText = "select current_version from __version__";

                try
                {
                    object? getVersionResult = getVersion.ExecuteScalar();
                    if (getVersionResult is long dbVersion)
                    {
                        currentVersion = dbVersion;
                    }
                }
                catch
                {
                    CreateVersionTable(connection);
                }
            }

            foreach ((long version, List<Step> steps) in _setupSteps)
            {
                if (version <= currentVersion) continue;

                foreach(Step step in steps)
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

                    using IDbCommand executeStep = connection.CreateCommand();
                    executeStep.CommandText = step.Command;
                    executeStep.ExecuteNonQuery();
                }

                connection.RunCommand($"update __version__ set current_version = {version}");
            }
        }

        private static void CreateVersionTable(IDbConnection connection)
        {
            using (IDbCommand createVersionTable = connection.CreateCommand())
            {
                createVersionTable.CommandText = "create table __version__ (current_version int not null);";
                createVersionTable.ExecuteNonQuery();
            }
            
            using IDbCommand insertVersion = connection.CreateCommand();
            insertVersion.CommandText = "insert into __version__ (current_version) values (0);";
            insertVersion.ExecuteNonQuery();
        }
    }
}
