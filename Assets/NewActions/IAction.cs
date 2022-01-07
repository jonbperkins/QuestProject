using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ActionState { Preparing, Ready, Running, Finishing, Stopped };
public interface IAction
{
    public ActionState CurrentState { get; set; }
    public float StartDelay { get; set; }
    public List<IAction> NextActions {get;}
    public int ID { get; set; }
    public void Execute();
    public void Reverse();
    public void Undo();
    public void RegisterNextState(IAction nextState, int nextStateID);
    public void CallNextState();
    public void Destroy();

    public void Pause();
    public void UnPause();

}
