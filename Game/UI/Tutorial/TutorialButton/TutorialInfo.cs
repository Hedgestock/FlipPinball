using Godot;
using System;

public partial class TutorialInfo : Control
{
    [Export]
    public string Title;

    [Export(PropertyHint.MultilineText)]
    public string Description;

    public void OnPressed()
    {
        if (GetParent() is TutorialInfo parent)
        {
            parent.OnPressed();
        }
        else
        {
            var popup = GD.Load<PackedScene>("res://Game/UI/Tutorial/TutorialButton/TutorialPopup.tscn").Instantiate<Control>();
            ((Label)popup.FindChild("Title")).Text = Title;
            ((Label)popup.FindChild("Content")).Text = Description;
            GetParent().AddChild(popup);
        }
    }
}
