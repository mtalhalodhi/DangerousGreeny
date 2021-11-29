using Godot;
using System;

public class GameManager : Node
{
	public Greeny Greeny = null;
	public GUI GUI = null;
	public int Lives = 3;

	public override void _Ready()
	{
		Greeny = GetNode<Greeny>("Level/Greeny");
		GUI = GetNode<GUI>("GUI");

		GUI.LivesCounter.SetLives(3);
	}

	public override void _Process(float delta)
	{
		if (GUI != null)
		{
			GUI.ScoreCounter.Text = Greeny?.Score.ToString();
			GUI.CupCollectedLabel.Visible = Greeny?.CupCollected ?? false;
		}
	}
}
