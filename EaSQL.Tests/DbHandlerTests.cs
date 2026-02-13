using EaSQL.DbInit;
using Microsoft.Data.Sqlite;

namespace EaSQL.Tests
{
    public class DbHandlerTests
    {
        [Fact]
        public async Task Setup_ItInitializesTheDatabase()
        {
            await using SqliteConnection connection = new("Data Source=:memory:");
            connection.Open();

            DbHandler handler = new();
            await handler.Setup(connection);

            long version = (long)new SqliteCommand("select current_version from __version__", connection).ExecuteScalar()!;

            Assert.Equal(0, version);
        }

        [Fact]
        public void Setup_CreatesANewDatabase()
        {
            using SqliteConnection connection = new("Data Source=:memory:");
            connection.Open();

            DbHandler handler = new();

            handler.NewVersion()
                .AddStep("create table test(id int not null primary key, value varchar(50) null);")
                .AddStep("create table test2(id int not null primary key, value varchar(30) not null);");
            handler.NewVersion()
                .AddStep("create table test3(id int not null primary key, value varchar(50) null);")
                .AddStep("alter table test2 add column value2 long");

            handler.Setup(connection);

            long version = (long)new SqliteCommand("select current_version from __version__", connection).ExecuteScalar()!;

            Assert.Equal(2, version);
        }

        [Fact]
        public void Setup_UpdatesDatabase()
        {
            using SqliteConnection connection = new("Data Source=:memory:");
            connection.Open();

            DbHandler handler = new();

            handler.NewVersion()
                .AddStep("create table test(id int not null primary key, value varchar(50) null);")
                .AddStep("create table test2(id int not null primary key, value varchar(30) not null);");
            handler.NewVersion()
                .AddStep("create table test3(id int not null primary key, value varchar(50) null);")
                .AddStep("alter table test2 add column value2 long");

            handler.Setup(connection);

            handler.NewVersion()
                .AddStep("drop table test3");

            handler.Setup(connection);

            long version = (long)new SqliteCommand("select current_version from __version__", connection).ExecuteScalar()!;

            Assert.Equal(3, version);
        }

        [Fact]
        public async Task Setup_CreatesANewDatabaseWithVersions()
        {
            await using SqliteConnection connection = new("Data Source=:memory:");
            connection.Open();

            DbHandler handler = new();

            handler
                .AddVersion(s =>
                {
                    s.AddTable("test", t =>
                    {
                        t.AddColumn("id", c =>
                        {
                            c.HasType(ColumnType.Int)
                                .IsNotNull()
                                .AsPrimaryKey();
                        });
                        t.AddColumn("value", c =>
                        {
                            c.HasType(ColumnType.Varchar)
                                .WithLength(50);
                        });
                    });
                    s.AddTable("test2", t =>
                    {
                        t.AddColumn("id", c =>
                        {
                            c.HasType(ColumnType.Int)
                                .IsNotNull()
                                .AsPrimaryKey();
                        });
                        t.AddColumn("value", c =>
                        {
                            c.HasType(ColumnType.Varchar)
                                .WithLength(50)
                                .IsNotNull();
                        });
                    });
                })
                .AddVersion(s =>
                {
                    s.AddTable("test3", t =>
                    {
                        t.AddColumn("id", c =>
                        {
                            c.HasType(ColumnType.Int)
                                .IsNotNull()
                                .AsPrimaryKey();
                        });
                        t.AddColumn("value", c =>
                        {
                            c.HasType(ColumnType.Varchar)
                                .WithLength(50);
                        });
                    });
                });

            await handler.Setup(connection);

            long version = (long)new SqliteCommand("select current_version from __version__", connection).ExecuteScalar()!;

            Assert.Equal(2, version);
        }
    }
}
