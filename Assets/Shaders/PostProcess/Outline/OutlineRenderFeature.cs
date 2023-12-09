using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderFeature : ScriptableRendererFeature
{
    private OutlinePass outlinePass;

    public override void Create()
    {
        outlinePass = new OutlinePass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(outlinePass);
    }

    class OutlinePass : ScriptableRenderPass
    {
        private Material _mat;
        int outlineId = Shader.PropertyToID("_TempID");
        RenderTargetIdentifier src, outline;

        public OutlinePass()
        {
            if (!_mat)
            {
                _mat = CoreUtils.CreateEngineMaterial("PostProcessing/SobelOutline");
            }

            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            src = renderingData.cameraData.renderer.cameraColorTargetHandle;
            cmd.GetTemporaryRT(outlineId, desc, FilterMode.Bilinear);
            outline = new RenderTargetIdentifier();
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("OutlineRenderFeature");
            VolumeStack volumes = VolumeManager.instance.stack;
            CustomPostOutline cpo = volumes.GetComponent<CustomPostOutline>();

            if (cpo.IsActive())
            {
                _mat.SetFloat("_OutlineThickness", (float)cpo.outlineThickness);
                _mat.SetFloat("_OutlineDepthMultiplier", (float)cpo.outlineDepthMultiplier);
                _mat.SetFloat("_OutlineDepthBias", (float)cpo.outlineDepthBias);
                _mat.SetFloat("_OutlineNormalBias", (float)cpo.outlineNormalBias);

                _mat.SetColor("_OutlineColor", (Color)cpo.outlineColor);

                Blit(commandBuffer, src, outline, _mat, 0);
                Blit(commandBuffer, outline, src);
            }

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);   
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(outlineId);
        }
    }
}
