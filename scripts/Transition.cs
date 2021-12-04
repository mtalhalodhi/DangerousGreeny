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
        SetLevelsLeft(levelsLeft);
    }

    public void SetLevelsLeft(int value)
    {
        levelsLeft = value;
        if (label != null)
        {
            if (value == 0)
            {
                label.Text = $"YOU WON!!!";
            }
            if (value == 1)
            {
                label.Text = $"THIS IS THE LAST LEVEL!!!";
            }
            else
            {
                label.Text = $"GOOD WORK! ONLY {levelsLeft} MORE TO GO!";
            }
        }
    }
}
