using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.Speed;

public partial class Fast : Area2D
{
    public override void _Ready()
    {
        BodyEntered += Fast_BodyEntered;
    }

    private void Fast_BodyEntered(Node2D body)
    {
        if (body is Player player) player.SlowFall = false;
    }
}
