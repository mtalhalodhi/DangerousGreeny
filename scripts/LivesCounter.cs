using Godot;
using System;

public class LivesCounter : HBoxContainer
{
    public void SetLives(int lives) {
        
        for (int i = 0; i < GetChildCount(); i++) {
            var child = GetChild<Control>(i);
            child.Visible = i < lives;
        }

    }
}
