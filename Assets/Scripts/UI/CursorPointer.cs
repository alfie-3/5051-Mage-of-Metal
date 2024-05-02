//Controls the custom cursor for guitar and computer mouse
//This is an adapted version for the pixilated screen created off the camera footage of the player

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CursorPointer : MonoBehaviour
{
    public static CursorPointer Instance { get; private set; }

    [SerializeField] RectTransform ir_pointer;

    Controls _controlsKnm;
    InputAction controls;

    Image image;
    [Space]
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask enemyLayerMask;
    [Space]
    [SerializeField] Color idleColour;
    [SerializeField] Color activeColour;

    Vector2 screenPos;

    private void Awake()
    {
        Instance = this;
        image = ir_pointer.GetComponent<Image>();
    }

    private void OnEnable()
    {
        _controlsKnm = new Controls();
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

    #region Cursor functions
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
            //ir_pointer.transform.SetParent(transform.GetChild(0));


            Vector2 pointerPos = WiiInputManager.CursorWiiMote.IRPointScreenPos();
            screenPos = pointerPos;

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
        //ir_pointer.transform.SetParent(transform);

        Vector3 mappedCursor = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

        screenPos = mappedCursor;

        ir_pointer.anchorMin = mappedCursor;
        ir_pointer.anchorMax = mappedCursor;

        ir_pointer.anchoredPosition = Vector2.zero;
    }
    #endregion

    //Checks for enemy by raycasting below the cursor to the world.
    public IDamage CheckForEnemy()
    {
        //Ray ray = Camera.main.ScreenPointToRay(ir_pointer.position / PixelatedCamera.main.screenScaleFactor);

        Ray ray = Camera.main.ScreenPointToRay((WiiInputManager.CursorWiiMote.HasRemote ? WiiInputManager.CursorWiiMote.IRPointScreenPos() : (Vector2)Input.mousePosition) / PixelatedCamera.main.screenScaleFactor);
        RaycastHit hitInfo = new();


        if (Physics.Raycast(ray, out hitInfo, 999, layerMask))
        {
            Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.blue, Time.deltaTime);
            if (hitInfo.transform.TryGetComponent(out IDamage damageable))
            {
                return damageable;
            }
        }

        return null;
    }

    #region Strum behaviour
    //Computer strum input, calls guitar strum input function when testing
    private void Attack(InputAction.CallbackContext obj)
    {
        Attack();
    }

    //Basic interaction, called from guitar strum input
    private void Attack()
    {
        Ray cursorRay = Camera.main.ScreenPointToRay((WiiInputManager.CursorWiiMote.HasRemote ? WiiInputManager.CursorWiiMote.IRPointScreenPos() : (Vector2)Input.mousePosition) / PixelatedCamera.main.screenScaleFactor);
        RaycastHit hit = new();

        //Checks to see if the level is paused so the enemies in pause state can't be attacked
        if (LevelManager.isPaused)
        {
            //Trigger mouse click at cursor location if game is paused
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = LevelManager.pointer.GetComponent<RectTransform>().position;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (var result in results)
            {
                //Click button if part of PointerEventData results
                if (result.gameObject.TryGetComponent(out Button button))
                {
                    button.onClick.Invoke();
                }
            }
        }
        //If RuneFMODBridge exists, trigger rune interactions for power check then shoot enemy
        else if (RuneFMODBridge.Instance != null)
        {
            Shoot(RuneFMODBridge.Instance.RuneAttack());
        }
        else
        {
            // Get ray from cursor to world point on strum
            RaycastHit hitInfo = new();
            if (Physics.Raycast(cursorRay, out hitInfo, 30, layerMask))
            {
                if (hitInfo.transform.TryGetComponent(out UnityEngine.UI.Button _button))
                {
                    _button.onClick.Invoke();

                    return;
                }
            }
        }
    }

    //Spell launched at enemy at random if power is not less than 1
    public void Shoot(int power)
    {
        if (power > 0)
        {
            //Add score for successful rune play
            LevelManager.player.TryGetComponent(out IScore _Score); _Score.AddScore(0.05f, 20*power);

            IDamage hitItem = CheckForEnemy();

            if (hitItem != null)
            {
                //Launch spell at target if it's damageable
                GameObject baseSpell = ObjectPooler.Instance.SpawnRandomFromType(PoolType.Spell, LevelManager.player.transform.position);
                baseSpell.GetComponent<BaseSpell>().OnStart(hitItem.gameObject.transform, LevelManager.player.transform.position, power * 4, hitItem);
            }
        }
    }
    #endregion

}
