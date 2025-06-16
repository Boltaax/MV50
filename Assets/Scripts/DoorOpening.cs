using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoorOpening : MonoBehaviour
{
    public GameObject button;
    public AudioSource audioDoor;
    public AudioSource audioClassroom;
    public AudioSource audioHeartBeat;
    public Animator animator;
    private bool estOuverte = false;

    public void TogglePorte()
    {
        if (estOuverte)
        {
            animator.SetBool("Ouvrir/Fermer", false);
            estOuverte = false;
        }
        else
        {
            estOuverte = true;
            StartCoroutine(SequenceAvecAttente());
        }
    }

    IEnumerator SequenceAvecAttente()
    {
        audioDoor.Play();

        yield return new WaitForSeconds(4f);

        audioClassroom.Pause();

        audioHeartBeat.Play();

        animator.SetBool("Ouvrir/Fermer", true);

        button.SetActive(false);

        // Active tous les scripts LookAtPlayer de la scène
        LookAtPlayer[] lookScripts = FindObjectsByType<LookAtPlayer>(FindObjectsSortMode.None); // true = inclut objets inactifs

        foreach (LookAtPlayer script in lookScripts)
        {
            script.enabled = true;
        }
    }
}
