using Godot;
using System;

public class GUI : Node
{
	public LivesCounter LivesCounter = null;
	public Label ScoreCounter = null;
	public Label CupCollectedLabel = null;
	public Control PauseMenu = null;
    public bool QuitButtonPressed = false;

	public bool PausedMenuOpen
	{
		get
		{
			return PauseMenu?.Visible ?? false;
		}
	}

	public override void _Ready()
	{
		var container = GetNode<Control>("Container");
		if (container != null)
		{
			LivesCounter = container.GetNode<LivesCounter>("LivesCounter");
			ScoreCounter = container.GetNode<Label>("ScoreCounter");
			CupCollectedLabel = container.GetNode<Label>("CupCollectedLabel");
			PauseMenu = container.GetNode<Control>("PauseMenu");

			container.Visible = true;
		}
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			TogglePause();
		}
	}

	public void TogglePause()
	{
		if (PauseMenu == null) return;

		PauseMenu.Visible = !PauseMenu.Visible;
		GetTree().Paused = PauseMenu.Visible;
	}

	public void SetLives(int lives) {
		if (LivesCounter == null) return;

		for (int i = 1; i <= 6; i++) {
			
		}
	}

	private void _on_ContinueButton_pressed()
	{
		TogglePause();
	}

	private void _on_QuitButton_pressed()
	{
		QuitButtonPressed = true;
		TogglePause();
	}
}
