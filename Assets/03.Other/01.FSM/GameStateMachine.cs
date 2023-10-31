using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GameStartState,
    StageStartState,
    SelectCardState,
    JudgmentState,
    StageEndState,
    GameEndState
}

public class GameStateMachine
{
    private IState currentState;
    private GameState currentStateType;
    private Dictionary<GameState, IState> states = new Dictionary<GameState, IState>();

    public void ChangeState(GameState state)
    {
        currentState?.Exit();
        currentStateType = state;
        currentState = GetState(state);
        currentState.Enter();
    }

    private IState GetState(GameState key)
    {
        if (!states.ContainsKey(key))
        {
            Debug.Log("create");
            IState _state = null;
            switch (key)
            {
                case GameState.GameStartState:
                    _state = new GameStartState();
                    break;
                case GameState.StageStartState:
                    _state = new StageStartState();
                    break;
                case GameState.SelectCardState:
                    _state = new SelectCardState();
                    break;
                case GameState.JudgmentState:
                    _state = new JudgmentState();
                    break;
                case GameState.StageEndState:
                    _state = new StageEndState();
                    break;
                case GameState.GameEndState:
                    _state = new GameEndState();
                    break;
                default:
                    Debug.Log("에러 : 존재하지 않는 IState 타입");
                    break;
            }
            states.Add(key, _state);
        }
        return states[key];
    }

    public void Update()
    {
        currentState?.Update();
    }
}
