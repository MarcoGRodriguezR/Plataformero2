using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.MovingBlocks;

public partial class _MovingBlock : AnimatableBody2D
{
    protected AnimationPlayer Player;

    public override void _Ready()
    {
        Player = GetNode<AnimationPlayer>("AnimationPlayer");
        Player.AnimationFinished += Player_AnimationFinished;
    }

    private void Player_AnimationFinished(StringName animName)
    {
        if (animName.Equals("RESET")) Player.Play("default");
    }
}
