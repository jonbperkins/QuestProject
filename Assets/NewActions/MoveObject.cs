using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour, IAction, ISaveable
{
    public ActionState CurrentState { get; set; }
    public float StartDelay { get; set; }
    public int ID { get; set; }

    public List<IAction> NextActions => new List<IAction>();

    public void CallNextState()
    {
        foreach(IAction action in NextActions)
        {
            action.Execute();
        }
    }

    public void Destroy()
    {
    }

    public void Execute()
    {
    }

    public void FromJson()
    {
    }

    public void Pause()
    {
    }

    public void RegisterNextState(IAction nextAction, int nextObjectID)
    {
 //       NextActions.Add(nextState, nextObjectID);
    }

    public void ToJson()
    {
    }

    public void Undo()
    {
    }

    public void UnPause()
    {
    }

    void IAction.Reverse()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
