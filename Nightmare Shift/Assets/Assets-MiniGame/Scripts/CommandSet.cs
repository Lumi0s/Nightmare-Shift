using System;
using System.Collections.Generic;
using UnityEngine;

public static class CommandSet
{
    static Dictionary<string, List<Action>> commands = new Dictionary<string, List<Action>>();

    public static void AddCommand(string commandCode, List<Action> actions)
    {
        if (!commands.ContainsKey(commandCode))
        {
            commands[commandCode] = new List<Action>();
        }
        foreach (var action in actions)
        {
            commands[commandCode].Add(action);
        }
    }

    public static void ExecuteCommand(string commandCode)
    {
        if (commands.TryGetValue(commandCode, out List<Action> actions))
        {
            foreach (var action in actions)
            {
                action.Invoke();
            }
        }
        else
        {
            Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
                (byte)(Color.red.r * 255f), (byte)(Color.red.g * 255f), (byte)(Color.red.b * 255f),
                $"Command: '{commandCode}' not found"));
        }
    }

    public static void RemoveCommand(string commandCode)
    {
        if (commands.ContainsKey(commandCode))
        {
            commands.Remove(commandCode);
        }
    }
}
