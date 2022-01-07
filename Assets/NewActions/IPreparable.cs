using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPreparable
{
    public float PreTime { get; set; }
    public void CalculateInitialPos();
}