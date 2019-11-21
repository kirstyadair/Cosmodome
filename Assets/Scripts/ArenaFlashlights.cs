using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaFlashlights : MonoBehaviour
{
    public Light[] lights;
    public float flashChance = 0.01f;
    public float flashLength = 0.5f;
    public float extraFlashChance = 0f;
    public float extraFlashOffset = 0.5f;

    void Update()
    {
        if (Random.Range(0, 1f) < flashChance + extraFlashChance)
        {
            Light chosenLight = lights[Random.Range(0, lights.Length)];
            chosenLight.enabled = true;

            extraFlashChance = extraFlashOffset;
            StartCoroutine(TurnOffLight(chosenLight));
        }

        if (extraFlashChance > 0) extraFlashChance -= Time.deltaTime;
    }

    public IEnumerator TurnOffLight(Light light)
    {
        yield return new WaitForSeconds(flashLength);
        light.enabled = false;
    }
}
