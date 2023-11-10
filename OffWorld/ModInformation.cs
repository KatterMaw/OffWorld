namespace OffWorld;

public sealed class ModInformation
{
	public string Id { get; }
	public string Name { get; }
	public string Directory { get; }

	internal ModInformation(string id, string name, string directory)
	{
		Id = id;
		Name = name;
		Directory = directory;
	}
}