using Godot;
using System;

public class GUI : Node
{
    public HBoxContainer LivesCounter = null;
    public Label ScoreCounter = null;
    public Label CupCollectedLabel = null;

    public override void _Ready()
    {
        var container = GetNode<Control>("Container");
        if (container != null) {
            LivesCounter = container.GetNode<HBoxContainer>("LivesCounter");
            ScoreCounter = container.GetNode<Label>("ScoreCounter");
            CupCollectedLabel = container.GetNode<Label>("CupCollectedLabel");

            container.Visible = true;
        }
    }
}
