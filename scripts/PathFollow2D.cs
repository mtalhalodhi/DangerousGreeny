using Godot;
using System;

public class PathFollower : PathFollow2D
{
	[Export] public float TotalTime = 5;

	public override void _Ready()
	{
		
	}

    public override void _Process(float delta)
    {
		UnitOffset += (delta / TotalTime);
    }
}
