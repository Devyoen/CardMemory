using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerState
{
    canAct,
    canNotAct
}

public class InGameManager : MonoSington<InGameManager>
{
    #region Values
    [Header ("GameSetting")]
    [SerializeField] private Vector2Int stageSize = new Vector2Int();

    public Vector2Int StageSize => stageSize;

    [Space (10f)]
    [SerializeField] private float cardIntervalX = 1f;
    [SerializeField] private float cardIntervalY = 1f;

    public float CardIntervalX => cardIntervalX;
    public float CardIntervalY => cardIntervalY;

    [Header("Info")]
    [SerializeField] private int score = 0;
    [SerializeField] private float timer = 0;

    public int Score => score;
    public float Timer => timer;

    //상태 변수들
    [Header ("StateValues")]
    private GameStateMachine stateMachine;
    private PlayerState playerState;

    public GameStateMachine StateMachine => stateMachine;
    public PlayerState PlayerState => playerState;

    #endregion

    #region SetValueMethods
    public void SetPlayerState(PlayerState state)
    {
        playerState = state;
    }

    public void AddScore(int value)
    {
        score += value;
    }
    #endregion

    private void Start()
    {
        stateMachine = new GameStateMachine();
        StateMachine.ChangeState(GameState.StartState);
    }

    public bool CanSelectCard() => InGameManager.instance.PlayerState == PlayerState.canAct;
}

public class StartState : IState
{
    private Vector3 cardsMixedPos;
    private List<CardType> curCardsType = new List<CardType>();
    private InGameManager inGameManager;
    private CardManager cardManager;

    public void Enter()
    {
        Init();
        inGameManager.SetPlayerState(PlayerState.canNotAct);
        inGameManager.StartCoroutine(SetGame_co());
        Debug.Log("StartState");
    }

    public void Update()
    {

    }

    public void Exit()
    {
        
    }

    private void Init()
    {
        inGameManager = InGameManager.instance;
        cardManager = CardManager.instance;
    }

    private IEnumerator SetGame_co()
    {
        cardManager.SetCards();
        yield return YieldCache.WaitForSeconds(2f);
        cardManager.FlipAllCards(CardDirection.Back);
        yield return YieldCache.WaitForSeconds(1f);
        cardManager.MoveAllCards(cardsMixedPos);
        cardManager.ShuffleCards();
        cardManager.PositionCards();
        yield return YieldCache.WaitForSeconds(2f);
        yield return cardManager.MoveAllCardsToOrigin_co();
        yield return YieldCache.WaitForSeconds(0.5f);
        EndStartState();
    }

    private void EndStartState()
    {
        inGameManager.StateMachine.ChangeState(GameState.SelectCardState);
    }

}

public class SelectCardState : IState
{
    private InGameManager inGameManager;
    private CardManager cardManager;

    public void Enter()
    {
        Init();
        inGameManager.SetPlayerState(PlayerState.canAct);
        cardManager.ResetSelectedCards();
        cardManager.onAllSelected += OnAllCardsSelected;
        Debug.Log("SelectState");
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }

    private void Init()
    {
        inGameManager = InGameManager.instance;
        cardManager = CardManager.instance;
    }

    private void OnAllCardsSelected()
    {
        inGameManager.StateMachine.ChangeState(GameState.JudgmentState);
    }
}

public class JudgmentState : IState
{
    private InGameManager inGameManager;
    private CardManager cardManager;

    public void Enter()
    {
        Init();
        inGameManager.SetPlayerState(PlayerState.canNotAct);
        inGameManager.StartCoroutine(CheakCardMatching_co());
        Debug.Log("JudgmentState");
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }

    private void Init()
    {
        inGameManager = InGameManager.instance;
        cardManager = CardManager.instance;
    }

    private IEnumerator CheakCardMatching_co()
    {
        yield return YieldCache.WaitForSeconds(0.5f);
        Debug.Log(cardManager.SelectedCards.Length);
        if (cardManager.SelectedCards[0].CardType == cardManager.SelectedCards[1].CardType)
        {
            cardManager.SucceededFlip();
            inGameManager.AddScore(100);
        }
        else
        {
            cardManager.FailedFlip();
        }
        yield return YieldCache.WaitForSeconds(0.5f);
        EndJudgmentState();
    }

    private void EndJudgmentState()
    {
        inGameManager.StateMachine.ChangeState(GameState.SelectCardState);
    }
}

public class EndState : IState
{
    public void Enter()
    {
        Debug.Log("EndState");
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}
