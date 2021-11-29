using Godot;

public class Greeny : KinematicBody2D
{
	[Export] public int Score = 0;
	[Export] public bool CupCollected = false;

	[Export] public float Speed = 0;
	[Export] public float JumpSpeed = 0;
	[Export] public float Gravity = 0;

	Vector2 velocity = Vector2.Zero;

	public override void _Ready()
	{
		var area = GetNode<Area2D>("Area2D");
		if (area != null)
		{
			area.Connect("area_entered", this, "AreaEntered");
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		velocity.x = 0;
		if (Input.IsActionPressed("walk_right"))
		{
			velocity.x += Speed;
		}
		if (Input.IsActionPressed("walk_left"))
		{
			velocity.x -= Speed;
		}

		velocity.y += Gravity * delta;
		velocity = MoveAndSlide(velocity, Vector2.Up, true);
		if (Input.IsActionJustPressed("jump"))
		{
			if (IsOnFloor())
			{
				velocity.y = -JumpSpeed;
			}
		}
	}

	public void AreaEntered(Area2D area)
	{
		if (area is Gem)
		{
			var gem = area as Gem;
			Score += gem.Score;
			gem.QueueFree();
		}
		if (area.Name.ToLower().Contains("cup"))
		{
			Score += 500;
			CupCollected = true;
			area.QueueFree();
		}
		if (area.Name.ToLower().Contains("door"))
		{
			if (CupCollected)
			{
				GetTree().Quit();
			}
		}
	}
}
