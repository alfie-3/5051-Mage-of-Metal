using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Fireball : BaseSpell
{

    public override void OnHit()
    {
        base.OnHit();
        GetComponent<VisualEffect>().Stop();
    }
}
