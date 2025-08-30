namespace EaSQL.DbInit
{
    public interface IVersionSetup
    {
        IVersionSetup AddStep(string command);
    }
}
