using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScore
{
    public void AddScore(float scoreMult, int score);
    public void DamageScore(float scoreMult, Color vignCol, float vignTime);

}
