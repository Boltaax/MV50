using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerLockZone : MonoBehaviour
{
    public Transform chairTransform;
    public GameObject xrRig;  // XR Origin
    public GameObject locomotionSystem;

    public AudioSource audioClassroom;
    public AudioSource audioHeartBeat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Téléporte le XR Rig
            xrRig.transform.position = chairTransform.position;
            xrRig.transform.rotation = chairTransform.rotation;

            // Désactive la locomotion (téléportation + déplacement continu)
            if (locomotionSystem != null)
            {
                locomotionSystem.SetActive(false);
            }

            audioClassroom.Play();
            audioHeartBeat.Pause();
        }
    }
}
