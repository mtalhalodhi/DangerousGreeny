using Godot;
using System;

public class Level : Node2D
{
	public int Lives = 3;
	public bool LevelCompleted = false;
	public bool LevelQuit = false;

	public Greeny Greeny = null;
	public GUI GUI = null;

	public override void _Ready()
	{
		Greeny = GetNode<Greeny>("Greeny");
		GUI = GetNode<GUI>("GUI");
	}

	public override void _Process(float delta)
	{
		GUI.ScoreCounter.Text = Greeny.Score.ToString();
		GUI.CupCollectedLabel.Visible = Greeny.CupCollected;
		GUI.LivesCounter.SetLives(Lives);
		LevelQuit = GUI.QuitButtonPressed;

		LevelCompleted = Greeny.LevelCompleted;
	}
}
