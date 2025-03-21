using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;

public partial class BallterationViewer : Control
{
    [Export]
    Label NameLabel;
    [Export]
    VBoxContainer DescriptionContainer;

    Ballteration _ballteration;
    public Ballteration Ballteration
    {
        set
        {
            _ballteration = value;
            NameLabel.Text = _ballteration.DisplayName;

            foreach (var effect in value.GetChildren().OfType<Effect>())
            {
                Label effectDescription = new();
                effectDescription.Text = effect.Description;
                effectDescription.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                DescriptionContainer.AddChild(effectDescription);
            }
        }
    }
}
