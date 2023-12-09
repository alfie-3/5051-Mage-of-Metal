using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/Outline", typeof(UniversalRenderPipeline))]
public class CustomPostOutline : VolumeComponent, IPostProcessComponent
{
    public FloatParameter outlineThickness = new FloatParameter(1);
    public FloatParameter outlineDepthMultiplier = new FloatParameter(1);
    public FloatParameter outlineDepthBias = new FloatParameter(1);
    public FloatParameter outlineNormalBias = new FloatParameter(1);

    public ColorParameter outlineColor = new ColorParameter(Color.black);

    public bool IsActive() => true;

    public bool IsTileCompatible() => true;
}
