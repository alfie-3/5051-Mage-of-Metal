//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""GuitarControls"",
            ""id"": ""80e62223-fda8-45da-be4b-1d76195e8d24"",
            ""actions"": [
                {
                    ""name"": ""ScreenPoint"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0fbf1c68-37ea-4933-b735-2bd5cdabf96f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Note1"",
                    ""type"": ""Value"",
                    ""id"": ""a5bce679-bf97-459b-a2d2-a1f12ab55071"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Note2"",
                    ""type"": ""Value"",
                    ""id"": ""3136b2dd-77d0-4864-89f3-52b5b4a9799a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Note3"",
                    ""type"": ""Value"",
                    ""id"": ""a5b470ef-bfdf-490e-8e28-1f779a032e1c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Note4"",
                    ""type"": ""Value"",
                    ""id"": ""31a4c2e9-e9ab-44e0-98f1-6b375e6ef42c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Note5"",
                    ""type"": ""Value"",
                    ""id"": ""b041df77-640f-474b-8083-e9e9d3654c35"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Strum"",
                    ""type"": ""Button"",
                    ""id"": ""6614e8eb-0bc0-43d1-9835-011be2ccb78e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6829ba78-7e0f-414c-9aa6-54a59977ef04"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ScreenPoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8707e13b-65ee-4323-90dc-c3df7731f2ac"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Note1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b430fdef-82db-4697-b96d-3e7508e773a8"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Note2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d507ef16-4db7-4045-8cd1-bec2b34a9737"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Note3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f728fe1e-5bdc-4757-bf20-bd703fe92fb8"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Note4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24d8c848-8afb-477c-a7a7-0ac88680b857"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Note5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30b5365d-94a9-4af2-9ee4-66e37c8d8460"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Strum"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GuitarControls
        m_GuitarControls = asset.FindActionMap("GuitarControls", throwIfNotFound: true);
        m_GuitarControls_ScreenPoint = m_GuitarControls.FindAction("ScreenPoint", throwIfNotFound: true);
        m_GuitarControls_Note1 = m_GuitarControls.FindAction("Note1", throwIfNotFound: true);
        m_GuitarControls_Note2 = m_GuitarControls.FindAction("Note2", throwIfNotFound: true);
        m_GuitarControls_Note3 = m_GuitarControls.FindAction("Note3", throwIfNotFound: true);
        m_GuitarControls_Note4 = m_GuitarControls.FindAction("Note4", throwIfNotFound: true);
        m_GuitarControls_Note5 = m_GuitarControls.FindAction("Note5", throwIfNotFound: true);
        m_GuitarControls_Strum = m_GuitarControls.FindAction("Strum", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GuitarControls
    private readonly InputActionMap m_GuitarControls;
    private List<IGuitarControlsActions> m_GuitarControlsActionsCallbackInterfaces = new List<IGuitarControlsActions>();
    private readonly InputAction m_GuitarControls_ScreenPoint;
    private readonly InputAction m_GuitarControls_Note1;
    private readonly InputAction m_GuitarControls_Note2;
    private readonly InputAction m_GuitarControls_Note3;
    private readonly InputAction m_GuitarControls_Note4;
    private readonly InputAction m_GuitarControls_Note5;
    private readonly InputAction m_GuitarControls_Strum;
    public struct GuitarControlsActions
    {
        private @Controls m_Wrapper;
        public GuitarControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ScreenPoint => m_Wrapper.m_GuitarControls_ScreenPoint;
        public InputAction @Note1 => m_Wrapper.m_GuitarControls_Note1;
        public InputAction @Note2 => m_Wrapper.m_GuitarControls_Note2;
        public InputAction @Note3 => m_Wrapper.m_GuitarControls_Note3;
        public InputAction @Note4 => m_Wrapper.m_GuitarControls_Note4;
        public InputAction @Note5 => m_Wrapper.m_GuitarControls_Note5;
        public InputAction @Strum => m_Wrapper.m_GuitarControls_Strum;
        public InputActionMap Get() { return m_Wrapper.m_GuitarControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GuitarControlsActions set) { return set.Get(); }
        public void AddCallbacks(IGuitarControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_GuitarControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GuitarControlsActionsCallbackInterfaces.Add(instance);
            @ScreenPoint.started += instance.OnScreenPoint;
            @ScreenPoint.performed += instance.OnScreenPoint;
            @ScreenPoint.canceled += instance.OnScreenPoint;
            @Note1.started += instance.OnNote1;
            @Note1.performed += instance.OnNote1;
            @Note1.canceled += instance.OnNote1;
            @Note2.started += instance.OnNote2;
            @Note2.performed += instance.OnNote2;
            @Note2.canceled += instance.OnNote2;
            @Note3.started += instance.OnNote3;
            @Note3.performed += instance.OnNote3;
            @Note3.canceled += instance.OnNote3;
            @Note4.started += instance.OnNote4;
            @Note4.performed += instance.OnNote4;
            @Note4.canceled += instance.OnNote4;
            @Note5.started += instance.OnNote5;
            @Note5.performed += instance.OnNote5;
            @Note5.canceled += instance.OnNote5;
            @Strum.started += instance.OnStrum;
            @Strum.performed += instance.OnStrum;
            @Strum.canceled += instance.OnStrum;
        }

        private void UnregisterCallbacks(IGuitarControlsActions instance)
        {
            @ScreenPoint.started -= instance.OnScreenPoint;
            @ScreenPoint.performed -= instance.OnScreenPoint;
            @ScreenPoint.canceled -= instance.OnScreenPoint;
            @Note1.started -= instance.OnNote1;
            @Note1.performed -= instance.OnNote1;
            @Note1.canceled -= instance.OnNote1;
            @Note2.started -= instance.OnNote2;
            @Note2.performed -= instance.OnNote2;
            @Note2.canceled -= instance.OnNote2;
            @Note3.started -= instance.OnNote3;
            @Note3.performed -= instance.OnNote3;
            @Note3.canceled -= instance.OnNote3;
            @Note4.started -= instance.OnNote4;
            @Note4.performed -= instance.OnNote4;
            @Note4.canceled -= instance.OnNote4;
            @Note5.started -= instance.OnNote5;
            @Note5.performed -= instance.OnNote5;
            @Note5.canceled -= instance.OnNote5;
            @Strum.started -= instance.OnStrum;
            @Strum.performed -= instance.OnStrum;
            @Strum.canceled -= instance.OnStrum;
        }

        public void RemoveCallbacks(IGuitarControlsActions instance)
        {
            if (m_Wrapper.m_GuitarControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGuitarControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_GuitarControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GuitarControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GuitarControlsActions @GuitarControls => new GuitarControlsActions(this);
    public interface IGuitarControlsActions
    {
        void OnScreenPoint(InputAction.CallbackContext context);
        void OnNote1(InputAction.CallbackContext context);
        void OnNote2(InputAction.CallbackContext context);
        void OnNote3(InputAction.CallbackContext context);
        void OnNote4(InputAction.CallbackContext context);
        void OnNote5(InputAction.CallbackContext context);
        void OnStrum(InputAction.CallbackContext context);
    }
}
