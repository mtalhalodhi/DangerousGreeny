using Godot;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameManager : Node
{
	public enum GameState { MainMenu, Transition, Level, WinScreen, ScoreScreen }

	[Export] public PackedScene MainMenu;
	[Export] public PackedScene Transition;
	[Export] public PackedScene WinScene;
	[Export] public PackedScene ScoreScene;
	[Export] public PackedScene[] Levels;

	public GameState State;
	public int Lives = 3;
	public int Score = 0;
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
				else
				{
					LoadWinMenu();
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
				Score = level.Score;

				var transition = Transition.Instance() as Transition;
				transition.SetLevelsLeft(Levels.Length - CurrentLevel - 1);

				State = GameState.Transition;
				current = transition;
				AddChild(current);
			}
			if (level.LevelQuit)
			{
				LoadMainMenu();
			}
			if (level.LevelLost)
			{
				LoadScoreMenu();
			}
		}
		if (State == GameState.WinScreen) {
			if (Input.IsActionPressed("ui_accept"))
			{
				LoadScoreMenu();
			}
		}
		if (State == GameState.ScoreScreen) {
			var level = current as Node2D;
			var gui = current.GetNode<ScoreGui>("GUI");

			if (gui.CancelButtonPressed) 
			{
				LoadMainMenu();
			}
			else if (gui.SaveButtonPressed) 
			{
				SaveScore(gui.NameTextBox.Text, Score);
				LoadMainMenu();
			}
		}
	}

	public void LoadLevel(int index)
	{
		MakeSureScoresAreUpToDate();

		if (State == GameState.MainMenu)
		{
			Lives = 3;
			Score = 0;
		}

		State = GameState.Level;
		var level = Levels[index].Instance() as Level;
		CurrentLevel = index;
		level.Lives = Lives;
		level.Score = Score;

		current.QueueFree();
		current = level;
		AddChild(current);
	}

	public void MakeSureScoresAreUpToDate() 
	{
		if (State == GameState.Level)
		{
			var currentLevel = current as Level;
			Lives = currentLevel.Lives;
			Score = currentLevel.Score;
		}
	}

	public void LoadMainMenu()
	{
		current.QueueFree();

		Lives = 3;
		CurrentLevel = -1;
		Score = 0;
		State = GameState.MainMenu;
		current = MainMenu.Instance();
		AddChild(current);
	}

	public void LoadWinMenu() 
	{
		MakeSureScoresAreUpToDate();

		var scene = WinScene.Instance();

		State = GameState.WinScreen;
		current.QueueFree();
		current = scene;
		AddChild(current);
	}

	public void LoadScoreMenu()
	{
		MakeSureScoresAreUpToDate();

		var scene = ScoreScene.Instance();

		State = GameState.ScoreScreen;
		current.QueueFree();
		current = scene;
		AddChild(current);

		var gui = scene.GetNode<ScoreGui>("GUI");

		gui.CurrentScore.Text = $"YOUR SCORE : {Score}";
		var scores = LoadScores();
		string scoreText = "";
		foreach (var score in scores)
		{
			scoreText += $"{score.Name} : {score.Score}\n";
		}
		gui.HighScores.Text = scoreText;
	}

	public List<(string Name, int Score)> LoadScores()
	{
		var file = new File();

		if (!file.FileExists("user://scores.json"))
		{
			file.Open("user://scores.json", File.ModeFlags.Write);
			file.Close();
		}

		file.Open("user://scores.json", File.ModeFlags.Read);
		var content = file.GetAsText();
		file.Close();

		if (content.Empty()) content = "[]";

		return JsonConvert.DeserializeObject<List<(string Name, int Score)>>(content).OrderBy(x => x.Score).Reverse().ToList();
	}

	public void SaveScore(string name, int score)
	{
		var scores = LoadScores();
		scores.Add((name, score));
		scores = scores.OrderBy(x => x.Score).Reverse().ToList();
		var text = JsonConvert.SerializeObject(scores);

		var file = new File();
		file.Open("user://scores.json", File.ModeFlags.Write);
		file.StoreString(text);
		file.Close();
	}
}
