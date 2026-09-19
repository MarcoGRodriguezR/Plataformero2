using Godot;
using System;

public partial class Menu : Node2D
{
	protected GridContainer LevelSelection;
	public override void _Ready()
	{
		LevelSelection = GetNode<GridContainer>("UI/Level Selection");
	}
}
