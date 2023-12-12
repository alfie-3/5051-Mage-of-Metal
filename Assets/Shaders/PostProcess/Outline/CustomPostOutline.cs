using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/Outline", typeof(UniversalRenderPipeline))]
public class CustomPostOutline : VolumeComponent, IPostProcessComponent
{
    public FloatParameter outlineThickness = new FloatParameter(0);
    public FloatParameter outlineDepthMultiplier = new FloatParameter(3000);
    public FloatParameter outlineDepthBias = new FloatParameter(0.9f);
    public FloatParameter outlineNormalMultiplier = new FloatParameter(1.77f);
    public FloatParameter outlineNormalBias = new FloatParameter(2.3f);

    public ColorParameter outlineColor = new ColorParameter(Color.black);

    public bool IsActive() => true;

    public bool IsTileCompatible() => true;
}
