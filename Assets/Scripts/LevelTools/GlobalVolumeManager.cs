//Manager script that deals with volume settings

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolumeManager : MonoBehaviour
{
    public static GlobalVolumeManager Instance {  get; private set; }

    [Header("Visual effects variables")]
    [SerializeField] Volume _volume;
    [SerializeField] AnimationCurve damageAnim;
    UnityEngine.Rendering.Universal.Vignette _vignette;

    private void Awake() { 
        Instance = this;
        _volume.profile.TryGet(out _vignette);
        _vignette.intensity.value = 0;
    }

    //Screen effect for players getting wrong runes or taking damage
    public IEnumerator PlayerVignetteEffect(float screenTime, Color vignCol)
    {
        float time = 0;
        _vignette.color.value = vignCol;

        while (time < screenTime)
        {
            time += (Time.unscaledDeltaTime);
            _vignette.intensity.value = damageAnim.Evaluate(time / screenTime);
            yield return null;
        }
        _vignette.intensity.value = damageAnim.Evaluate(0);
        yield return null;
    }
}
