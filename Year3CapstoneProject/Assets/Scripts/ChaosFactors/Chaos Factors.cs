using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ChaosFactor
{
    public float Timer { get; }

    public string Name { get; }

    public void OnEndOfChaosFactor(bool earlyEnd);
    
}
