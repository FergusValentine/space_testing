using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance { get; private set; }

    private Stack<string> _controllableStack;
    private Dictionary<string, List<IControllable>> _registeredControllables;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _controllableStack = new Stack<string>();
        _registeredControllables = new Dictionary<string, List<IControllable>>();
    }

    private void ToggleMap(string map, bool state)
    {
        List<IControllable> controllablesList = _registeredControllables[map];

        if(state)
            InputSystem.actions.FindActionMap(map).Enable();
        else
            InputSystem.actions.FindActionMap(map).Disable();

        foreach (IControllable contollable in controllablesList)
        {
            if (state)
                contollable.InputEnabled();
            else
                contollable.InputDisabled();
        }
    }

    public void SubscribeToControllable(IControllable controllable)
    {
        string map = controllable.actionMap.ToString();

        // Check if the map dictionary exists already
        if(!_registeredControllables.ContainsKey(map))
            _registeredControllables.Add(map, new List<IControllable>());

        // Add controllable to list
        if (_registeredControllables[map].Contains(controllable))
            return;
        else
            _registeredControllables[map].Add(controllable);
        
        if(_controllableStack.Count > 0)
        {
            string currentEnabled = _controllableStack.Peek();
            ToggleMap(currentEnabled, false);
        }

        ToggleMap(map, true);

        // Check if we already added the map to the stack
        if (_controllableStack.Contains(map))
            return;
        _controllableStack.Push(map);
    }

    public void UnsubscribeControllable(IControllable controllable)
    {
        string map = controllable.actionMap.ToString();

        if (!_registeredControllables.ContainsKey(map))
            return;

        ToggleMap(map, false);
        _registeredControllables[map].Remove(controllable);
        _controllableStack.Pop();

        if (_controllableStack.Count > 0)
        {
            string currentEnabled = _controllableStack.Peek();
            ToggleMap(currentEnabled, true);
        }
    }
}
