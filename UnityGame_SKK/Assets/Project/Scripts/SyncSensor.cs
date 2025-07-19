using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncSensor : MonoBehaviour
{
    public GameObject shouheki;
    public Renderer button1Renderer;
    public Renderer button2Renderer;
    public float cooldownTime = 5f;

    private bool isInCooldown = false;
    private Color originalColor;

    void Start()
    {
        if (button1Renderer != null)
            originalColor = button1Renderer.material.color;

        if (shouheki != null)
            shouheki.SetActive(true);
    }

    public void TriggerFromSensor()
    {
        if (!isInCooldown)
            StartCoroutine(HandleWallToggle());
    }

    IEnumerator HandleWallToggle()
    {
        isInCooldown = true;

        if (shouheki != null)
            shouheki.SetActive(false);

        if (button1Renderer != null)
            button1Renderer.material.color = Color.white;
        if (button2Renderer != null)
            button2Renderer.material.color = Color.white;

        yield return new WaitForSeconds(cooldownTime);

        if (shouheki != null)
            shouheki.SetActive(true);

        if (button1Renderer != null)
            button1Renderer.material.color = originalColor;
        if (button2Renderer != null)
            button2Renderer.material.color = originalColor;

        isInCooldown = false;
    }
}
