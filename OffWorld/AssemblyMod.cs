namespace OffWorld;

public sealed class AssemblyMod : Mod
{
	public ModAssembly Assembly { get; }

	internal AssemblyMod(ModInformation information, ModContent content, ModAssembly assembly) : base(information, content)
	{
		Assembly = assembly;
	}
}