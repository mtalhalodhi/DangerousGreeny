using Godot;
using System;

public class Gun : Node2D
{
    [Export] public PackedScene Bullet;
    [Export] public float FireDelay = 1;
    float fireCooldown = 0;

    AnimatedSprite sprite = null;
    Node2D bulletSpawn = null;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite>("Sprite");
        bulletSpawn = GetNode<Node2D>("Sprite/BulletSpawn");
    }

    public override void _Process(float delta)
    {
        if (fireCooldown > 0) {
            fireCooldown -= delta;
        }

        var rot = GetAngleTo(GetGlobalMousePosition());
        Rotate(rot);

        sprite.FlipV = GetGlobalMousePosition().x < GlobalPosition.x;

        if (Input.IsActionJustPressed("fire_gun"))
        {
            if (fireCooldown <= 0) {
                var bullet = Bullet.Instance() as Bullet;
                GetTree().Root.AddChild(bullet);
                bullet.GlobalPosition = bulletSpawn.GlobalPosition;
                bullet.GlobalRotation = bulletSpawn.GlobalRotation;
                bullet.PlayerBullet = true;
                fireCooldown = FireDelay;
            }
        }
    }
}
