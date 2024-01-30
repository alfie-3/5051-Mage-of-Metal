using UnityEngine;
using UnityEngine.UI;

public class CursorPointer : MonoBehaviour
{
    [SerializeField] RectTransform ir_pointer;
    [SerializeField] NoteController _noteController;

    Image image;
    [Space]
    [SerializeField] LayerMask layerMask;
    [Space]
    [SerializeField] Color idleColour;
    [SerializeField] Color activeColour;

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

    //Changes cursor colour to see if the raycasting works and add some visual feedback
    private void UpdateCursorColor()
    {
        IDamage enemy = CheckForEnemy();

        if (enemy != null)
            image.color = activeColour;

        else
            image.color = idleColour;
    }

    private void UpdateCursorPos()
    {
        //If the wii remote can't be found or the IR bar cant be found it defaults to mouse mode.
        //Sets the position of the cursor to where the wii remote is pointing.
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

        //Hacky but the canvas used for the IR cursor is bigger than the screen and 4:3 because that works better for pointing
        //But when the mouse takes over it doesnt map properly so I have to change it back to the root canvas
        ir_pointer.transform.SetParent(transform);

        Vector2 mappedCusor = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

        ir_pointer.anchorMin = mappedCusor;
        ir_pointer.anchorMax = mappedCusor;

        ir_pointer.anchoredPosition = Vector2.zero;
    }

    //Checks for enemy by raycasting below the cursor to the world.
    public IDamage CheckForEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(ir_pointer.position / PixelatedCamera.main.screenScaleFactor);
        RaycastHit hitInfo = new();

        Debug.DrawRay(ray.origin, ray.direction * 50);

        if (Physics.Raycast(ray, out hitInfo, 30, layerMask))
        {
            if (hitInfo.transform.TryGetComponent(out IDamage damageable))
            {
                return damageable;
            }
        }

        return null;
    }

    //Basic attack, to have rune breaking stuff added to
    private void Attack()
    {
        WiimoteApi.GuitarData guitardata = WiiInputManager.GuitarWiiMote.WiiMote.Guitar;
        if (!_noteController.CheckGuitarNotes(guitardata.green_fret, guitardata.red_fret, guitardata.yellow_fret, guitardata.blue_fret, guitardata.orange_fret)) return;

        Debug.Log("SUCCESSFUL ATTACK");
        IDamage damageable = CheckForEnemy();

        if (damageable != null)
        {
            Debug.Log("Attack");
            damageable.Damage(3);
        }
        else
        {
            Debug.Log("Miss");
        }
    }
}
