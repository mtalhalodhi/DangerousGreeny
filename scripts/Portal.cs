using Godot;
using System;

[Tool]
public class Portal : Area2D
{
	[Export] public bool CancelVelocity = false;
	public Node2D Target = null;

	public override void _Ready()
	{
		Target = GetNode<Node2D>("Target");
	}

	public override string _GetConfigurationWarning()
	{
		if (!HasNode("Target"))return "Target:Node2D Required";
		return "";
	}
}
