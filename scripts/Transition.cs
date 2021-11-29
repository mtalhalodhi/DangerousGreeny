using Godot;
using System;

public class Transition : Node2D
{
	public bool TransitionCompleted = false;

	TransitionGreeny transitionGreeny = null;
	Label label = null;
	int levelsLeft = 0;


	public override void _Ready()
	{
		transitionGreeny = GetNode<TransitionGreeny>("TransitionGreeny");
		label = GetNode<Label>("GUI/Label");
	}

	public override void _Process(float delta)
	{
		TransitionCompleted = transitionGreeny.TransitionCompleted;
		label.Text = $"GOOD WORK! ONLY {levelsLeft} MORE TO GO!";
	}

	public void SetLevelsLeft(int value)
	{
		levelsLeft = value;
		if (label != null)
		{
			label.Text = $"GOOD WORK! ONLY {levelsLeft} MORE TO GO!";
		}
	}
}
