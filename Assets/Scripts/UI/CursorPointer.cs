using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

public class CursorPointer : MonoBehaviour
{
    public static CursorPointer Instance {  get; private set; }

    [SerializeField] RectTransform ir_pointer;
    [SerializeField] RuneTestPlayer _runeTestPlayer;

    Controls _controlsKnm;
    InputAction controls;

    Image image;
    [Space]
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask enemyLayerMask;
    [Space]
    [SerializeField] Color idleColour;
    [SerializeField] Color activeColour;

    //Shoot area adjustments
    [Space]
    [Header("Aiming options")]
    [SerializeField] int checkXArea = 15;
    [SerializeField] int checkYArea = 15;
    [SerializeField] int intervalXArea = 5;
    [SerializeField] int intervalYArea = 5;
    [SerializeField] GameObject temp;
    private void Awake()
    {
        Instance = this;
        image = ir_pointer.GetComponent<Image>();
        _controlsKnm = new Controls();
    }

    private void OnEnable()
    {
        WiiInputManager.GuitarWiiMote.Strummed += Attack;

        controls = _controlsKnm.GuitarControls.Strum;
        _controlsKnm.GuitarControls.Strum.performed += Attack;
        _controlsKnm.GuitarControls.Strum.Enable();

    }
    private void OnDisable()
    {
        WiiInputManager.GuitarWiiMote.Strummed -= Attack;
        _controlsKnm.GuitarControls.Strum.performed -= Attack;
        _controlsKnm.GuitarControls.Strum.Disable();
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

    private void Attack(InputAction.CallbackContext obj)
    {
        Attack();
    }

    //Basic attack, to have rune breaking stuff added to
    private void Attack()
    {
        //WiimoteApi.GuitarData guitardata = WiiInputManager.GuitarWiiMote.WiiMote.Guitar;
        if (_runeTestPlayer != null)
        {
            //_runeTestPlayer.Strummed();
        }

        Ray ray = Camera.main.ScreenPointToRay(ir_pointer.position / PixelatedCamera.main.screenScaleFactor);
        RaycastHit hitInfo = new();

        //Debug.DrawRay(ray.origin, ray.direction * 50);

        if (LevelManager.isPaused)
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = LevelManager.pointer.GetComponent<RectTransform>().position;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent(out Button button))
                {
                    button.onClick.Invoke();
                }
            }
        }
        if (Physics.Raycast(ray, out hitInfo, 30, layerMask))
        {
            if (hitInfo.transform.TryGetComponent(out UnityEngine.UI.Button _button))
            {
                _button.onClick.Invoke();

                return;
            }
        }
    }
    public void Shoot(int power)
    {
        Debug.Log("SHOOT");
        RaycastHit hitInfo = new();
        Ray ray = Camera.main.ScreenPointToRay(ir_pointer.position / PixelatedCamera.main.screenScaleFactor);
        if (Physics.Raycast(ray, out hitInfo, 30, enemyLayerMask))
        {
            if (hitInfo.transform.TryGetComponent(out IDamage damage))
            {
                Debug.Log("Yes");
                damage.Damage(power * 5);
            }
        }
    }
    
}
