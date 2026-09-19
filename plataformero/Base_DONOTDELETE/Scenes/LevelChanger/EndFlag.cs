using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using Godot;
using System;
using System.Diagnostics;

namespace EjJorge.Base_DONOTDELETE.Scenes.LevelChanger;

public partial class EndFlag : Node2D
{
    protected Area2D Hitbox;

    [Export]
    public string UID;

    public override void _Ready()
    {
        Hitbox = GetNode<Area2D>("Hitbox");
        Hitbox.BodyEntered += ChangeLevel;
    }

    private void ChangeLevel(Node2D body)
    {
        Error error = GetTree().ChangeSceneToFile(UID);

        if (error is not Error.Ok)
        {
            Debug.Print($"error cambiando a {UID}, {error}");
            if (body is Player player) player.GoToMainMenu();
        }
        //else Debug.Print($"Se cambió de escena exitosamente, nueva escena: {UID}");
    }
}
