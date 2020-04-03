using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnnouncerDialouge : MonoBehaviour
{

    public Text subtitleText;
    

    public string[] dialougePlayerOnPlayerCollision;
    public string[] dialougePlayerShot;
    public string[] dialougePlayerTrapSetup;
    public string[] dialougePlayerTrapTrigger;

    public string[] dialouge_WelcomeToTheCosmodome;

    [Header("Character Introductions")]
    public string[] dialouge_DaveIntro;
    public string[] dialouge_BigSchlugIntro;
    public string[] dialouge_HHHIntro;
    public string[] dialouge_ElMosoIntro;

    [Header("End of round lines")]
    public string[] dialouge_EndOfRoundBanter;
    public string[] dialouge_DaveOut;
    public string[] dialouge_HHHOut;
    public string[] dialouge_MoscoOut;
    public string[] dialouge_SchlugOut;

    [Header("Dave Collision Lines")]
    public string[] dialouge_Coll_Dave_VS_Mosco;
    public string[] dialouge_Coll_Dave_VS_HHH;
    public string[] dialouge_Coll_Dave_VS_Schlug;
    [Header("Big Schlug Collision Lines")]
    public string[] dialouge_Coll_Schlug_VS_Dave;
    public string[] dialouge_Coll_Schlug_VS_Mosco;
    public string[] dialouge_Coll_Schlug_VS_HHH;
    [Header("Mosco Collision Lines")]
    public string[] dialouge_Coll_Mosco_VS_Dave;
    public string[] dialouge_Coll_Mosco_VS_Schlug;
    public string[] dialouge_Coll_Mosco_VS_HHH;
    [Header("HHH Collision Lines")]
    public string[] dialouge_Coll_HHH_VS_Dave;
    public string[] dialouge_Coll_HHH_VS_Mosco;
    public string[] dialouge_Coll_HHH_VS_Schlug;









    Animator _animator;
    AudioEvent _audioEvent;
    bool _subtitlesAreShowing = false;
    Coroutine _hideSubtitlesCoroutine;
    Coroutine _animateInSubtitlesCoroutine;
    string _queuedSubtitle = "";
    bool _isDramatic = false;
    float _queuedTime = 0;
    bool _shouldAnimateNextSubtitle = false;

    /// <summary>
    /// Show a subtitle for the specified amount of time, replacing any existing subtitle
    /// </summary>
    /// <param name="subtitle">The subtitle to show</param>
    /// <param name="time">The time to show it for</param>
    /// <param name="dramatic">True for cutscenes where it's ticked in, false for more subtle in game version</param>
    void ShowSubtitle(string subtitle, float time, bool dramatic) { 
        if (_subtitlesAreShowing) {
            // We're already showing subtitles, swap them out
            StopCoroutine(_hideSubtitlesCoroutine);

            if (_animateInSubtitlesCoroutine != null) {
                StopCoroutine(_animateInSubtitlesCoroutine);
                _animateInSubtitlesCoroutine = null;
            }

             if (dramatic) _animator.Play("Swap", -1, 0);
             else _animator.Play("Swap ingame", -1, 0);
        } else {
            // No subs showing right now, so play appear animation
            if (dramatic) _animator.Play("Appear");
            else _animator.Play("Appear ingame");
        }

        _isDramatic = dramatic;

        _subtitlesAreShowing = true;

        // We will set the subtitle once the animation calls the event
        _queuedSubtitle = subtitle;
        _queuedTime = time;
        if (dramatic) _queuedTime -= 2; // make the text animate in a bit faster than the talking

        _shouldAnimateNextSubtitle = dramatic;

        // And hide the subtitles after the given time
        _hideSubtitlesCoroutine = StartCoroutine(HideSubtitlesWhenDone(time + 1));
    }

    public void CancelSubtitles() {
        if (!_subtitlesAreShowing) return;

        if (_animateInSubtitlesCoroutine != null) {
            StopCoroutine(_animateInSubtitlesCoroutine);
            _animateInSubtitlesCoroutine = null;
        }

        if (_hideSubtitlesCoroutine != null) {
            StopCoroutine(_hideSubtitlesCoroutine);
            _hideSubtitlesCoroutine = null;
        }


        if (_isDramatic) _animator.Play("Dissapear");
        else _animator.Play("Dissapear ingame");
        _queuedSubtitle = "";
        _queuedTime = 0;
        _subtitlesAreShowing = false;
    }

    /// <summary>
    /// Called by animator event when we are ready to change the subtitle text
    /// </summary>
    public void Animator_ChangeSubtitle() {
        if (_queuedSubtitle != "") {
            subtitleText.text = "";
            if (_shouldAnimateNextSubtitle) {
                _animateInSubtitlesCoroutine = StartCoroutine(AnimateInSubtitles(_queuedSubtitle, _queuedTime - 1f)); // minus -1 show the subtitles stay on screen for a second
            } else {
                subtitleText.text = _queuedSubtitle;
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            }
        } else {
            subtitleText.text = "";
        }
    }

    /// <summary>
    /// Slowly tick the subtitles in
    /// </summary>
    /// <param name="subtitle">The subtitle to show</param>
    /// <param name="time">The total time to tick them in</param>
    /// <returns></returns>
    IEnumerator AnimateInSubtitles(string subtitle, float time) {
        RectTransform trns = GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(trns);

        float timeBetweenCharacters = time / subtitle.Length;

        for (int i = 0; i < subtitle.Length; i++) {
            subtitleText.text += subtitle[i];
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }

    IEnumerator HideSubtitlesWhenDone(float time) {
        yield return new WaitForSeconds(time);

        CancelSubtitles();
    }

    // Start is called before the first frame update
    void Start()
    {

        _animator = GetComponent<Animator>();
        _audioEvent = GameObject.Find("Announcer").GetComponent<AudioEvent>();
        dialougePlayerShot = new string[11];
        dialougePlayerOnPlayerCollision = new string[24];
        dialougePlayerTrapSetup = new string[5];
        dialougePlayerTrapTrigger = new string[16];

        dialouge_WelcomeToTheCosmodome = new string[10];
        dialouge_DaveIntro = new string[11];
        dialouge_BigSchlugIntro = new string[16];
        dialouge_HHHIntro = new string[10];
        dialouge_ElMosoIntro = new string[11];


        dialouge_EndOfRoundBanter = new string[27];
        dialouge_DaveOut = new string[13];
        dialouge_HHHOut = new string[9];
        dialouge_MoscoOut = new string[10];
        dialouge_SchlugOut = new string[14];

        dialouge_Coll_Dave_VS_HHH = new string[11];
        dialouge_Coll_Dave_VS_Mosco = new string[11];
        dialouge_Coll_Dave_VS_Schlug = new string[11];

        dialouge_Coll_Mosco_VS_Dave = new string[12];
        dialouge_Coll_Mosco_VS_Schlug = new string[12];
        dialouge_Coll_Mosco_VS_HHH = new string[12];

        dialouge_Coll_Schlug_VS_Dave = new string[11];
        dialouge_Coll_Schlug_VS_Mosco = new string[11];
        dialouge_Coll_Schlug_VS_HHH = new string[11];

        dialouge_Coll_HHH_VS_Dave = new string[9];
        dialouge_Coll_HHH_VS_Schlug = new string[9];
        dialouge_Coll_HHH_VS_Mosco = new string[9];



        InitLines();

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

        dialouge_Coll_Dave_VS_Mosco[0] = "<color=red>Dave</color> absolutely just smacked <color=green>El Mosco</color>";
        dialouge_Coll_Dave_VS_Mosco[1] = "Oh my god! I hope<color=green> El Mosco</color> is okay after what <color=red>Dave </color>just done to them!";
        dialouge_Coll_Dave_VS_Mosco[2] = "Oh! Look at <color=red>Dave </color>doin the crashy smashy with <color=green>El Mosco</color>";
        dialouge_Coll_Dave_VS_Mosco[3] = "There’s <color=red>Dave </color>using his fairground experience to bump <color=green>El Mosco</color> away!";
        dialouge_Coll_Dave_VS_Mosco[4] = "Hey <color=green>El Mosco</color>! <color=red>Dave</color> just made a fool of you, why arent you bumping him back?";
        dialouge_Coll_Dave_VS_Mosco[5] = "Ohhhh, <color=green>El Mosco</color> will need a new paintjob after what <color=red>Dave</color> just did.";
        dialouge_Coll_Dave_VS_Mosco[6] = "If this was a game of pinball, then that hit between <color=red>Dave</color> and <color=green>El Mosco</color> would have got <color=red>Dave</color> some big points. But its not pinball.";
        dialouge_Coll_Dave_VS_Mosco[7] = "An absolutely great hit from <color=red>Dave</color>! I wonder if <color=green>El Mosco</color> will retaliate?";
        dialouge_Coll_Dave_VS_Mosco[8] = "I hope <color=green>El Moscos</color> ship can still run after that collision from <color=red>Dave</color>";
        dialouge_Coll_Dave_VS_Mosco[9] = "HEY! I just made a song for what just happened! <color=red>Dave</color> and <color=green>El Mosco</color> sittin' in some seats, C-R-A-S-H-I-N-G.";
        dialouge_Coll_Dave_VS_Mosco[10] = "Oh MY! A big ol, T-Bone from <color=red>Dave</color>, c'mon <color=green>El Mosco</color> you have to admit that was a good hit";

        dialouge_Coll_Dave_VS_HHH[0] = "<color=red>Dave</color> absolutely just smacked <color=blue>Henry</color>";
        dialouge_Coll_Dave_VS_HHH[1] = "Oh my god! I hope<color=blue> Henry</color> is okay after what <color=red>Dave </color>just done to them!";
        dialouge_Coll_Dave_VS_HHH[2] = "Oh! Look at <color=red>Dave </color>doin the crashy smashy with <color=blue>Hammerhead Henry</color>";
        dialouge_Coll_Dave_VS_HHH[3] = "There’s <color=red>Dave </color>using his fairground experience to bump <color=blue>Henry</color> away!";
        dialouge_Coll_Dave_VS_HHH[4] = "Hey <color=blue>Henry</color>! <color=red>Dave</color> just made a fool of you, why arent you bumping him back?";
        dialouge_Coll_Dave_VS_HHH[5] = "Ohhhh, <color=blue>Henry's</color> gonna need a new paintjob after what <color=red>Dave</color> just did.";
        dialouge_Coll_Dave_VS_HHH[6] = "If this was a game of pinball, then that hit between <color=red>Dave</color> and <color=blue>Henry</color> would have got <color=red>Dave</color> some big points. But its not pinball.";
        dialouge_Coll_Dave_VS_HHH[7] = "An absolutely great hit from <color=red>Dave</color>! I wonder if <color=blue>Henry</color> will retaliate?";
        dialouge_Coll_Dave_VS_HHH[8] = "I hope <color=blue>Henrys</color> ship can still run after that collision from <color=red>Dave</color>";
        dialouge_Coll_Dave_VS_HHH[9] = "HEY! I just made a song for what just happened! <color=red>Dave</color> and <color=blue>Henry</color> sittin' in some seats, C-R-A-S-H-I-N-G.";
        dialouge_Coll_Dave_VS_HHH[10] = "Oh MY! A big ol, T-Bone from <color=red>Dave</color>, c'mon <color=blue>Henry</color> you have to admit that was a good hit";

        dialouge_Coll_Dave_VS_Schlug[0] = "<color=red>Dave</color> absolutely just smacked <color=purple>Big Schlug</color>";
        dialouge_Coll_Dave_VS_Schlug[1] = "Oh my god! I hope<color=purple> Big Schlug</color> is okay after what <color=red>Dave </color>just done to them!";
        dialouge_Coll_Dave_VS_Schlug[2] = "Oh! Look at <color=red>Dave </color>doin the crashy smashy with <color=purple>Big Schlug</color>";
        dialouge_Coll_Dave_VS_Schlug[3] = "There’s <color=red>Dave </color>using his fairground experience to bump <color=purple>Big Schlug</color> away!";
        dialouge_Coll_Dave_VS_Schlug[4] = "Hey <color=purple>Big Schlug</color>! <color=red>Dave</color> just made a fool of you, why arent you bumping him back?";
        dialouge_Coll_Dave_VS_Schlug[5] = "Ohhhh, <color=purple>Big Schlug</color> will need a new paintjob after what <color=red>Dave</color> just did.";
        dialouge_Coll_Dave_VS_Schlug[6] = "If this was a game of pinball, then that hit between <color=red>Dave</color> and <color=purple>Big Schlug</color> would have got <color=red>Dave</color> some big points. But its not pinball.";
        dialouge_Coll_Dave_VS_Schlug[7] = "An absolutely great hit from <color=red>Dave</color>! I wonder if <color=purple>Big Schlug</color> will retaliate?";
        dialouge_Coll_Dave_VS_Schlug[8] = "I hope <color=purple>Big Schlugs</color> ship can still run after that collision from <color=red>Dave</color>";
        dialouge_Coll_Dave_VS_Schlug[9] = "HEY! I just made a song for what just happened! <color=red>Dave</color> and <color=purple>Big Schlug</color> sittin' in some seats, C-R-A-S-H-I-N-G.";
        dialouge_Coll_Dave_VS_Schlug[10] = "Oh MY! A big ol, T-Bone from <color=red>Dave</color>, c'mon <color=purple>Big Schlug</color> you have to admit that was a good hit";


        dialouge_Coll_Schlug_VS_Mosco[0] = "With a hit like that on <color=green>Mosco</color>, I’m surprised that <color=purple>Big schlugs</color> ship didn’t absorb them!";
        dialouge_Coll_Schlug_VS_Mosco[1] = "<color=purple>Schlug</color> comes in and absolutely obliterates <color=green>Mosco</color> with his signature move, Big Schmack.";
        dialouge_Coll_Schlug_VS_Mosco[2] = "OH NO! <color=green>Mosco</color>! Are you okay? Do you need a minute? <color=purple>Schlug</color>, stop playing so rough!";
        dialouge_Coll_Schlug_VS_Mosco[3] = "Oh! And <color=purple>Big Schlug</color> flattens <color=green>Mosco</color>!";
        dialouge_Coll_Schlug_VS_Mosco[4] = "OH metal on metal! That’s what <color=purple>Big Schlug</color> is all about! Hopefully <color=green>Mosco<color> doesn’t end up on the receiving end again!";
        dialouge_Coll_Schlug_VS_Mosco[5] = "Awww <color=green>Mosco</color>, It’s really hard not to get hit by <color=purple>Big Schlug</color> so dont feel bad.";
        dialouge_Coll_Schlug_VS_Mosco[6] = "<color=purple>Big schlug</color> coming in with a painful looking slam on <color=green>Mosco</color>";
        dialouge_Coll_Schlug_VS_Mosco[7] = "<color=purple>Big Schlug</color> smashing into <color=green>Mosco</color> like that is the reason why we’re currently in the process of making some sort of rule on weight class!";
        dialouge_Coll_Schlug_VS_Mosco[8] = "Oh and thats <color=green>Mosco</color> squashed. Watching <color=purple>Schlug</color> do his thing is always fun! Not for <color=green>Mosco</color> though.";
        dialouge_Coll_Schlug_VS_Mosco[9] = "With <color=gree>Mosco</color> getting crashed into like that I'm surprised they haven't learned to steer clear of <color=purple>Big Schlug</color>";
        dialouge_Coll_Schlug_VS_Mosco[10] = "<color=purple>Big Schlug</color> has it in for <color=green>Mosco</color>! C'mon man stop bullying him.";

        dialouge_Coll_Schlug_VS_Dave[0] = "With a hit like that on <color=red>Dave</color>, I’m surprised that <color=purple>Big schlugs</color> ship didn’t absorb them!";
        dialouge_Coll_Schlug_VS_Dave[1] = "<color=purple>Schlug</color> comes in and absolutely obliterates <color=red>Dave</color> with his signature move, Big Schmack.";
        dialouge_Coll_Schlug_VS_Dave[2] = "OH NO! <color=green>Mosco</color>! Are you okay? Do you need a minute? <color=purple>Schlug</color>, stop playing so rough!";
        dialouge_Coll_Schlug_VS_Dave[3] = "Oh! And <color=purple>Big Schlug</color> flattens <color=red>Dave</color>!";
        dialouge_Coll_Schlug_VS_Dave[4] = "OH metal on metal! That’s what <color=purple>Big Schlug</color> is all about! Hopefully <color=red>Dave<color> doesn’t end up on the receiving end again!";
        dialouge_Coll_Schlug_VS_Dave[5] = "Awww <color=red>Dave</color>, It’s really hard not to get hit by <color=purple>Big Schlug</color> so dont feel bad.";
        dialouge_Coll_Schlug_VS_Dave[6] = "<color=purple>Big schlug</color> coming in with a painful looking slam on <color=red>Dave</color>";
        dialouge_Coll_Schlug_VS_Dave[7] = "<color=purple>Big Schlug</color> smashing into <color=red>Dave</color> like that is the reason why we’re currently in the process of making some sort of rule on weight class!";
        dialouge_Coll_Schlug_VS_Dave[8] = "Oh and thats <color=red>Dave</color> squashed. Watching <color=purple>Schlug</color> do his thing is always fun! Not for <color=red>Dave</color> though.";
        dialouge_Coll_Schlug_VS_Dave[9] = "With <color=red>Dave</color> getting crashed into like that I'm surprised they haven't learned to steer clear of <color=purple>Big Schlug</color>";
        dialouge_Coll_Schlug_VS_Dave[10] = "<color=purple>Big Schlug</color> has it in for <color=red>Dave</color>! C'mon man stop bullying him.";

        dialouge_Coll_Schlug_VS_HHH[0] = "With a hit like that on <color=blue>Henry</color>, I’m surprised that <color=purple>Big schlugs</color> ship didn’t absorb them!";
        dialouge_Coll_Schlug_VS_HHH[1] = "<color=purple>Schlug</color> comes in and absolutely obliterates <color=blue>Henry</color> with his signature move , Big Schmack.";
        dialouge_Coll_Schlug_VS_HHH[2] = "OH NO! <color=blue>Henry</color>! Are you okay? Do you need a minute? <color=purple>Schlug</color>, stop playing so rough!";
        dialouge_Coll_Schlug_VS_HHH[3] = "Oh! And <color=purple>Big Schlug</color> flattens <color=blue>Henry</color>!";
        dialouge_Coll_Schlug_VS_HHH[4] = "OH metal on metal! That’s what <color=purple>Big Schlug</color> is all about! Hopefully <color=blue>Henry<color> doesn’t end up on the receiving end again!";
        dialouge_Coll_Schlug_VS_HHH[5] = "Awww <color=blue>Henry</color>, It’s really hard not to get hit by <color=purple>Big Schlug</color> so dont feel bad.";
        dialouge_Coll_Schlug_VS_HHH[6] = "<color=purple>Big schlug</color> coming in with a painful looking slam on <color=blue>Henry</color>";
        dialouge_Coll_Schlug_VS_HHH[7] = "<color=purple>Big Schlug</color> smashing into <color=blue>Henry</color> like that is the reason why we’re currently in the process of making some sort of rule on weight class!";
        dialouge_Coll_Schlug_VS_HHH[8] = "Oh and thats <color=blue>Henry</color> squashed. Watching <color=purple>Schlug</color> do his thing is always fun! Not for <color=blue>Henry</color> though.";
        dialouge_Coll_Schlug_VS_HHH[9] = "With <color=blue>Henry</color> getting crashed into like that I'm surprised they haven't learned to steer clear of <color=purple>Big Schlug</color>";
        dialouge_Coll_Schlug_VS_HHH[10] = "<color=purple>Big Schlug</color> has it in for <color=blue>Henry</color>! C'mon man stop bullying him.";



        dialouge_Coll_Mosco_VS_Dave[0] = "There’s <color=green>Mosco</color> doing what he does best, annoying <color=red>Dave</color>.";
        dialouge_Coll_Mosco_VS_Dave[2] = "And <color=red>Daves</color> been bumped hard! I hope they know that isn’t the last they’ll see of <color=green>Mosco</color>!";
        dialouge_Coll_Mosco_VS_Dave[3] = "<color=green>Mosco</color> going straight for <color=red>Dave</color>, like a cat on a caffeine buzz.";
        dialouge_Coll_Mosco_VS_Dave[4] = "Oh my! <color=green>Mosco</color> just laid <color=red>Dave</color> out, I would be embarrassed if I were them.";
        dialouge_Coll_Mosco_VS_Dave[5] = "<color=green>Mosco</color> flying straight into the face of <color=red>Dave</color>.";
        dialouge_Coll_Mosco_VS_Dave[6] = "I see <color=red>Dave's</color> gettin' the ol' one two off of <color=green>El Mosco</color>.";
        dialouge_Coll_Mosco_VS_Dave[7] = "OH! <color=green>Mosco</color> comes in with a quick hit on <color=red>Dave</color>.";
        dialouge_Coll_Mosco_VS_Dave[8] = "Quick in, quick out, just what <color=green>Mosco</color> does, bet that stung <color=red>Dave</color>!";
        dialouge_Coll_Mosco_VS_Dave[9] = "Oooo, and <color=green>Mosco</color> comes in and shows <color=red>Dave</color> who’s boss.";
        dialouge_Coll_Mosco_VS_Dave[10] = "Oh! <color=green>Mosco</color> takes a swipe at <color=red>Dave</color> and it connects!";
        dialouge_Coll_Mosco_VS_Dave[11] = "<color=green>Mosco</color> goes off the metaphorical top rope and slams into <color=red>Dave</color>!";

        dialouge_Coll_Mosco_VS_Schlug[0] = "There’s <color=green>Mosco</color> doing what he does best, annoying <color=purple>Big Schlug</color>.";
        dialouge_Coll_Mosco_VS_Schlug[2] = "And <color=purple>Big Schlugs</color> been bumped hard! I hope they know that isn’t the last they’ll see of <color=green>Mosco</color>!";
        dialouge_Coll_Mosco_VS_Schlug[3] = "<color=green>Mosco</color> going straight for <color=purple>Big Schlug</color>, like a cat on a caffeine buzz.";
        dialouge_Coll_Mosco_VS_Schlug[4] = "Oh my! <color=green>Mosco</color> just laid <color=purple>Big Schlug</color> out, I would be embarrassed if I were them.";
        dialouge_Coll_Mosco_VS_Schlug[5] = "<color=green>Mosco</color> flying straight into the face of <color=purple>Big Schlug</color>.";
        dialouge_Coll_Mosco_VS_Schlug[6] = "I see <color=purple>Big Schlug's</color> gettin' the ol' one two off of <color=green>El Mosco</color>.";
        dialouge_Coll_Mosco_VS_Schlug[7] = "OH! <color=green>Mosco</color> comes in with a quick hit on <color=purple>Big Schlug</color>.";
        dialouge_Coll_Mosco_VS_Schlug[8] = "Quick in, quick out, just what <color=green>Mosco</color> does, bet that stung <color=purple>Big Schlug</color>!";
        dialouge_Coll_Mosco_VS_Schlug[9] = "Oooo, and <color=green>Mosco</color> comes in and shows <color=purple>Big Schlug</color> who’s boss.";
        dialouge_Coll_Mosco_VS_Schlug[10] = "Oh! <color=green>Mosco</color> takes a swipe at <color=purple>Big Schlug</color> and it connects!";
        dialouge_Coll_Mosco_VS_Schlug[11] = "<color=green>Mosco</color> goes off the metaphorical top rope and slams into <color=purple>Big Schlug</color>!";

        dialouge_Coll_Mosco_VS_HHH[0] = "There’s <color=green>Mosco</color> doing what he does best, annoying <color=blue>Henry</color>.";
        dialouge_Coll_Mosco_VS_HHH[2] = "And <color=blue>Henrys</color> been bumped hard! I hope they know that isn’t the last they’ll see of <color=green>Mosco</color>!";
        dialouge_Coll_Mosco_VS_HHH[3] = "<color=green>Mosco</color> going straight for <color=blue>Henry</color>, like a cat on a caffeine buzz.";
        dialouge_Coll_Mosco_VS_HHH[4] = "Oh my! <color=green>Mosco</color> just laid <color=blue>Henry</color> out, I would be embarrassed if I were them.";
        dialouge_Coll_Mosco_VS_HHH[5] = "<color=green>Mosco</color> flying straight into the face of <color=blue>Henry</color>.";
        dialouge_Coll_Mosco_VS_HHH[6] = "I see <color=blue>Henry's</color> gettin' the ol' one two off of <color=green>El Mosco</color>.";
        dialouge_Coll_Mosco_VS_HHH[7] = "OH! <color=green>Mosco</color> comes in with a quick hit on <color=blue>Henry</color>.";
        dialouge_Coll_Mosco_VS_HHH[8] = "Quick in, quick out, just what <color=green>Mosco</color> does, bet that stung <color=blue>Henry</color>!";
        dialouge_Coll_Mosco_VS_HHH[9] = "Oooo, and <color=green>Mosco</color> comes in and shows <color=blue>Henry</color> who’s boss.";
        dialouge_Coll_Mosco_VS_HHH[10] = "Oh! <color=green>Mosco</color> takes a swipe at <color=blue>Henry</color> and it connects!";
        dialouge_Coll_Mosco_VS_HHH[11] = "<color=green>Mosco</color> goes off the metaphorical top rope and slams into <color=blue>Henry</color>!";


        dialouge_Coll_HHH_VS_Dave[0] = "And there’s <color=blue>Henry</color> bringing the hammer down on <color=red>Dave</color>!";
        dialouge_Coll_HHH_VS_Dave[1] = "<color=blue>Henry</color> comes up and bashes <color=red>Dave</color>!";
        dialouge_Coll_HHH_VS_Dave[2] = "<color=blue>Henry</color> coming out of nowhere and taking <color=red>Dave</color> out!";
        dialouge_Coll_HHH_VS_Dave[3] = "OH! And <color=red>Dave</color> get chomped by <color=blue>Henry</color>!";
        dialouge_Coll_HHH_VS_Dave[4] = "No pain no gain <color=red>Dave</color>! But there is no gain and just lots of pain because <color=blue>Henry</color> did it.";
        dialouge_Coll_HHH_VS_Dave[5] = "There’s <color=blue>Henry</color> using his predatory instincts to take a bite out of <color=red>Dave</color>.";
        dialouge_Coll_HHH_VS_Dave[6] = "Oh and thats <color=red>Dave</color> being spun out by <color=blue>Henry</color>, the big shark-scary thing!";
        dialouge_Coll_HHH_VS_Dave[7] = "Oh and <color=blue>Henry</color> goes and takes a big chunk out of <color=red>Dave</color>!";
        dialouge_Coll_HHH_VS_Dave[8] = "<color=blue>Henry's</color> taking all the risks hitting <color=red>Dave</color> like that!";

        dialouge_Coll_HHH_VS_Mosco[0] = "And there’s <color=blue>Henry</color> bringing the hammer down on <color=green>El Mosco</color>!";
        dialouge_Coll_HHH_VS_Mosco[1] = "<color=blue>Henry</color> comes up and bashes <color=green>El Mosco</color>!";
        dialouge_Coll_HHH_VS_Mosco[2] = "<color=blue>Henry</color> coming out of nowhere and taking <color=green>El Mosco</color> out!";
        dialouge_Coll_HHH_VS_Mosco[3] = "OH! And <color=green>El Mosco</color> get chomped by <color=blue>Henry</color>!";
        dialouge_Coll_HHH_VS_Mosco[4] = "No pain no gain <color=green>El Mosco</color>! But there is no gain and just lots of pain because <color=blue>Henry</color> did it.";
        dialouge_Coll_HHH_VS_Mosco[5] = "There’s <color=blue>Henry</color> using his predatory instincts to take a bite out of <color=green>El Mosco</color>.";
        dialouge_Coll_HHH_VS_Mosco[6] = "Oh and thats <color=green>El Mosco</color> being spun out by <color=blue>Henry</color>, the big shark-scary thing!";
        dialouge_Coll_HHH_VS_Mosco[7] = "Oh and <color=blue>Henry</color> goes and takes a big chunk out of <color=green>El Mosco</color>!";
        dialouge_Coll_HHH_VS_Mosco[8] = "<color=blue>Henry's</color> taking all the risks hitting <color=green>El Mosco</color> like that!";

        dialouge_Coll_HHH_VS_Schlug[0] = "And there’s <color=blue>Henry</color> bringing the hammer down on <color=purple>Big Schlug</color>!";
        dialouge_Coll_HHH_VS_Schlug[1] = "<color=blue>Henry</color> comes up and bashes <color=purple>Big Schlug</color>!";
        dialouge_Coll_HHH_VS_Schlug[2] = "<color=blue>Henry</color> coming out of nowhere and taking <color=purple>Big Schlug</color> out!";
        dialouge_Coll_HHH_VS_Schlug[3] = "OH! And <color=purple>Big Schlug</color> get chomped by <color=blue>Henry</color>!";
        dialouge_Coll_HHH_VS_Schlug[4] = "No pain no gain <color=purple>Big Schlug</color>! But there is no gain and just lots of pain because <color=blue>Henry</color> did it.";
        dialouge_Coll_HHH_VS_Schlug[5] = "There’s <color=blue>Henry</color> using his predatory instincts to take a bite out of <color=purple>Big Schlug</color>.";
        dialouge_Coll_HHH_VS_Schlug[6] = "Oh and thats <color=purple>Big Schlug</color> being spun out by <color=blue>Henry</color>, the big shark-scary thing!";
        dialouge_Coll_HHH_VS_Schlug[7] = "Oh and <color=blue>Henry</color> goes and takes a big chunk out of <color=purple>Big Schlug</color>!";
        dialouge_Coll_HHH_VS_Schlug[8] = "<color=blue>Henry's</color> taking all the risks hitting <color=purple>Big Schlug</color> like that!";



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

        dialouge_EndOfRoundBanter[0] = "C'mon, C'mon hurry up I want to see the next round! C'mon hurry. Hurry c'mon.";
        dialouge_EndOfRoundBanter[1] = "Oh-ho-ho, I don't know whats gonna happen but I bet it's gonna be cool! Next round!";
        dialouge_EndOfRoundBanter[2] = "Your guess as good as mine as to whats up next but... It's probably gonna be another round.";
        dialouge_EndOfRoundBanter[3] = "(to technician) Be honest with me am I doing a good job? I don't feel like I'm doing a good job. (notices mic is on) OH... Next round.";
        dialouge_EndOfRoundBanter[4] = "(to technician) I can't belive they paid that much for a ticket, front row? You're gonna die! (notices mic is on) OH... Next round.";
        dialouge_EndOfRoundBanter[5] = "Uhh, if there's any ladies in the crowd that uhh, want to see me later my number is 0-6-2-4-7 (tecnican pulls him away from mic)";
        dialouge_EndOfRoundBanter[6] = "And now a word from our mid-game sponsor... uhh corn... yum!";
        dialouge_EndOfRoundBanter[7] = "Who is gonna win, I don't know. All I wanna see is ships smashin'.";
        dialouge_EndOfRoundBanter[8] = "Two words! Don't go! p-please, aww that's three. It's gonna get good I swear!";
        dialouge_EndOfRoundBanter[9] = "(to technician) Am I getting paid for this? How much? ehhh- (to mic) Next round.";
        dialouge_EndOfRoundBanter[10] = "After the event I'll be doing a book signing for my new book: How to be bad at everything but still get a job.";
        dialouge_EndOfRoundBanter[11] = "Whats gonna happen next?... No really tell me 'cause I don't know!";
        dialouge_EndOfRoundBanter[12] = "(sad) If my mum and dad could see me now... It think they'd be gravely disappointed. Next round. Go.";
        dialouge_EndOfRoundBanter[13] = "Phew! What a game, I wonder who's gonna get put out next.";
        dialouge_EndOfRoundBanter[14] = "Oh it's really heating up now, I wonder who is gonna win? Or lose.";
        dialouge_EndOfRoundBanter[15] = "There can only be one winner folks, who's it gonna be?";
        dialouge_EndOfRoundBanter[16] = "Ha ha, point at the loser and laugh, laugh at the loser! Up next: is the next round obviously.";
        dialouge_EndOfRoundBanter[17] = "Now it's time for our mid-game sponsor! D-uhh... I forgot.";
        dialouge_EndOfRoundBanter[18] = "S-uhh, uhh... um. Time for... next round. Round time! Whoo! Let's go!";
        dialouge_EndOfRoundBanter[19] = "Ooo I wonder who's gonna win the game, probably that guy that I'm pointing at... yeaa.";
        dialouge_EndOfRoundBanter[20] = "Well you b-uhh, better buckle your seatbelts and get ready for the next round 'cause it's gonna be a wild one!";
        dialouge_EndOfRoundBanter[21] = "Buckle your seatbelts and hold onto your butts 'cause it's gonna be a wild next round!";
        dialouge_EndOfRoundBanter[22] = "(to technician) Hey where'd my sandwich go? Yea that's it there! (to mic) Oh-next round.";
        dialouge_EndOfRoundBanter[23] = "Buckle up cause next rounds gonna be crazy!";
        dialouge_EndOfRoundBanter[24] = "Who's gonna win? I don't know! I don't know what's going on! I don't even know who I am!";
        dialouge_EndOfRoundBanter[25] = "I don't know about you but I'm gonna have to lay down after this game!";
        dialouge_EndOfRoundBanter[26] = "*cough* OH, somebody farted in the commentator booth and it's horrible!";
        dialouge_EndOfRoundBanter[27] = "Am I doin a good job? Like, am I-I doin' okay? Cheer if I'm doin' a good job cause I don't know what's going on.";

        dialouge_DaveOut[0] = "And that's Dave the human out, human Dave...Dave human, eww.";
        dialouge_DaveOut[1] = "Gross human known as Dave is out... Ewwww, bye-bye!";
        dialouge_DaveOut[2] = "Oh! Bye-bye Dave bye-bye, you're not good, bye!";
        dialouge_DaveOut[3] = "Well! Looks like Dave's gonna be out this round!";
        dialouge_DaveOut[4] = "See you later Dave! You're out! Bye-bye!";
        dialouge_DaveOut[5] = "Oh! Looks like Dave's the one getting put out this round!";
        dialouge_DaveOut[6] = "And the crowd DOESN'T go wild for Dave!";
        dialouge_DaveOut[7] = "Well looks like you're sittin' this one out Dave!";
        dialouge_DaveOut[8] = "Oh, say bye-be to Dave everybody! Bye-bye!";
        dialouge_DaveOut[9] = "Oh it was close but Dave's out this round!";
        dialouge_DaveOut[10] = "And comin' in at last place is Dave!";
        dialouge_DaveOut[11] = "Gross human person is out, go away, bye-bye!";
        dialouge_DaveOut[12] = "And that's Dave the human gone, gross Dave, Dave human. Bye-bye!";
        dialouge_DaveOut[13] = "-Name's got four letters and he's out. It's Dave!";

        dialouge_HHHOut[0] = "Weird shark-thing is out... Henry bye.";
        dialouge_HHHOut[1] = "Henry's out of the game. Hammerhead Henry, fish guy.";
        dialouge_HHHOut[2] = "Annd Hammerhead Henry is out of the game. Sorry Henry.";
        dialouge_HHHOut[3] = "Annd Henry's out, bye Henry. You're out. You're bad.";
        dialouge_HHHOut[4] = "He's got fins and he's out! It's Henry! Bye Henry.";
        dialouge_HHHOut[5] = "And Henry's out, he's bad, you're bad. B-uhhh bye.";
        dialouge_HHHOut[6] = "Henry you're out. You're not a shark, you're a loser. Bye-bye.";
        dialouge_HHHOut[7] = "Aww and Henry's out. Out of the game. Sorry Henry.";
        dialouge_HHHOut[8] = "Smelly Henry's out, stinky Henry, gross Henry. You're out, bye.";
        dialouge_HHHOut[9] = "He smells and he's out. It's ! Smells like fish, 'cause he's a shark.";

        dialouge_MoscoOut[0] = "Don't hit your face on the door on your way out Mosco.";
        dialouge_MoscoOut[1] = "Oh! And Mosco's out!";
        dialouge_MoscoOut[2] = "And Mosco's outta the game!";
        dialouge_MoscoOut[3] = "Looks like you weren't fast enough Mosco 'cause you're out!";
        dialouge_MoscoOut[4] = "And Mosco's the one going out this round!";
        dialouge_MoscoOut[5] = "Mosquito Man's out. Big, scary mosquito man, bye-bye.";
        dialouge_MoscoOut[6] = "And Mosco's out of the game... sorry.";
        dialouge_MoscoOut[7] = "And Mosco's gonna take home nothing today 'cause he's out!";
        dialouge_MoscoOut[8] = "And the mosquito's out! Bye-bye El Mosco.";
        dialouge_MoscoOut[9] = "Look like you were too slow El Mosco! Bye-bye-bye.";
        dialouge_MoscoOut[10] = "That's you out Mosco. Bye-bye, sit down, bye-bye!";

        dialouge_SchlugOut[0] = "And it looks like Sclug's gonna be the one that's out.";
        dialouge_SchlugOut[1] = "Hate to say it but Sclug's out. Don't kill me Sclug please.";
        dialouge_SchlugOut[2] = "Big scary Sclug being's out, bye-bye Big Sclug.";
        dialouge_SchlugOut[3] = "And Sclug's outta here, sit down Sclug!";
        dialouge_SchlugOut[4] = "Put the kettle on 'cause Sclug'sout!";
        dialouge_SchlugOut[5] = "Slimey and gross lookin' thing's out. Bye Sclug!";
        dialouge_SchlugOut[6] = "It was close but Sclug's gone. Bye Sclug.";
        dialouge_SchlugOut[7] = "I'm gonna go into hinding now 'cause Sclug's out. He's out.";
        dialouge_SchlugOut[8] = "And Big Sclug's outta the game. I'm sorry Sclug don't beat me up.";
        dialouge_SchlugOut[9] = "He's slimey and he's a sore loser, It's Schlug, you're out!";
        dialouge_SchlugOut[10] = "This muscle has got you far, but not far enough. Schlug's out.";
        dialouge_SchlugOut[11] = "Hope your seats comfortable Schlug, 'cause you're gonna be watching everyone else. You're out.";
        dialouge_SchlugOut[12] = "Bye Schlug you're out. Bye-bye, bye you're bad.";
        dialouge_SchlugOut[13] = "Strong and muscly but also todays loser, it's Schlug. Bye-bye.";
        dialouge_SchlugOut[14] = "And Schlug's outta the game!";



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
        dialouge_BigSchlugIntro[10] = "Being the scariest and strongest is tough, when a sprinkle of salt can melt you. It's Big Schlug!";
        dialouge_BigSchlugIntro[11] = "Coming from a planet that mainly slides to get from one point to another its big schlug!";
        dialouge_BigSchlugIntro[12] = "Their entire species began when someone decided to put steroids into a garden slug, it’s big schlug!";
        dialouge_BigSchlugIntro[13] = "The bigger they are, the harder they slide away without a trace and come back when you least expect it, it’s big schlug!";
        dialouge_BigSchlugIntro[14] = "Being a vegetarian is tough when your packing same amount of muscle as a wild bear with a gym membership, its big schlug!";
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

        dialouge_ElMosoIntro[0] = "If you ever thought a fly buzzing was annoying, imagine how annoying a massive one is! Its El Mosco!";
        dialouge_ElMosoIntro[1] = "El Mosco is here! And at first you probably think he hails from Moscow, Russia, And you may be correct but I don’t even know to tell you the truth.";
        dialouge_ElMosoIntro[2] = "Is there a giant, flesh eating insect flying around in here? Oh no, it’s just our cuddly friend, El Mosco!";
        dialouge_ElMosoIntro[3] = "El Mosco is ready to buzz, win and buzz some more! He also enjoys oil paintings in his spare time.";
        dialouge_ElMosoIntro[4] = "Getting into your opponent's head is a good way to win in a competition, but a constant buzzing noise works just as well. Its El Mosco";
        dialouge_ElMosoIntro[5] = "A character of culture our El Mosco, he enjoys fine wine and annoying Dave and the other guys, but mostly Dave. ";
        dialouge_ElMosoIntro[6] = "Contrary to popular belief El Mosco doesn't suck blood, he actually sucks out brain juice which is far worse in my opinion.";
        dialouge_ElMosoIntro[7] = "After bug repellent spray was invented, he went into hiding and made a human repellent gun, it’s El Mosco!";
        dialouge_ElMosoIntro[8] = "Why have a spaceship when you can fly already? Maybe to show everyone how multi talented you are, its El Mosco!";
        dialouge_ElMosoIntro[9] = "Quick on his feet and hard to catch in a cup and put outside, its El Mosco!";
        dialouge_ElMosoIntro[10] = "Fast and efficient is the two words I'd use to describe El Mosco, but if I had to pick a third one I think it would have to be unsettling.";




    }



    public void DisplayPlayerShotSubtitle(int subtitle, float time)
    {
        ShowSubtitle(dialougePlayerShot[subtitle], time, false);
    }

    public void DisplayPlayerOnPlayerCollisionSubtitle(int subtitle, float time)
    {
        ShowSubtitle(dialougePlayerOnPlayerCollision[subtitle], time, false);
    }

    public void DisplayPlayerTrapSetupSubtitle(int subtitle, float time) 
    {
        ShowSubtitle(dialougePlayerTrapSetup[subtitle], time, false);
    }

    public void DisplayPlayerTrapTriggerSubtitle(int subtitle, float time)
    {
        ShowSubtitle(dialougePlayerTrapTrigger[subtitle], time, false);
    }

    public void DisplayDaveIntoSubtile(int subtitle, float time)
    {
        ShowSubtitle(dialouge_DaveIntro[subtitle], time, true);
    }

    public void DisplayBigSchlugIntoSubtile(int subtitle, float time)
    {
        ShowSubtitle(dialouge_BigSchlugIntro[subtitle], time, true);
    }

    public void DisplayHHHIntoSubtile(int subtitle, float time)
    {
        ShowSubtitle(dialouge_HHHIntro[subtitle], time, true);
    }
    public void DisplayMoscoIntoSubtile(int subtitle, float time)
    {
        ShowSubtitle(dialouge_ElMosoIntro[subtitle], time, true);
    }


    public void DisplayDaveCollisionSubtitle(string playerBeingHit, int randomLine, float time)
    {
        if(playerBeingHit == "Mosco")
        {
            ShowSubtitle(dialouge_Coll_Dave_VS_Mosco[randomLine], time, false);
        } 
        if(playerBeingHit == "Schlug")
        {
            ShowSubtitle(dialouge_Coll_Dave_VS_Schlug[randomLine], time, false);
        }
        if(playerBeingHit == "HHH")
        {
            ShowSubtitle(dialouge_Coll_Dave_VS_HHH[randomLine], time, false);
        }
    }

    public void DisplayMoscoCollisionSubtitle(string playerBeingHit, int randomLine, float time)
    {
        if (playerBeingHit == "Dave")
        {
            ShowSubtitle(dialouge_Coll_HHH_VS_Dave[randomLine], time, false);
        }
        if (playerBeingHit == "Schlug")
        {
            ShowSubtitle(dialouge_Coll_Mosco_VS_Schlug[randomLine], time, false);
        }
        if (playerBeingHit == "HHH")
        {
            ShowSubtitle(dialouge_Coll_Mosco_VS_HHH[randomLine], time, false);
        }
    }

    public void DisplaySchlugCollisionSubtitle(string playerBeingHit, int randomLine, float time)
    {
        
        if (playerBeingHit == "Dave")
        {
            ShowSubtitle(dialouge_Coll_Schlug_VS_Dave[randomLine], time, false);
        }
        if (playerBeingHit == "Mosco")
        {
            ShowSubtitle(dialouge_Coll_Schlug_VS_Mosco[randomLine], time, false);
        }
        if (playerBeingHit == "HHH")
        {
            ShowSubtitle(dialouge_Coll_Schlug_VS_HHH[randomLine], time, false);
        }
    }

    public void DisplayHHHCollisionSubtitle(string playerBeingHit, int randomLine, float time)
    {

        if (playerBeingHit == "Dave")
        {
            ShowSubtitle(dialouge_Coll_HHH_VS_Dave[randomLine], time, false);
        }
        if (playerBeingHit == "Mosco")
        {
            ShowSubtitle(dialouge_Coll_HHH_VS_Mosco[randomLine], time, false);
        }
        if (playerBeingHit == "Schlug")
        {
            ShowSubtitle(dialouge_Coll_HHH_VS_Schlug[randomLine], time, false);
        }
    }

}
