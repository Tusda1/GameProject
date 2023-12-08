using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDatabase
{
    private Dictionary<string, Delegate> _database = new Dictionary<string, Delegate>();

    public bool HasCommand(string commandName) => _database.ContainsKey(commandName);

    public void AddCommand(string commandName, Delegate command)
    {
        if (!_database.ContainsKey(commandName))
        {
            _database.Add(commandName, command);
        }
        else
            Debug.LogError($"Command already exists in the database '{commandName}'");
    }

    public Delegate GetCommand(string commandName)
    {
        if (!_database.ContainsKey(commandName))
        {
            Debug.LogError($"Command '{commandName}' does not exist");
            return null;
        }

        return _database[commandName];

    }
}
