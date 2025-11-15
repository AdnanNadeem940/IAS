using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopState : IState<Bee>
{
    public void OnEnter(Bee b)
    {
        
    }

    public void OnExecute(Bee b)
    {
        if (!LevelManagerBeeGame.Ins.timesUp)
        {
            b.TransitionToState(b.moveState);
        }    
        b.rb.linearVelocity = Vector3.zero;
    }

    public void OnExit(Bee b)
    {
        
    }

}
