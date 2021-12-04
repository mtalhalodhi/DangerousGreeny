using Godot;
using System;

public class ShooterEnemy : Node2D
{
	[Export] public PackedScene Bullet;
	[Export] public float FireDelay = 2;
	float fireCooldown = 0;

	public Area2D vision;
	Greeny greeny;
	bool targetInRange = false;

	public override void _Ready()
	{
		var vision = GetNode<Area2D>("Vision");
		//vision.Connect("body_entered ", this, "BodyEntered");
		//vision.Connect("body_exited ", this, "BodyExited");
	}

	public override void _Process(float delta)
	{
		if (fireCooldown > 0)
		{
			fireCooldown -= delta;
		}

		if (fireCooldown <= 0 && targetInRange)
		{
			var bullet = Bullet.Instance() as Bullet;
			bullet.PlayerBullet = false;
			GetTree().Root.AddChild(bullet);
			bullet.GlobalPosition = GlobalPosition;
			bullet.GlobalRotation = GlobalRotation;
			bullet.Rotate(GetAngleTo(greeny.GlobalPosition));
			fireCooldown = FireDelay;
		}
	}

	private void _on_Vision_body_entered(object body)
	{
		if (body is Greeny)
		{
			targetInRange = true;
			greeny = body as Greeny;
		}
	}


	private void _on_Vision_body_exited(object body)
	{
		if (body is Greeny)
		{
			targetInRange = false;
		}
	}

	public void Die()
	{
		QueueFree();
	}

}
