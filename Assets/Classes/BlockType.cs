public record BlockType
{
    public string Name { get; }

    public bool IsSolid { get; }

    public BlockType(string name, bool isSolid)
    {
        Name    = name;
        IsSolid = isSolid;
    }
}