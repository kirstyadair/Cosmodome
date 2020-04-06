using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject firePS;
    [SerializeField] GameObject burstFirePS;
    [SerializeField] GameObject fireworksPS;
    
    void OnEnable()
    {
        ExcitementManager.OnComboIncrease += BurstFire;
    }

    void Start()
    {
        GameObject ps = Instantiate(firePS, transform.position, transform.rotation);
    }

    void BurstFire(PlayerData playerData)
    {
        GameObject ps = Instantiate(burstFirePS, transform.position, transform.rotation);
        ps.GetComponent<ParticleSystem>().startColor = playerData.playerColor;
    }

    void Fireworks()
    {
        GameObject ps = Instantiate(fireworksPS, transform.position, transform.rotation);
    }
}
