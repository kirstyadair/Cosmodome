using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IconToUI : MonoBehaviour
{
    public Image iconMaster;
    public Vector3 iconOffset;
    public GameObject target;



    void Start()
    {
        PlayerScript.OnPlayerCollision += CreateIcon;
    }

    void OnDisable() {
        PlayerScript.OnPlayerCollision -= CreateIcon;
    }


    Vector3 GetPosition(Image icon, GameObject player)
    {
        Vector3 iconPos = Camera.main.WorldToScreenPoint(player.transform.position);
        return icon.transform.position = iconPos + iconOffset;
    }

    void CreateIcon(PlayerScript playerHit, PlayerScript playerAttacking)
    {
        
        if(playerAttacking.gameObject==gameObject)
        {
            Image newIcon;
            newIcon = Instantiate(iconMaster);
            Vector3 newIconPos = GetPosition(newIcon, playerAttacking.gameObject);
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


    void Update()
    {
        

    }

}
