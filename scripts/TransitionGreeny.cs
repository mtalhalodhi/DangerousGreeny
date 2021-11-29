using Godot;
using System;

public class TransitionGreeny : Area2D
{
	[Export] public float Speed = 1;

	public bool TransitionCompleted = false;

    public override void _Ready()
    {
        Connect("area_entered", this, "AreaEntered");
    }

    public override void _Process(float delta)
    {
        Position = new Vector2(Position.x + Speed * delta, Position.y);
    }
	public void AreaEntered(Area2D area)
	{
		if (area.Name == "TransitionEnd")
		{
			TransitionCompleted = true;
		}
	}
}
