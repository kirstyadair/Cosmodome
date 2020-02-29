using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceDiversifiier : MonoBehaviour
{
    GameObject[] alienPositions;
    List<Animator> aliens = new List<Animator>();
    public GameObject[] possiblePrefabs;
    ScoreManager sm;
    public float speed;

    public Vector2 animatorSpeedRange;
    public Vector2 alienScaleRange;
    public Vector2 alienAnimateRange;

    // Start is called before the first frame update
    void Start()
    {
        alienPositions = GameObject.FindGameObjectsWithTag("CrowdMember");
        sm = ScoreManager.Instance;
        ExcitementManager.OnAddHype += RandomizeSpeed;
        ExcitementManager.OnResetHype += RandomizeSpeed;
        Randomize();
        //FaceTowardCamera(); nah
    }

    public void FaceTowardCamera()
    {
        //foreach (GameObject alienSprite in alienSprites)
        //{
            ///alienSprite.transform.forward = -Camera.main.transform.forward;
        //}
    }

    public void RandomizeSpeed()
    {
        foreach (Animator alien in aliens)
        {

            alien.SetFloat("Speed", speed);
        }

        
    }

    public void Randomize()
    {
        foreach (GameObject position in alienPositions)
        {
            int randomNumber = Random.Range(0, 5);
            GameObject alien = Instantiate(possiblePrefabs[randomNumber], position.transform.position, position.transform.rotation, this.gameObject.transform);
            aliens.Add(alien.GetComponent<Animator>());
        }
    }
}
