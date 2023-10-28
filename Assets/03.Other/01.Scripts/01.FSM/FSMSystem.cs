using System.Collections.Generic;
using UnityEngine;

public class FSMSystem : MonoBehaviour
{

    public enum FSMState
    {
        Start,
        InProgress,
        End
    }

    public FSMState currentState;
    protected Dictionary<FSMState, FSMStateBehavior> behaviors = new Dictionary<FSMState, FSMStateBehavior>();


    private void Update()
    {
        behaviors[currentState].Update(gameObject);
    }

    public void ChangeState(FSMState state)
    {
        currentState = state;
    }
}

public abstract class FSMStateBehavior
{
    public abstract void Update(GameObject gameObject);
}

