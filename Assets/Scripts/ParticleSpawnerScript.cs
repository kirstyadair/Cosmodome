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
        ExcitementManager.OnComboIncrease += Fireworks;
        ScoreManager.OnRemovePlayer += Fireworks;
    }

    void OnDestroy()
    {
        ExcitementManager.OnComboIncrease -= BurstFire;
        ScoreManager.OnRemovePlayer -= Fireworks;
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

    void Fireworks(PlayerData playerData)
    {
        GameObject ps = Instantiate(fireworksPS, transform.position, transform.rotation);
        ps.GetComponent<ParticleSystem>().startColor = playerData.playerColor;
    }
}
