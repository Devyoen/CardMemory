using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    StartState,
    SelectCardState,
    JudgmentState,
    EndState
}

public class GameStateMachine
{
    private GameState currentState;
    private Dictionary<GameState, IState> states = new Dictionary<GameState, IState>();

    public void ChangeState(GameState state)
    {
        GetState(currentState)?.Exit();
        currentState = state;
        GetState(currentState).Enter();
    }

    private IState GetState(GameState key)
    {
        if (!states.ContainsKey(key))
        {
            IState _state = null;
            switch (key)
            {
                case GameState.StartState:
                    _state = new StartState();
                    break;
                case GameState.SelectCardState:
                    _state = new SelectCardState();
                    break;
                case GameState.JudgmentState:
                    _state = new JudgmentState();
                    break;
                case GameState.EndState:
                    _state = new EndState();
                    break;
                default:
                    Debug.Log("에러 : 할당되지 않은 IState");
                    break;
            }
            states.Add(key, _state);
        }
        return states[key];
    }

    public void Update()
    {
        GetState(currentState)?.Update();
    }
}
