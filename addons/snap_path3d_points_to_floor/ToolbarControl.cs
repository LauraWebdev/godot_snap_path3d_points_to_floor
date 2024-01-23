using Godot;
using Godot.Collections;

namespace media.laura.snap_path3d_points_to_floor;

[Tool]
public partial class ToolbarControl : HBoxContainer
{
	private Array<Path3D> _selectedPath3Ds = new();

	public override void _EnterTree()
	{
		_selectedPath3Ds.Clear();
		
		foreach (var selectedNode in EditorInterface.Singleton.GetSelection().GetSelectedNodes())
		{
			if (selectedNode is Path3D path3dNode)
			{
				_selectedPath3Ds.Add(path3dNode);
			}
		}
	}

	public void SnapPointsToFloor()
	{
		foreach (var path3D in _selectedPath3Ds)
		{
			for (var i = 0; i < path3D.Curve.PointCount; i++)
			{
				var pointPosition = path3D.Curve.GetPointPosition(i);
				
				// Find floor from point downwards
				var spaceState = path3D.GetWorld3D().DirectSpaceState;
				var query = PhysicsRayQueryParameters3D.Create(pointPosition, Vector3.Down * 100000f);
				var result = spaceState.IntersectRay(query);
				
				path3D.Curve.SetPointPosition(i, result["position"].AsVector3());
			}
		}
	}

	public override void _ExitTree()
	{
		_selectedPath3Ds.Clear();
	}
}
