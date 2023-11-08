#if TOOLS
using Godot;

namespace OffWorld.addons.OffWorldDevTools;

[Tool]
public partial class DevTools : EditorPlugin
{
	private Control? _dock;

	public override void _EnterTree()
	{
		_dock = GD.Load<PackedScene>("res://addons/OffWorldDevTools/DefinitionsDeserializerDock.tscn").Instantiate<Control>();
		AddControlToDock(DockSlot.RightBl, _dock);
	}

	public override void _ExitTree()
	{
		if (_dock == null)
			return;
		RemoveControlFromDocks(_dock);
		_dock.Free();
	}
}

#endif
