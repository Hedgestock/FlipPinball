using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FontTest : VBoxContainer
{
    private string BasePath = "res://Game/Themes/Fonts/";

    public override void _Ready()
    {
        base._Ready();
        foreach (string path in GetFonts(BasePath))
        {
            Label label = new();
            label.Text = "the quick brown fox jumps over the lazy dog\nTHE QUICK BROWN FOX JUMPS OVER THE LAZY DOG";
            label.AddThemeFontOverride("font", GD.Load<FontFile>(path));
            AddChild(label);
        }
    }

    private List<string> GetFonts(string dirPath)
    {

        List<string> result = new();
        var dir = DirAccess.Open(dirPath);
        if (DirAccess.GetOpenError() == Error.Ok)
        {
            dir.ListDirBegin();

            var fileName = dir.GetNext();
            while (!string.IsNullOrWhiteSpace(fileName))
            {
                if (dir.CurrentIsDir())
                    result = result.Concat(GetFonts(dir.GetCurrentDir() + "/" + fileName)).ToList();
                else if (fileName.GetExtension() == "ttf")
                    result.Add(dir.GetCurrentDir() + "/" + fileName);
                fileName = dir.GetNext();
            }
        }
        return result;
    }
}
