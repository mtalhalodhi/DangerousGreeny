using Godot;
using System;

public class GameManager : Node
{
	public enum GameState { MainMenu, Transition, Level }

	[Export] public PackedScene MainMenu;
	[Export] public PackedScene Transition;
	[Export] public PackedScene[] Levels;

	public GameState State;
	public int Lives = 3;
	public int CurrentLevel = -1;

	private Node current = null;

	public override void _Ready()
	{
		State = GameState.MainMenu;
		current = MainMenu.Instance();
		AddChild(current);
	}

	public override void _Process(float delta)
	{
		if (State == GameState.MainMenu)
		{
			var menu = (MainMenu)current;
			if (menu.Start)
			{
				current.QueueFree();
				LoadLevel(0);
			}
		}
		if (State == GameState.Transition)
		{
			var transition = current as Transition;
			if (transition.TransitionCompleted) 
			{
				transition.QueueFree();

				CurrentLevel++;
				if (CurrentLevel < Levels.Length)
				{
					LoadLevel(CurrentLevel);
				}
			}
		}
		if (State == GameState.Level)
		{
			var level = current as Level;
			if (level.LevelCompleted)
			{
				current.QueueFree();
				Lives = level.Lives;

				var transition = Transition.Instance() as Transition;
				transition.SetLevelsLeft(Levels.Length - CurrentLevel - 1);

				State = GameState.Transition;
				current = transition;
				AddChild(current);
			}
			if (level.LevelQuit)
			{
				current.QueueFree();

				Lives = 3;
				CurrentLevel = -1;
				State = GameState.MainMenu;
				current = MainMenu.Instance();
				AddChild(current);
			}
		}
	}

	public void LoadLevel(int index)
	{
		Lives = 3;

		if (State == GameState.Level)
		{
			var currentLevel = current as Level;
			Lives = currentLevel.Lives;
		}

		State = GameState.Level;
		var level = Levels[index].Instance() as Level;
		CurrentLevel = index;
		level.Lives = Lives;

		current = level;
		AddChild(current);
	}
}
