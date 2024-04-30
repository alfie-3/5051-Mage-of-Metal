using UnityEngine;

public class ShaderCamera : MonoBehaviour
{
    Transform thisCamera;

    private void Awake()
    {
        thisCamera = GetComponent<Transform>();
    }

    private void Update()
    {
        thisCamera.localPosition = LevelManager.shaderOverlayCamera.transform.localPosition;
        thisCamera.localEulerAngles = LevelManager.shaderOverlayCamera.transform.localEulerAngles;
    }
}
