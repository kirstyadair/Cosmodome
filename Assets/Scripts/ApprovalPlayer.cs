using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApprovalPlayer : MonoBehaviour
{
    public Image[] approvalArrows;

    enum ArrowType
    {
        UP_LARGE,
        UP_SMALL,
        DOWN_LARGE,
        DOWN_SMALL
    }

    void ShowApproval(ArrowType arrowType)
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        Color tempColor = approvalArrows[(int)ArrowType.UP_LARGE].color;
        tempColor.a = 0f;

        approvalArrows[(int)ArrowType.UP_LARGE].color = tempColor;
        approvalArrows[(int)ArrowType.UP_SMALL].color = tempColor;
        approvalArrows[(int)ArrowType.DOWN_LARGE].color = tempColor;
        approvalArrows[(int)ArrowType.DOWN_SMALL].color = tempColor;


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 imagePos = Camera.main.WorldToScreenPoint(this.transform.position);
        
    }
}
