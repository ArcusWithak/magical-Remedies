using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : ObjectInfoMain
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (nessasryComponent)
        {
            base.Update();
        }
    }
}
