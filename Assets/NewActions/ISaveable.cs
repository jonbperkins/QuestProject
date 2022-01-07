using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public void ToJson();
    public void FromJson();
}
