using Godot;
using System;

public class Bullet : KinematicBody2D
{
    [Export] public float Speed = 200;
    public bool PlayerBullet = false;

    public override void _Process(float delta)
    {
        var vec = new Vector2(Speed * delta, 0);
        vec = vec.Rotated(Rotation);

        var col = MoveAndCollide(vec);
        if (col != null)
        {
            var shape = col.Collider;
			bool destroy = true;

            if (shape != null)
            {
                if (PlayerBullet)
                {
                    if (shape is ShooterEnemy)
                    {
                        (shape as ShooterEnemy).Die();
                    }
                }
                else
                {
                    if (shape is Greeny)
                    {
                        (shape as Greeny).Die();
                    }
					if (shape is ShooterEnemy) {
						destroy = false;
					}
                }
            }

			if (destroy) QueueFree();
        }
    }
}

