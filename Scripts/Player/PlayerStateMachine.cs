using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }//可公开获取，私有修改，只读
    public void Initialize(PlayerState _startstate)
    {
        currentState = _startstate;
        currentState.Enter(); 

    }
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();

    }
}
