using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;

public partial class Mission : Node
{
    [Signal]
    public delegate void CompletedEventHandler();

    [Export]
    int CompletionValue = 10000;

    [Export]
    private Array<Array<NodePath>> Goals;

    public string MissionName;
    public string StatusCompleted;

    int CurrentStep = 0;

    public override void _Ready()
    {
        base._Ready();
        string tmp = Regex.Replace(Name + GameManager.CurrentBoard.Name, "(?<!^)([A-Z])", "_$1").ToUpperInvariant();
        MissionName = "MISSION_" + tmp;
        StatusCompleted = "MISSION_COMPLETED_" + tmp;
    }

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
                ScoreManager.BoardScore(CompletionValue);
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
