using Godot;

public class Greeny : KinematicBody2D
{
	public enum GreenyState { Alive, Dying, Dead }

	[Export] public int Score = 0;
	[Export] public bool CupCollected = false;
	[Export] public bool LevelCompleted = false;
	[Export] public float Speed = 0;
	[Export] public float JumpSpeed = 0;
	[Export] public float Gravity = 0;
	[Export] bool HasGun = false;
	[Export] bool HasJetpack = false;
	[Export] bool Flying = false;
	[Export] float MaxFuel = 15;
	[Export] float Fuel = 0;
	public GreenyState State = GreenyState.Alive;

	Vector2 velocity = Vector2.Zero;
	AnimatedSprite sprite = null;
	AnimatedSprite deathSprite = null;
	AnimatedSprite jetpackSprite = null;
	Label fuelLabel = null;
	Gun gun = null;

	public override void _Ready()
	{
		var area = GetNode<Area2D>("Area2D");
		if (area != null)
		{
			area.Connect("area_entered", this, "AreaEntered");
		}

		sprite = GetNode<AnimatedSprite>("Sprite");
		jetpackSprite = GetNode<AnimatedSprite>("JetpackSprite");
		fuelLabel = GetNode<Label>("JetpackSprite/FuelLabel");
		deathSprite = GetNode<AnimatedSprite>("DeathSprite");
		deathSprite.Connect("animation_finished", this, "DeathAnimationFinished");

		gun = GetNode<Gun>("Gun");
		gun.Visible = HasGun;
		gun.SetProcess(HasGun);
	}

	public override void _PhysicsProcess(float delta)
	{
		if (State == GreenyState.Alive)
		{
			gun.Visible = HasGun;
			gun.SetProcess(HasGun);

			if (Input.IsActionJustPressed("fly") && HasJetpack)
			{
				if (Fuel > 0) {
					Flying = !Flying;
					jetpackSprite.Visible = Flying;
				}
			}

			velocity.x = 0;
			if (Input.IsActionPressed("right"))
			{
				velocity.x += Speed;
			}
			if (Input.IsActionPressed("left"))
			{
				velocity.x -= Speed;
			}
			if (Flying)
			{
				velocity.y = 0;
				if (Input.IsActionPressed("up"))
				{
					velocity.y -= Speed;
				}
				if (Input.IsActionPressed("down"))
				{
					velocity.y += Speed;
				}

				Fuel -= delta;
				if (Fuel <= 0) {
					Fuel = 0;
					Flying = false;
					jetpackSprite.Visible = false;
				}

				fuelLabel.Text = $"{Mathf.Round(Fuel / MaxFuel * 100)}%";
			}

			if (!Flying) velocity.y += Gravity * delta;
			velocity = MoveAndSlide(velocity, Flying ? Vector2.Zero : Vector2.Up, true);

			if (Input.IsActionJustPressed("jump") && !Flying)
			{
				if (IsOnFloor())
				{
					velocity.y = -JumpSpeed;
				}
			}
		}
		else
		{
			gun.Visible = false;
			gun.SetProcess(false);
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
			LevelCompleted = CupCollected;
		}
		if (area.Name.ToLower().Contains("gun"))
		{
			HasGun = true;
			gun.Visible = HasGun;
			gun.SetProcess(HasGun);
			area.QueueFree();
		}
		if (area.Name.ToLower().Contains("jetpack"))
		{
			HasJetpack = true;
			Fuel = MaxFuel;
			area.QueueFree();
		}
		if (area.Name.ToLower().Contains("fire") || area.Name.ToLower().Contains("water") || area.Name.ToLower().Contains("tentacle"))
		{
			Die();
		}
	}

	public void ReSpawn(Vector2 position)
	{
		State = GreenyState.Alive;
		Position = position;
		sprite.Visible = true;
	}

	public void DeathAnimationFinished()
	{
		if (State == GreenyState.Dying)
		{
			State = GreenyState.Dead;
			deathSprite.Visible = false;
		}
	}

	public void Die() 
	{
			State = GreenyState.Dying;
			sprite.Visible = false;
			deathSprite.Visible = true;
			deathSprite.Frame = 0;
			deathSprite.Play();
	}
}
