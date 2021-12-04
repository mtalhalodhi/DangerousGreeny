using Godot;
using System;

public class DissolvingPlatform : Area2D
{
	AnimatedSprite sprite = null;
	StaticBody2D platform = null;

	public override void _Ready()
	{
		Connect("area_entered", this, "AreaEntered");

		sprite = GetNode<AnimatedSprite>("Sprite");
		sprite.Connect("animation_finished", this, "AnimationFinished");

		platform = GetNode<StaticBody2D>("Platform");
	}
	
	public void AreaEntered(Area2D area) {
		if (area.Name == "GreenyArea")
		{
			platform.QueueFree();
			sprite.Play();
		}
	}

	public void AnimationFinished()
	{
		sprite.Stop();
		QueueFree();
	}
}
