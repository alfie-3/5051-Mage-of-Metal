using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class TerrainDetailer : MonoBehaviour
{
    public Terrain terrain;

    [SerializeField] private TerrainDetail[] terrainDetails;
    void Start()
    {
        //GenerateDetail();
    }

    public void GenerateDetail()
    {
        print("Regenerating");

        int alphamapWidth = terrain.terrainData.alphamapWidth;
        int alphamapHeight = terrain.terrainData.alphamapHeight;
        int detailWidth = terrain.terrainData.detailResolution;
        int detailHeight = detailWidth;

        float resolutionDiffFactor = (float)alphamapWidth / detailWidth;

        float[,,] splatmap = terrain.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);

        foreach (TerrainDetail detail in terrainDetails)
        {
            if (!terrain)
            {
                Debug.Log("You have not selected a terrain object");
                return;
            }

            if (detail.detailIndex >= terrain.terrainData.detailPrototypes.Length)
            {
                Debug.Log("You have chosen a detail index which is higher than the number of detail prototypes in your detail libary. Indices starts at 0");
                return;
            }

            if (detail.affectingSplatmaps.Length > terrain.terrainData.terrainLayers.Length)
            {
                Debug.Log("You have selected more splat textures to paint on, than there are in your libary.");
                return;
            }

            for (int i = 0; i < detail.affectingSplatmaps.Length; i++)
            {
                if (detail.affectingSplatmaps[i] >= terrain.terrainData.terrainLayers.Length)
                {
                    Debug.Log("You have chosen a splat texture index which is higher than the number of splat prototypes in your splat libary. Indices starts at 0");
                    return;
                }
            }

            int[,] newDetailLayer = new int[detailWidth, detailHeight];

            //loop through splatTextures
            for (int i = 0; i < detail.affectingSplatmaps.Length; i++)
            {

                //find where the texture is present
                for (int j = 0; j < detailWidth; j++)
                {

                    for (int k = 0; k < detailHeight; k++)
                    {

                        float alphaValue = splatmap[(int)(resolutionDiffFactor * j), (int)(resolutionDiffFactor * k), detail.affectingSplatmaps[i]];

                        newDetailLayer[j, k] = (int)Mathf.Round(alphaValue * ((float)detail.detailCount)) + newDetailLayer[j, k];

                    }
                }
            }

            terrain.terrainData.SetDetailLayer(0, 0, detail.detailIndex, newDetailLayer);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TerrainDetailer))]
public class TerrainDetailerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainDetailer detailer = (TerrainDetailer)target;

        if (GUILayout.Button("Generate"))
        {
            detailer.GenerateDetail();
        }
    }
}
#endif

[System.Serializable]
public struct TerrainDetail
{
    public int[] affectingSplatmaps;
    [Space]
    public int detailIndex;
    public int detailCount;
}