using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnnouncerDialouge : MonoBehaviour
{

    public Text dialouge;

    public string[] dialougePlayerOnPlayerCollision;
    public string[] dialougePlayerShot;
    public string[] dialougePlayerTrapSetup;
    public string[] dialougePlayerTrapTrigger;
    
    
    string[] dialougePlayerTaunting;

    private AudioEvent audioEvent;
    bool isInit = false;


    // Start is called before the first frame update
    void Start()
    {
        audioEvent = GameObject.Find("Announcer").GetComponent<AudioEvent>();
        dialougePlayerShot = new string[11];
        dialougePlayerOnPlayerCollision = new string[24];
        dialougePlayerTrapSetup = new string[5];
        dialougePlayerTrapTrigger = new string[16];

    }

    void InitLines()
    {
        
        
        dialougePlayerShot[0] = "Oh! Longshot!";
        dialougePlayerShot[1] = "Pinpoint precision";
        dialougePlayerShot[2] = "Oh ho! Looks like the wind was blowing in their favour";
        dialougePlayerShot[3] = "Oh ho, there's a new sherrif in town!";
        dialougePlayerShot[4] = "Shooting like their life depended on it";
        dialougePlayerShot[5] = "Sharpshooter!";
        dialougePlayerShot[6] = "Oh! That shot landed.";
        dialougePlayerShot[7] = "Somebody get this cowboy a horse!";
        dialougePlayerShot[8] = "Ooo! Shooting like it's just target practice!";
        dialougePlayerShot[9] = "Oh ho-hoo! Gunslinger!";
        dialougePlayerShot[10] = "Oh ho, their rooting, tooting, pointing and shooting!";

        dialougePlayerOnPlayerCollision[0] = "Ooo! That looked like it hurt";
        dialougePlayerOnPlayerCollision[1] = "That's gonna sting in the morning!";
        dialougePlayerOnPlayerCollision[2] = "Lets hope they can walk after that one...";
        dialougePlayerOnPlayerCollision[3] = "Lets hope they can walk after that one!";
        dialougePlayerOnPlayerCollision[4] = "Is anyone a doctor in here?";
        dialougePlayerOnPlayerCollision[5] = "Oh ho! Serving up a beatdown!";
        dialougePlayerOnPlayerCollision[6] = "T-Boooooone!";
        dialougePlayerOnPlayerCollision[7] = "Oh ho great hit!";
        dialougePlayerOnPlayerCollision[8] = "Looks like they're not afraid to trade some paint!";
        dialougePlayerOnPlayerCollision[9] = "Looks like they're not afraid to trade some paint!";
        dialougePlayerOnPlayerCollision[10] = "OH! I hope he's still alive";
        dialougePlayerOnPlayerCollision[11] = "I hope you're looking forward to eating food out of a tube!";
        dialougePlayerOnPlayerCollision[12] = "That was brutal!";
        dialougePlayerOnPlayerCollision[13] = "Uhhh... I don't think intergalactic health insurance will cover that...";
        dialougePlayerOnPlayerCollision[14] = "Now that's one way to take out your anger issues.";
        dialougePlayerOnPlayerCollision[15] = "Now that is ONE way to take out your anger issues.";
        dialougePlayerOnPlayerCollision[16] = "Good luck recovering from that one...";
        dialougePlayerOnPlayerCollision[17] = "Ohh, avert your eyes!";
        dialougePlayerOnPlayerCollision[18] = "Looks like their new nickname is going to be scrapper!";
        dialougePlayerOnPlayerCollision[19] = "OH! Head on!";
        dialougePlayerOnPlayerCollision[20] = "Oh ho hoo! Head on!";
        dialougePlayerOnPlayerCollision[21] = "Ahrg! There was no escaping that one.";
        dialougePlayerOnPlayerCollision[22] = "There was no escaping that one.";
        dialougePlayerOnPlayerCollision[23] = "I can't believe that just happened!";
        
        dialougePlayerTrapSetup[0] = "Somebody is setting up a trap!";
        dialougePlayerTrapSetup[1] = "Looks like there's a trap being activated!";
        dialougePlayerTrapSetup[2] = "Somebody just flipped the switch on a trap!";
        dialougePlayerTrapSetup[3] = "Looks like somebody is setting up a trap.";
        dialougePlayerTrapSetup[4] = "Look out! A trap has been activated!";
        
        dialougePlayerTrapTrigger[0] = "How did they not see that?";
        dialougePlayerTrapTrigger[1] = "Are they blind?";
        dialougePlayerTrapTrigger[2] = "Ohh! Fell into a trap!";
        dialougePlayerTrapTrigger[3] = "Oh, now that was clumsy.";
        dialougePlayerTrapTrigger[4] = "I don't think insurance covers THAT.";
        dialougePlayerTrapTrigger[5] = "Oh ho! Head first into a trap.";
        dialougePlayerTrapTrigger[6] = "Now that's something you don't see everyday.";
        dialougePlayerTrapTrigger[7] = "Oh, a dissapointing mistake...";
        dialougePlayerTrapTrigger[8] = "It is ALLMOST as if they didn't see it.";
        dialougePlayerTrapTrigger[9] = "Let's hope they can come back from that one...";
        dialougePlayerTrapTrigger[10] = "Uhh, let's hope they can come back after that.";
        dialougePlayerTrapTrigger[11] = "And the trap takes another victim.";
        dialougePlayerTrapTrigger[12] = "Ugh, now THATS embarrassing...";
        dialougePlayerTrapTrigger[13] = "It's almost as if they drove straight into it.";
        dialougePlayerTrapTrigger[14] = "Shameful display...";
        dialougePlayerTrapTrigger[15] = "Now that's a mistake they'll never make again.";

        isInit = true;
    }

    public void DisplayPlayerShotSubtitle(int subtitle)
    {
        dialouge.text =dialougePlayerShot[subtitle];
    }

    public void DisplayPlayerOnPlayerCollisionSubtitle(int subtitle)
    {
        dialouge.text = dialougePlayerOnPlayerCollision[subtitle];
    }

    public void DiaplayPlayerTrapSetupSubtitle(int subtitle)
    {
        dialouge.text = dialougePlayerTrapSetup[subtitle];
    }

    public void DiaplayPlayerTrapTriggerSubtitle(int subtitle)
    {
        dialouge.text = dialougePlayerTrapTrigger[subtitle];
    }


    // Update is called once per frame
    void Update()
    {
        if(isInit == false)
        {
            InitLines();
        }

        if(!audioEvent.isPlaying)
        {
            dialouge.enabled = false;
        }
        else
        {
            dialouge.enabled = true;
        }
    }
}
