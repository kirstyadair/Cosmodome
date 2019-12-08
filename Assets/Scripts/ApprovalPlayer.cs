using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApprovalPlayer : MonoBehaviour
{
    public Image[] approvalArrows;
    public GameObject player;
    public int approvalValue;
    int currentAppoval;


    enum ArrowType
    {
        UP_LARGE,
        UP_SMALL,
        DOWN_LARGE,
        DOWN_SMALL
    }


    // Start is called before the first frame update
    void Start()
    {
        approvalValue = player.GetComponent<PlayerScript>().approval.percentage;
        currentAppoval = approvalValue;

        Color tempColor = approvalArrows[(int)ArrowType.UP_LARGE].color;
        tempColor.a = 0f;

        approvalArrows[(int)ArrowType.UP_LARGE].color = tempColor;
        approvalArrows[(int)ArrowType.UP_SMALL].color = tempColor;
        approvalArrows[(int)ArrowType.DOWN_LARGE].color = tempColor;
        approvalArrows[(int)ArrowType.DOWN_SMALL].color = tempColor;


    }

    private IEnumerator ShowApproval(ArrowType arrowType,float timeMultiplier)
    {
        approvalArrows[(int)arrowType].color = new Color(approvalArrows[(int)arrowType].color.r, approvalArrows[(int)arrowType].color.g, approvalArrows[(int)arrowType].color.b, 1);

        while (approvalArrows[(int)arrowType].color.a > 0.0f)
        {
            approvalArrows[(int)arrowType].color = new Color(approvalArrows[(int)arrowType].color.r, approvalArrows[(int)arrowType].color.g, approvalArrows[(int)arrowType].color.b, approvalArrows[(int)arrowType].color.a - (Time.deltaTime * timeMultiplier));
            yield return null;
        }
    }

    public void StartPlayerApproval(int arrowType)
    {
        if(arrowType==1)
        {
            StartCoroutine(ShowApproval(ArrowType.UP_LARGE, .5f));
        }
        if (arrowType == 2)
        {
            StartCoroutine(ShowApproval(ArrowType.UP_SMALL, .5f));
        }
        if (arrowType == 3)
        {
            StartCoroutine(ShowApproval(ArrowType.DOWN_LARGE, .5f));
        }
        if (arrowType == 4)
        {
            StartCoroutine(ShowApproval(ArrowType.DOWN_SMALL, .5f));
        }


    }

    // Update is called once per frame
    void Update()
    {
        approvalValue = player.GetComponent<PlayerScript>().approval.percentage;
        if (currentAppoval!=approvalValue)
        {
            int changeInApproval = approvalValue  - currentAppoval;
            print(changeInApproval);

            if(changeInApproval <= -3 )
            {
                StartPlayerApproval(3);
            }
            if (changeInApproval <= -1 && changeInApproval >= -2)
            {
                StartPlayerApproval(4);
            }
            if(changeInApproval>=3)
            {
                StartPlayerApproval(1);
            }
            if (changeInApproval >= 1 && changeInApproval <=2)
            {
                StartPlayerApproval(2);
            }


            currentAppoval = approvalValue;
        }

        Vector3 imagePos = Camera.main.WorldToScreenPoint(this.transform.position);
        for(int i=0; i<approvalArrows.Length;i++)
        {
            approvalArrows[i].transform.position = Vector3.Lerp(approvalArrows[i].transform.position, imagePos + new Vector3(0, 10, 0),Time.deltaTime*20);
        }

        
    }
}
