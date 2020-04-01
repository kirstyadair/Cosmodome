using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IconToUI : MonoBehaviour
{
    [SerializeField] Image iconMaster;
    [SerializeField] Sprite greenIcon;
    [SerializeField] Sprite redIcon;
    public Vector3 iconOffset;
    public GameObject target;



    void OnEnable()
    {
        PlayerScript.OnPlayerCollision += CreateGreenIcon;
        SpikeTrapScript.OnPlayerSpikeHit += CreateRedIcon;
        PlayerScript.OnPlayerHitByArenaCannon += CreateRedIcon;
    }

    void OnDisable()
    {
        PlayerScript.OnPlayerCollision -= CreateGreenIcon;
        SpikeTrapScript.OnPlayerSpikeHit -= CreateRedIcon;
        PlayerScript.OnPlayerHitByArenaCannon -= CreateRedIcon;
    }


    Vector3 GetPosition(Image icon, GameObject player)
    {
        Vector3 iconPos = Camera.main.WorldToScreenPoint(player.transform.position);
        return icon.transform.position = iconPos + iconOffset;
    }

    void CreateGreenIcon(PlayerScript playerHit, PlayerScript playerAttacking)
    {
        if(playerAttacking.gameObject==gameObject)
        {
            Image newIcon;
            newIcon = Instantiate(iconMaster);
            newIcon.sprite = greenIcon;
            Vector3 newIconPos = GetPosition(newIcon, playerAttacking.gameObject);
            newIcon.transform.SetParent(GameObject.Find("MainUICanvas").transform, false);
            newIcon.transform.position = newIconPos + iconOffset;

            StartCoroutine(MoveIcon(newIcon, 1f));
        }
    }

    void CreateRedIcon(PlayerScript playerHit)
    {
        if (playerHit.gameObject == gameObject)
        {
            Image newIcon;
            newIcon = Instantiate(iconMaster);
            newIcon.sprite = redIcon;
            Vector3 newIconPos = GetPosition(newIcon, playerHit.gameObject);
            newIcon.transform.SetParent(GameObject.Find("MainUICanvas").transform, false);
            newIcon.transform.position = newIconPos + iconOffset;

            StartCoroutine(MoveIcon(newIcon, 1f));
        }
    }

    IEnumerator MoveIcon(Image icon,float overTime)
    {
        float startTime = Time.time;

        while(Time.time<startTime+overTime)
        {
            icon.transform.position = Vector3.Lerp(icon.transform.position, target.transform.position, (Time.time-startTime)*0.1f);
            yield return null;
        }
        if(Time.time>=startTime+overTime)
        {
            Destroy(icon);
        }
        
    }

}
