public record BlockType
{
    public string Name { get; }

    public bool IsSolid { get; }

    public string MeshName { get; }

    public BlockType(string name, bool isSolid, string mesh)
    {
        Name    = name;
        IsSolid = isSolid;
        MeshName    = mesh;
    }
}