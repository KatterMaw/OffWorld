namespace OffWorld.Extensibility;

public class Mod
{
	public ModInformation Information { get; }
	public ModContent Content { get; }

	internal Mod(ModInformation information, ModContent content)
	{
		Information = information;
		Content = content;
	}
}