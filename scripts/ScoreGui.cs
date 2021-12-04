using Godot;
using System;

public class ScoreGui : CanvasLayer
{
	public Label CurrentScore;
	public LineEdit NameTextBox;
	public Button SaveButton;
	public Button CancelButton;
	public bool SaveButtonPressed = false;
	public bool CancelButtonPressed = false;
	public Label HighScores;

	public override void _Ready()
	{
		CurrentScore = GetNode<Label>("Container/CurrentScore");
		NameTextBox = GetNode<LineEdit>("Container/NameTextBox");
		SaveButton = GetNode<Button>("Container/SaveButton");
		CancelButton = GetNode<Button>("Container/CancelButton");
		HighScores = GetNode<Label>("Container/HighScores");

		SaveButton.Connect("pressed", this, "saveSignal");
		CancelButton.Connect("pressed", this, "cancelSignal");
	}

	void saveSignal()
	{
		SaveButtonPressed = true;
	}

	void cancelSignal()
	{
		CancelButtonPressed = true;
	}
}
