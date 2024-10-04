using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private Vector3 originalScale;  
    public float pressScale = 0.9f;  
    public float pressDuration = 0.1f;  

    private AudioSource audioSource;

    private void Start()
    {
        originalScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.localScale = originalScale * pressScale;

            audioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            transform.localScale = originalScale;
        }
    }
}
