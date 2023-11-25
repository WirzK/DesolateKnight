using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine 
{
    public EnemyState currentState { get; private set; }
    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();

    }
    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();//退出旧的
        currentState = _newState;
        currentState.Enter();//进入新的
    }
}
