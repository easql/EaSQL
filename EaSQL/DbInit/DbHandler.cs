using EaSQL.Query;
using System.Data;

namespace EaSQL.DbInit
{
    public class DbHandler : IVersionSetup
    {
        private readonly Dictionary<long, List<string>> _setupSteps = [];

        public IVersionSetup NewVersion()
        {
            _setupSteps.Add(_setupSteps.Count + 1, []);
            return this;
        }

        IVersionSetup IVersionSetup.AddStep(string command)
        {
            _setupSteps[_setupSteps.Count].Add(command);
            return this;
        }

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

            SqlQueryHandler handler = new(connection);
            foreach ((long version, List<string> steps) in _setupSteps)
            {
                if (version <= currentVersion) continue;

                foreach(string step in steps)
                {
                    using IDbCommand executeStep = connection.CreateCommand();
                    executeStep.CommandText = step;
                    executeStep.ExecuteNonQuery();
                }

                handler.CreateCommand($"update __version__ set current_version = {version}").ExecuteNonQuery();
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
