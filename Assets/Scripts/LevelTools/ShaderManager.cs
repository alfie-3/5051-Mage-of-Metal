//The shader manager deals with cool effects shared between multiple objects

using System.Collections;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    public static ShaderManager Instance { get; private set; }

    [Header("Dissolve Material values")]
    [SerializeField] Shader dissolveShader;
    string dissolveMatName = "_Alpha";
    string mainMaterialTextureName = "_BaseTexture";

    private void Awake() { Instance = this; }

    public IEnumerator DissolveObject(GameObject dissolveObj, Texture2D mainMaterialTexture, float enemyDeathTime, bool isPooled, GameObject sourceObj = null)
    {
        if (sourceObj == null) { sourceObj = dissolveObj; }

        //Create and set new materials
        Material mat = new Material(dissolveShader);
        Renderer rend = dissolveObj.GetComponent<Renderer>();
        rend.material = mat;
        rend.material.SetTexture(mainMaterialTextureName, mainMaterialTexture);
        rend.material.SetFloat(dissolveMatName, 1);
        rend.material.SetColor("_Color", new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255)));

        //Dissolve Effect over time
        float delta = enemyDeathTime;
        while (delta > 0)
        {
            delta -= Time.deltaTime;
            rend.material.SetFloat(dissolveMatName, delta / enemyDeathTime);
            yield return null;
        }

        //Deactivate object
        if (isPooled) { sourceObj.SetActive(false); }
        else { Destroy(sourceObj); }
    }
}