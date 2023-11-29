using UnityEngine;
using UnityEngine.UI;

public class CursorPointer : MonoBehaviour
{
    [SerializeField] RectTransform ir_pointer;

    Image image;

    private void Awake()
    {
        image = ir_pointer.GetComponent<Image>();
    }

    private void OnEnable()
    {
        WiiInputManager.GuitarWiiMote.Strummed += Attack;
    }

    private void OnDisable()
    {
        WiiInputManager.GuitarWiiMote.Strummed -= Attack;
    }

    private void Update()
    {
        UpdateCursorPos();
        UpdateCursorColor();
    }

    private void UpdateCursorColor()
    {
        IDamage enemy = CheckForEnemy();

        if (enemy != null)
            image.color = Color.green;

        else
            image.color = Color.red;
    }

    private void UpdateCursorPos()
    {
        if (WiiInputManager.CursorWiiMote.HasRemote)
        {
            ir_pointer.transform.SetParent(transform.GetChild(0));


            Vector2 pointerPos = WiiInputManager.CursorWiiMote.IRPointScreenPos();

            if (pointerPos.x != -1 && pointerPos.y != -1)
            {
                ir_pointer.anchorMin = pointerPos;
                ir_pointer.anchorMax = pointerPos;

                ir_pointer.anchoredPosition = Vector2.zero;
                return;
            }
        }

        ir_pointer.transform.SetParent(transform);

        Vector2 mappedCusor = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

        ir_pointer.anchorMin = mappedCusor;
        ir_pointer.anchorMax = mappedCusor;

        ir_pointer.anchoredPosition = Vector2.zero;
    }

    public IDamage CheckForEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(ir_pointer.position);
        RaycastHit hitInfo = new();

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.TryGetComponent(out IDamage damageable))
            {
                return damageable;
            }
        }

        return null;
    }

    private void Attack()
    {
        IDamage damageable = CheckForEnemy();

        if (damageable != null)
        {
            Debug.Log("Attack");
            damageable.Damage(1);
        }
        else
        {
            Debug.Log("Miss");
        }
    }
}
