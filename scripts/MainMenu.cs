using Godot;
using System;

public class MainMenu : Node2D
{
	public bool Start = false;

	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("ui_accept"))
		{
			Start = true;
		}
	}
}
