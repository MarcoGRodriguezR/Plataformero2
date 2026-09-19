using Godot;
using System;
using System.Diagnostics;

namespace EjJorge.Base_DONOTDELETE.Scenes.UI;

public partial class LevelChangeButton : Button
{
    [Export]
    public string UID;

    public override void _Ready()
    {
        Pressed += ChangeLevel;
    }

    private void ChangeLevel()
    {
        if (string.IsNullOrEmpty(UID)) throw new Exception($"{nameof(UID)} es nulo para {Name}");

        Error error = GetTree().ChangeSceneToFile(UID);

        if (error is not Error.Ok) Debug.Print($"error cambiando a {UID}, {error}");
    }
}
