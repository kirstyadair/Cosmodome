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

    public string[] dialouge_WelcomeToTheCosmodome;
    public string[] dialouge_DaveIntro;
    public string[] dialouge_BigSchlugIntro;
    public string[] dialouge_HHHIntro;
    public string[] dialouge_ElMosoIntro;




    string[] dialougePlayerTaunting;

    private AudioEvent audioEvent;

    bool isInit = false;



    public void OnStateChange(GameState newState, GameState oldState)
    {
        if (newState == GameState.INGAME)
        {
            dialouge.color = new Color32(255, 255, 255, 255);
        }
        if(newState ==GameState.ROUND_START_CUTSCENE)
        {
            dialouge.color = new Color32(255, 255, 255, 255);
        }
        if(newState==GameState.COUNTDOWN|| newState == GameState.END_OF_GAME|| newState == GameState.ROUND_END_CUTSCENE)
        {
            dialouge.color = new Color32(0, 0, 0, 0);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        ScoreManager.OnStateChanged += OnStateChange;

        audioEvent = GameObject.Find("Announcer").GetComponent<AudioEvent>();
        dialougePlayerShot = new string[11];
        dialougePlayerOnPlayerCollision = new string[24];
        dialougePlayerTrapSetup = new string[5];
        dialougePlayerTrapTrigger = new string[16];

        dialouge_WelcomeToTheCosmodome = new string[11];
        dialouge_DaveIntro = new string[11];
        dialouge_BigSchlugIntro = new string[16];
        dialouge_HHHIntro = new string[10];

    }

    void OnDisable() {
        ScoreManager.OnStateChanged -= OnStateChange;
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

        dialouge_WelcomeToTheCosmodome[0] = "Welcome to the CosmoDome! If you look behind your seats you’ll find a complimentary CosmoCorp™ debris shield! Feel free to use this when the event starts as there is no telling what will happen!";
        dialouge_WelcomeToTheCosmodome[1] = "Welcome to the CosmoDome! The only place in the 6e nebula that has 0 documented cases of employee theft, I mean the big binder with all the employee information disappeared but that’s just- awwww";
        dialouge_WelcomeToTheCosmodome[2] = "Welcome to the CosmoDome! One of only 300 places in the universe that has all 215 food intolerance certificates!";
        dialouge_WelcomeToTheCosmodome[3] = "Welcome to the DosmoCome! -Oh hold on ehhh... Welcome to the cosmodome! The only place where you can get hired 15 minutes before you are meant to commentate!";
        dialouge_WelcomeToTheCosmodome[4] = "Welcome to the CosmoDome! One of the only places where you can watch a human bad mouth a schlug-being and not totally get eaten straight away!";
        dialouge_WelcomeToTheCosmodome[5] = "Come one, come all to the cosmodome! Are you ready to witness the most amazing ship fights that don’t result in your imminent demise?";
        dialouge_WelcomeToTheCosmodome[6] = "Welcome to the CosmoDome! The one place where all contestant are legal obliged to sit next to each other and not eat one and other! Because if they could, and they would, then it wouldn’t be very fun to watch.";
        dialouge_WelcomeToTheCosmodome[7] = "Welcome to the CosmoDome! The only place that you can watch a schlug-being get salty and not dissolve into a watery paste!";
        dialouge_WelcomeToTheCosmodome[8] = "Welcome to the CosmoDome! Now sponsored by spearizate! If you need a new grator for your otiation drive then don’t hesitate, go spearizate!";
        dialouge_WelcomeToTheCosmodome[9] = "Welcome to the CosmoDome! The current record holder of the smallest establishment that a has class-7 licence to laser weapons! Take that StarSystemSphere™";
        dialouge_WelcomeToTheCosmodome[10] = "Welcome everybody to the cosmodome! I would like to remind you that after the event, feel free to visit our new cosmoStore! Where you can buy smaller, less nuclear volatile models of all of the ships that our contestants are using today!";

        dialouge_DaveIntro[0]= "With only 3 hours experience of controlling a grade 4 jet propulsion machines. Its Dave!";
        dialouge_DaveIntro[1]= "Making his debut entrance into ship fighting, even though he’s got 6 points on his licence, its Dave!";
        dialouge_DaveIntro[2]= "With an average IQ of 120 and a degree in civil engineering, its Dave!";
        dialouge_DaveIntro[3]= "Coming in at 45 pounds! That’s how much money he has in his pocket and he is not willing to lose it! Its Dave!";
        dialouge_DaveIntro[4]= "He may be the only human in this contest, but what he lacks in… well everything, he makes up in style. It's Dave!";
        dialouge_DaveIntro[5]= "Human-Robot hybrids exist, but he is still 100% gross meat, its Dave!";
        dialouge_DaveIntro[6]= "Its Dave! The only human in this contest that still insists in using a helmet to protect his squishy fragile brain instead of, you know, getting a titanium skull like everyone else.";
        dialouge_DaveIntro[7]= "Coming from a planet that still uses old farts to make their things work, its dave!";
        dialouge_DaveIntro[8]= "Its Dave! The only thing that stands in his way to victory is other vastly superior lifeforms that will outlive him by as much as 200 years! Try not to think about that Dave!";
        dialouge_DaveIntro[9]= "He may only be bipedal but that doesn’t stop him from putting one of those pedals to the metal! Its Dave!";
        dialouge_DaveIntro[10]= "Its Dave! He may find every alien in this arena repulsive and gross looking, but the jokes on him, because they all think the same about him!";

        dialouge_BigSchlugIntro[0] = "He used to be a general in the military until the as-salt of white mountain, heh-heh , it’s big schlug!";
        dialouge_BigSchlugIntro[1] = "Being huge and scary has it’s benefits! Like how I don’t want to make fun of him in case I suddenly disappear in the near future. Its big schlug!";
        dialouge_BigSchlugIntro[2] = "Having no bones whatsoever and only big strong large oily muscles- uhh its big schlug.";
        dialouge_BigSchlugIntro[3] = "He’s not your everyday garden nuisance, it’s big schlug!";
        dialouge_BigSchlugIntro[4] = "Leaving a trail of slime for everyone to slip on and hopefully not be event ready, it’s big schlug!";
        dialouge_BigSchlugIntro[5] = "He didn’t like the nickname little slugger so he bulked up! It’s big schlug!";
        dialouge_BigSchlugIntro[6] = "Having no teeth is no problem when you can mercilessly grind the competition down! It’s big schlug!";
        dialouge_BigSchlugIntro[7] = "With only one lung, and a terrifying amount of surprisingly slippery muscle, it’s big schlug!";
        dialouge_BigSchlugIntro[8] = "Spending most of his time underground plotting to take over at least 3 asteroid belts, it’s big schlug!";
        dialouge_BigSchlugIntro[9] = "Coming from the only planet that counts table salt as illegal contraband, it’s big schlug!";
        dialouge_BigSchlugIntro[10] = "Being huge and scary has it’s benefits! Like how I don’t want to make fun of him in case I suddenly disappear in the near future. Its big schlug!";
        dialouge_BigSchlugIntro[11] = "Coming from a planet that mainly slides to get from one point to another its big schlug!";
        dialouge_BigSchlugIntro[12] = "Their entire species began when someone decided to put steroids into a garden slug, it’s big schlug!";
        dialouge_BigSchlugIntro[13] = "The bigger they are, the harder they slide away without a trace and come back when you least expect it, it’s big schlug!";
        dialouge_BigSchlugIntro[14] = "Being a vegetarian is tough when your packing same amount of muscle as a wild bear, its big schlug!";
        dialouge_BigSchlugIntro[15] = "With one eye that stares deep and long into your terrified husk of a body, it’s big schlug!";

        dialouge_HHHIntro[0] = "Most sharks dream of one day being able to walk outside of the water, but Henry, he just decided to go to space and crash into people.";
        dialouge_HHHIntro[1] = "Hammerhead Henry is one half of the greatest champion of Cosmodome, the other half is his alter ego, Bartholomew. He scares me.";
        dialouge_HHHIntro[2] = "There he is in all his glory, Hammerhead Henry - ohhh no, don’t show him doing THAT. Oh wait, that’s how he breathes?";
        dialouge_HHHIntro[3] = "It's Hammerhead Henry or Triple H as he is commonly known as, The King of Kings,  that’s just because sharks are king of the seas and he decided to force us to call him that.";
        dialouge_HHHIntro[4] = "Sharks are considered the predators of the sea but today there’s no water so who knows what he can be considered as, its henry!";
        dialouge_HHHIntro[5] = "Half shark, half alien. A weird shark-thing hybrid, could be considered a monstrosity, but look at everyone else here. I’m looking at you, son. ";
        dialouge_HHHIntro[6] = "THAT'S NOT A CONTSESTANT, THAT'S A SHARK, HOW DID IT EVEN GET IN HERE?! (other announcer)- umm he filled in a form and got a spot? -(announcer) OH, it's Henry";
        dialouge_HHHIntro[7] = "It's Hammerhead Henry, he's got eyes on either side of his head that COULD be counted as an unfair advantage but who knows?";
        dialouge_HHHIntro[8] = "Hammerhead Henry, he’s got one eye on the FIN-ISH line... heh. And one eye on tonight’s dinner, whatever sharks eat";
        dialouge_HHHIntro[9] = "With a head like that I bet he's gonna hammer the competition today *chuckles* (other announcer) shut up, that wasn't even funny.Umm... its Henry, sorry about that Henry.";


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

    public void DisplayPlayerTrapSetupSubtitle(int subtitle)
    {
        dialouge.text = dialougePlayerTrapSetup[subtitle];
    }

    public void DisplayPlayerTrapTriggerSubtitle(int subtitle)
    {
        dialouge.text = dialougePlayerTrapTrigger[subtitle];
    }

    public void DisplayDaveIntoSubtile(int subtitle)
    {
        dialouge.text = dialouge_DaveIntro[subtitle];
    }
    public void DisplayBigSchlugIntoSubtile(int subtitle)
    {
        dialouge.text = dialouge_BigSchlugIntro[subtitle];
    }
    public void DisplayHHHIntoSubtile(int subtitle)
    {
        dialouge.text = dialouge_HHHIntro[subtitle];
    }
    //NEED TO ADD MOSCO LINES


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
