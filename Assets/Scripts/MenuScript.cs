using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public InputActionReference openMenuAction;
    public TMP_Dropdown dropdown;
    private int selectedOption = -1;
    public TMP_Text optionText;

    void Start()
    {
        openMenuAction.action.Enable();
        openMenuAction.action.performed += OpenMenu;
    }

    private void OpenMenu(InputAction.CallbackContext context)
    {
        Debug.Log("Hello, open menu");
        gameObject.SetActive(!gameObject.activeSelf);
        optionText.gameObject.SetActive(false);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                openMenuAction.action.Disable();
                openMenuAction.action.performed -= OpenMenu;
                break;
            case InputDeviceChange.Reconnected:
                openMenuAction.action.Enable();
                openMenuAction.action.performed += OpenMenu;
                break;
        }
    }

    private void UpdateChoiceText()
    {
        optionText.gameObject.SetActive(true);
        Debug.Assert(selectedOption >= 0);
        Debug.Assert(optionText != null);
        optionText.text = $"Scénario : {(char)(selectedOption + 65)}";
    }

    public void OnOptionSelect(int option)
    {
        selectedOption = option;
        UpdateChoiceText();
    }

    public void ConfirmChoice()
    {
        GetComponent<ChangeScene>().LoadScene(selectedOption);
    }
}
