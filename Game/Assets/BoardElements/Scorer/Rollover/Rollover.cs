using Godot;
using System;

public partial class Rollover : Scorer
{
    private void OnAreaBodyEnter(Node2D body)
    {
        if (body is Ball) Score();
    }
}
