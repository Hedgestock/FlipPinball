using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;

public partial class Mission : Node
{
    [Signal]
    public delegate void CompletedEventHandler();

    [Export]
    public string MissionName;
    [Export]
    public string StatusCompleted;
    [Export]
    private Array<Array<NodePath>> Goals;

    int CurrentStep = 0;

    public MissionGoal[] CurrentGoals
    {
        get
        {
            return Goals[CurrentStep].Select(g => GetNode<MissionGoal>(g)).ToArray();
        }
    }

    public System.Collections.Generic.List<MissionGoal> AllGoals
    {
        get
        {
            System.Collections.Generic.List<MissionGoal> result = new();
            foreach (var step in Goals)
            {
                result.AddRange(step.Select(g => GetNode<MissionGoal>(g)));
            }
            return result;
        }
    }

    public void Init()
    {
        CurrentStep = 0;
        InitGoals();
    }

    private void InitGoals()
    {
        foreach (MissionGoal goal in CurrentGoals)
        {
            goal.Init();
            goal.Connect(MissionGoal.SignalName.Updated, Callable.From(GoalUpdated));
            goal.Connect(MissionGoal.SignalName.Completed, Callable.From(GoalCompleted));
        }
        GoalUpdated();
    }

    public void GoalUpdated()
    {
        string status = string.Join("\n", CurrentGoals.Select(g => g.Status));
        StatusManager.Instance.EmitSignal(StatusManager.SignalName.MissionStatusChanged, status);
    }

    public void GoalCompleted()
    {
        if (CurrentGoals.All(g => g.IsComplete))
        {
            foreach (MissionGoal goal in CurrentGoals)
            {
                goal.Disconnect(MissionGoal.SignalName.Updated, Callable.From(GoalUpdated));
                goal.Disconnect(MissionGoal.SignalName.Completed, Callable.From(GoalCompleted));
            }
            if (CurrentStep + 1 >= Goals.Count)
            {
                StatusManager.Instance.EmitSignal(StatusManager.SignalName.MissionStatusChanged, StatusCompleted);
                EmitSignalCompleted();
            }
            else
            {
                CurrentStep++;
                InitGoals();
            }
        }
    }
}
