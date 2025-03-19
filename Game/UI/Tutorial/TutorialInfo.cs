using Godot;
using System;

public partial class TutorialInfo : Control
{
    [Export]
    public string Title;

    [Export(PropertyHint.MultilineText)]
    public string Description;

    public override void _Ready()
    {
        base._Ready();
        ProcessMode = ProcessModeEnum.Always;
    }

    public void OnPressed()
    {
        if (GetParent() is TutorialInfo parent)
        {
            parent.OnPressed();
        }
        else
        {
            var popup = GD.Load<PackedScene>("uid://desalmyy6b5on").Instantiate<Control>();
            ((Label)popup.FindChild("Title")).Text = Title;
            ((Label)popup.FindChild("Content")).Text = Description;
            GetParent().AddChild(popup);
        }
    }
}
