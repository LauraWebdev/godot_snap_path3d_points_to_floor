#if TOOLS
using System.Linq;
using Godot;
using Godot.Collections;

namespace media.laura.snap_path3d_points_to_floor;

[Tool]
public partial class Plugin : EditorPlugin
{
	private Array<Node> _editorSelection = new();
	private Control _toolbarControl;
	private bool _toolbarControlAdded;
	
	public override void _EnterTree()
	{
		_toolbarControl = GD.Load<PackedScene>("res://addons/snap_path3d_points_to_floor/toolbar_control.tscn").Instantiate<Control>();
		
	}

	public override void _Process(double delta)
	{
		var currentSelection = EditorInterface.Singleton.GetSelection().GetSelectedNodes();
		if (!_editorSelection.SequenceEqual(currentSelection))
		{
			_editorSelection = currentSelection;

			if (_editorSelection.All(x => x.GetType() == typeof(Path3D)) && !_toolbarControlAdded)
			{
				_toolbarControlAdded = true;
				AddControlToContainer(CustomControlContainer.SpatialEditorMenu, _toolbarControl);
			}
			else
			{
				if (_toolbarControlAdded)
				{
					_toolbarControlAdded = false;
					RemoveControlFromContainer(CustomControlContainer.SpatialEditorMenu, _toolbarControl);
				}
			}
		}

	}

	public override void _ExitTree()
	{
		if (_toolbarControlAdded)
		{
			_toolbarControlAdded = false;
			RemoveControlFromContainer(CustomControlContainer.SpatialEditorMenu, _toolbarControl);
		}
	}
}
#endif
