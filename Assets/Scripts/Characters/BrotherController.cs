using UnityEngine;

public class BrotherController : MonoBehaviour
{
    [SerializeField] GameObject Eyes;
    private void OnDisable()
    {
        foreach(var eye in Eyes.GetComponentsInChildren<EyeHazardController>())
        {
            eye.EnableEyeHazard();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerCharacterController>() != null)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerCharacterController>() != null)
        {
            gameObject.SetActive(false);
        }
    }
}
