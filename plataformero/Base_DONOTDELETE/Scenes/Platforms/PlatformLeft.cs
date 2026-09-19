using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.Platforms;

public partial class PlatformLeft : _PlatformBase
{
    protected override void _ReadyExtras() => Main.PlatformLefts.Add(this);
}
