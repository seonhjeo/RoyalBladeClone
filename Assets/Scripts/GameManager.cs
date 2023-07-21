
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum GameState
{
    OnStart,
    OnEnd
}

public class GameManager : MonoBehaviour
{
    #region Public Values
    
    public GameState State { get; private set; }

    #endregion

    #region Private Values

    [SerializeField] private TMP_Text announceText;
    
    [SerializeField] private UnityEvent eventOnStart;
    [SerializeField] private UnityEvent eventOnEnd;

    private PlayerInputAction _pInput;
    
    private int _destroyedBuildingNumber;

    #endregion


    #region Mono Methods

    private void Awake()
    {
        _pInput = new PlayerInputAction();
    }

    private void OnEnable()
    {
        _pInput.Manage.Enable();
        _pInput.Manage.QuitGame.started += _QuitGame;
    }

    private void OnDisable()
    {
        _pInput.Manage.QuitGame.started -= _QuitGame;
        _pInput.Manage.Disable();
    }

    private void Start()
    {
        State = GameState.OnEnd;
        GameStart();
    }

    #endregion
    

    #region Public Methods

    /// <summary>
    /// 부순 빌딩의 수를 하나씩 올리는 함수
    /// </summary>
    public void DestroyedBuildingNumberCountUp()
    {
        _destroyedBuildingNumber++;
    }
    
    /// <summary>
    /// 게임을 시작할 때 실행하는 함수, Start Game 버튼에 바인딩해 사용
    /// </summary>
    public void GameStart()
    {
        if (State == GameState.OnEnd)
        {
            _destroyedBuildingNumber = 0;
            eventOnStart.Invoke();
            State = GameState.OnStart;
        }
    }

    /// <summary>
    /// 게임이 끝났을 때 실행하는 함수, Player의 OnCollisionEnter2D함수에 바인딩해 사용
    /// </summary>
    public void GameEnd()
    {
        if (State == GameState.OnStart)
        {
            announceText.text = "You break\n" + _destroyedBuildingNumber + "\nBuildings";
            eventOnEnd.Invoke();
            State = GameState.OnEnd;
        }
    }

    #endregion


    #region Private Methods

    private void _QuitGame(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Started)
        {
            Application.Quit(0);
        }
    }

    #endregion
}
