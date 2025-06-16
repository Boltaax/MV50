using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public InputActionReference openMenuAction;
    public TMP_Dropdown dropdown;
    private int selectedOption = 0; // Par défaut première option sélectionnée
    private string currentSceneName;

    void Start()
    {
        openMenuAction.action.Enable();
        openMenuAction.action.performed += OpenMenu;
        currentSceneName = SceneManager.GetActiveScene().name;
        if(PersistantDataScript.instance.dayStarted){
            dropdown.options[0].text="Retourner a la maison";
            dropdown.options[1].text="Recommencer la scene";
            dropdown.options[2].text="Quitter";
        }
        else{
            dropdown.options[0].text="Commencer la journée";
            dropdown.options[1].text="Aller Dehors";
            dropdown.options[2].text="Aller en classe";
            dropdown.options.Add(new TMP_Dropdown.OptionData("Quitter"));
        }
        dropdown.RefreshShownValue();
    }

    private void OpenMenu(InputAction.CallbackContext context)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OnOptionSelect(int option)
    {
        selectedOption = option;
    }

    public void ConfirmChoice()
    {
        if (selectedOption < 0 || selectedOption >= dropdown.options.Count)
        {
            Debug.LogWarning("Option sélectionnée invalide.");
            return;
        }

        string selectedText = dropdown.options[selectedOption].text.ToLower();

        switch (selectedText)
        {
            case "commencer la journée":
                PersistantDataScript.instance.dayStarted = true;
                //GetComponent<ChangeScene>().LoadScene("Foule");//mettre le nom de la scene de la foule
                Debug.Log("Lance la scene de la foule");
                break;
            case "retourner a la maison":
                PersistantDataScript.instance.dayStarted = false;
                GetComponent<ChangeScene>().LoadScene("HouseHub");//retourne a la scene de la maison
                break;
            case "recommencer la scene":
                GetComponent<ChangeScene>().LoadScene(currentSceneName);//reload la scene actuelle
                break;
            case "aller dehors":
                PersistantDataScript.instance.dayStarted = true;
                GetComponent<ChangeScene>().LoadScene("Foule");//mettre le nom de la scene de la foule
                break;
            case "aller en classe":
                PersistantDataScript.instance.dayStarted = true;
                GetComponent<ChangeScene>().LoadScene("SchoolScene");//va direct dans la salle de classe
                break;
            case "quitter":
                Application.Quit();
                break;
            default:
                Debug.LogWarning("Option texte inconnue : " + selectedText);
                break;
        }
    }
}
