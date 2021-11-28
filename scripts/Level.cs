using Godot;
using System;

public class Level : Node
{
    public Greeny Greeny = null;
    public GUI GUI = null;

    public override void _Ready()
    {
        Greeny = GetNode<Greeny>("Greeny");
        GUI = GetNode<GUI>("GUI");
    }

    public override void _Process(float delta)
    {
        if (GUI != null) {
            GUI.ScoreCounter.Text = Greeny?.Score.ToString();
            GUI.CupCollectedLabel.Visible = Greeny?.CupCollected ?? false;
        }
    }
}
