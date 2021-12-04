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
    [Export] public bool HasGun = false;
    [Export] public bool HasJetpack = false;
    [Export] public bool Flying = false;
    [Export] public float MaxFuel = 15;
    [Export] public float Fuel = 0;
    [Export] public bool Climbing = false;
    public bool DueForALife = false;
    public GreenyState State = GreenyState.Alive;

    Vector2 velocity = Vector2.Zero;
    AnimatedSprite sprite = null;
    AnimatedSprite deathSprite = null;
    AnimatedSprite jetpackSprite = null;
    Label fuelLabel = null;
    Gun gun = null;

    public override void _Ready()
    {
        var area = GetNode<Area2D>("GreenyArea");
        if (area != null)
        {
            area.Connect("area_entered", this, "AreaEntered");
            area.Connect("area_exited", this, "AreaExited");
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
                if (Fuel > 0)
                {
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
            if (Flying || Climbing)
            {
                velocity.y = 0;
                if (Input.IsActionPressed("up") || Input.IsActionPressed("jump"))
                {
                    velocity.y -= Speed;
                }
                if (Input.IsActionPressed("down"))
                {
                    velocity.y += Speed;
                }

                if (Flying)
                {
                    Fuel -= delta;
                    if (Fuel <= 0)
                    {
                        Fuel = 0;
                        Flying = false;
                        jetpackSprite.Visible = false;
                    }
                    fuelLabel.Text = $"{Mathf.Round(Fuel / MaxFuel * 100)}%";
                }
            }

            if (!(Flying || Climbing)) velocity.y += Gravity * delta;
            velocity = MoveAndSlide(velocity, (Flying || Climbing) ? Vector2.Zero : Vector2.Up, true);

            if (Input.IsActionJustPressed("jump") && !(Flying || Climbing))
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

    public void AddScore(int value)
    {
        for (int i = 1; i < 10; i++)
        {
            DueForALife = (Score < (i * 5000)) && ((Score + value) >= (i * 5000));
            if (DueForALife) break;
        }
        Score += value;
    }

    public void AreaEntered(Area2D area)
    {
        if (area is Gem)
        {
            var gem = area as Gem;
			AddScore(gem.Score);
            gem.QueueFree();
        }
        if (area.Name.ToLower().Contains("cup"))
        {
            AddScore(500);
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
            RefillJetpack();
            area.QueueFree();
        }
        if (area.Name.ToLower().Contains("fire") || area.Name.ToLower().Contains("water") || area.Name.ToLower().Contains("tentacle"))
        {
            Die();
        }
        if (area.Name.ToLower().Contains("enemy"))
        {
            Die();
        }
        if (area.Name.ToLower().Contains("rope"))
        {
            Climbing = true;
        }
        if (area is Portal)
        {
			var portal = area as Portal;
			if (portal.CancelVelocity) velocity = Vector2.Zero;
            GlobalPosition =portal.Target.GlobalPosition;
        }
    }

    public void RefillJetpack()
    {
        HasJetpack = true;
        Fuel = MaxFuel;
    }

    public void AreaExited(Area2D area)
    {
        if (area.Name.ToLower().Contains("rope"))
        {
            Climbing = false;
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
        Flying = false;
        jetpackSprite.Visible = false;
        velocity = Vector2.Zero;
        State = GreenyState.Dying;
        sprite.Visible = false;
        deathSprite.Visible = true;
        deathSprite.Frame = 0;
        deathSprite.Play();
    }
}
