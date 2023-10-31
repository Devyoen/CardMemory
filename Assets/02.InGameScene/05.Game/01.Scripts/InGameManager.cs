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
    [Header("Common")]
    [SerializeField] private int successCount;
    [SerializeField] private DifficultyDataContainer difficultyDataContainer;

    public static int difficulty = 0;

    private int stageNum = 1;

    public int StageNum => stageNum;
    public int SuccessCount => successCount;

    [Header ("GameSetting")]
    [SerializeField] private Vector2Int stageSize = new Vector2Int();
    public bool isReplay = false;

    public Vector2Int StageSize => stageSize;

    [Space (10f)]
    [SerializeField] private float cardIntervalX = 1f;
    [SerializeField] private float cardIntervalY = 1f;

    public float CardIntervalX => cardIntervalX;
    public float CardIntervalY => cardIntervalY;

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
    #endregion

    private void Start()
    {
        stateMachine = new GameStateMachine();
        StateMachine.ChangeState(GameState.GameStartState);
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public void OnGameOver()
    {
        StateMachine.ChangeState(GameState.StageEndState);
    }

    public void AddSuccessCount()
    {
        successCount++;
    }

    public void InitSuccessCount()
    {
        successCount = 0;
    }

    public void AddStageNum()
    {
        stageNum++;
    }

    public void StageSetting()
    {
        switch (StageNum)
        {
            case 1:
                stageSize = new Vector2Int(2, 2);
                break;
            case 2:
                stageSize = new Vector2Int(2, 4);
                break;
            case 3:
                stageSize = new Vector2Int(3, 6);
                break;
        }
    }

    public DifficultyData GetDifficultyData()
    {
        return difficultyDataContainer.List[difficulty];
    }

    public bool IsClear() => (successCount >= CardManager.instance.Cards.Length / 2);

    public bool CanSelectCard() => InGameManager.instance.PlayerState == PlayerState.canAct;
}

public class GameStartState : IState
{
    public void Enter()
    {
        Debug.Log("GameStartState");
        GameInit();
        UIInit();
        InGameManager.instance.StateMachine.ChangeState(GameState.StageStartState);
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }

    private void GameInit()
    {
        Timer.instance.onTimeOver += InGameManager.instance.OnGameOver;
        Timer.instance.TimerInit();
    }

    private void UIInit()
    {
        ScoreManager.instance.ScoreInit();
        InGameUIs.instance.UpdateUI(UIType.Timer, Timer.instance.LeftTime);
        InGameUIs.instance.UpdateUI(UIType.Score, ScoreManager.instance.Score);
    }

}

public class StageStartState : IState
{
    private Vector3 cardsMixedPos;
    private List<CardType> curCardsType = new List<CardType>();
    private InGameManager inGameManager;
    private CardManager cardManager;

    public void Enter()
    {
        Init();
        inGameManager.StageSetting();
        inGameManager.SetPlayerState(PlayerState.canNotAct);
        inGameManager.StartCoroutine(SetGame_co());
        Debug.Log("StageStartState");
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
        inGameManager.InitSuccessCount();
        cardManager.Init();
    }

    private IEnumerator SetGame_co()
    {
        yield return YieldCache.WaitForSeconds(1f);
        ScreenEffect.instance.SlidingDownText($"스테이지 {inGameManager.StageNum} 준비");
        cardManager.SetCards();
        yield return YieldCache.WaitForSeconds(2f);
        cardManager.FlipAllCards(CardDirection.Back);
        yield return YieldCache.WaitForSeconds(1f);
        cardManager.MoveAllCards(cardsMixedPos);
        cardManager.ShuffleCards();
        cardManager.PositionCards();
        yield return YieldCache.WaitForSeconds(1f);
        yield return cardManager.MoveAllCardsToOrigin_co();
        ScreenEffect.instance.SlidingDownText($"게임 시작!");
        yield return YieldCache.WaitForSeconds(1f);
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
        cardManager.onAllSelected -= OnAllCardsSelected;
        cardManager.onAllSelected += OnAllCardsSelected;
        Debug.Log("SelectState");
    }

    public void Update()
    {
        Timer.instance.TimerUpdate();
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
        Timer.instance.TimerUpdate();
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
            ScoreManager.instance.AddScore();
            inGameManager.AddSuccessCount();
            if (inGameManager.IsClear())
            {
                inGameManager.OnGameOver();
                yield break;
            }
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

public class StageEndState : IState
{
    public void Enter()
    {
        Debug.Log("StageEndState");
        if (InGameManager.instance.IsClear())
        {
            InGameManager.instance.StartCoroutine(GameClear_co());
        }
        else
        {
            InGameManager.instance.StartCoroutine(GameOver_co());
        }
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }

    private IEnumerator GameClear_co()
    {
        
        yield return YieldCache.WaitForSeconds(1f);
        SoundManager.instance.Play(AudioClipName.GameOver);
        ScreenEffect.instance.SlidingDownText($"스테이지 {InGameManager.instance.StageNum} 클리어!");
        yield return YieldCache.WaitForSeconds(1.5f);
        switch (InGameManager.instance.StageNum)
        {
            case 1:
                InGameManager.instance.AddStageNum();
                InGameManager.instance.StateMachine.ChangeState(GameState.StageStartState);
                break;
            case 2:
                InGameManager.instance.AddStageNum();
                InGameManager.instance.StateMachine.ChangeState(GameState.StageStartState);
                break;
            case 3:
                InGameManager.instance.StateMachine.ChangeState(GameState.GameEndState);
                break;
            default:
                Debug.Log("오류 : 최대 스테이지를 벗어남");
                break;
        }
    }

    private IEnumerator GameOver_co()
    {

        yield return YieldCache.WaitForSeconds(1f);
        SoundManager.instance.Play(AudioClipName.GameOver);
        ScreenEffect.instance.SlidingDownText($"게임 오버!");
        yield return YieldCache.WaitForSeconds(1.5f);
        InGameManager.instance.StateMachine.ChangeState(GameState.GameEndState);
    }
}

public class GameEndState : IState
{
    public void Enter()
    {
        Debug.Log("GameEndState");
        GameResult.instance.ShowGameResult(ScoreManager.instance.Score);
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}
