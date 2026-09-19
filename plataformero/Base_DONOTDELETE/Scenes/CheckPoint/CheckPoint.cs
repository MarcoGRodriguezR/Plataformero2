using Godot;
using System;
using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;

namespace EjJorge.Base_DONOTDELETE.Scenes.CheckPoint;

public partial class CheckPoint : Node2D
{
    protected Main Main;
    protected AnimatedSprite2D Sprite;

    protected Area2D Hitbox;

    protected Vector2 RespawnPosition;

    public override void _Ready()
    {
        Main = FindParent("Main") as Main;
        Main.CheckPoints.Add(this);

        Hitbox = GetNode<Area2D>("Hitbox");
        Hitbox.BodyEntered += NewCheckPoint;

        Sprite = GetNode<AnimatedSprite2D>("Sprite");

        RespawnPosition = new(Position.X, Position.Y - 2);
    }

    public void UpdateGraphic()
    {
        bool chosen = RespawnPosition == Main.Player.RespawnSpot;

        if (chosen) Sprite.Play("Touched");
        else Sprite.Play("Untouched");
    }

    private void NewCheckPoint(Node2D body)
    {
        if (body is Player)
        {
            Main.Player.RespawnSpot = RespawnPosition;
            Main.CheckCheckPoints();
        }
    }
}
