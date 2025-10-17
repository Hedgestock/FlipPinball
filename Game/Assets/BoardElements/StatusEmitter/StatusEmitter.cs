using Godot;
using System;

public partial class StatusEmitter : Node
{
    [Export]
    string StatusMessage;

    private void Emit()
    {
        StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, StatusMessage);
    }

    private void Emit(string message)
    {
        StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, message);
    }
}
