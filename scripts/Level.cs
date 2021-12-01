using Godot;
using System;

public class Level : Node2D
{
	public int Lives = 3;
	public int Score = 0;
	public bool LevelCompleted = false;
	public bool LevelQuit = false;
	public bool LevelLost = false;
	public Greeny Greeny = null;
	public GUI GUI = null;

	Vector2 spawn;

	public override void _Ready()
	{
		Greeny = GetNode<Greeny>("Greeny");
		GUI = GetNode<GUI>("GUI");

		Greeny.Score = Score;
		spawn = Greeny.Position;
	}

	public override void _Process(float delta)
	{
		GUI.ScoreCounter.Text = Greeny.Score.ToString();
		GUI.CupCollectedLabel.Visible = Greeny.CupCollected;
		GUI.LivesCounter.SetLives(Lives);
		LevelQuit = GUI.QuitButtonPressed;

		LevelCompleted = Greeny.LevelCompleted;
		if (LevelCompleted) {
			Score = Greeny.Score;
		}

		if (Greeny.State == Greeny.GreenyState.Dead) {
			Greeny.ReSpawn(spawn);
			Lives--;
		}

		if (Lives < 0) {
			LevelLost = true;
		}
	}
}
