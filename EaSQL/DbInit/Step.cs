namespace EaSQL.DbInit
{
    internal sealed class Step
    {
        public required string Command { get; set; }
        public string? GuardQuery { get; set; }
    }
}
