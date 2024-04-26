//Base fireball behaviour

using UnityEngine.VFX;

public class Fireball : BaseSpell
{

    public override void OnHit()
    {
        base.OnHit();
        //Only difference to basespell script is the adaptation to VisualEffects component
        GetComponent<VisualEffect>().Stop();
    }
}
