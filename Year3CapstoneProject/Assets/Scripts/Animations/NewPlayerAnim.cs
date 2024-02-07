using UnityEngine;

// Controls player animation. If fire button is pressed, plays sound and animation for attack, 
//  otherwise keeps playing the idle animation.

public class NewPlayerAnim : MonoBehaviour
{
    private Animation anim;
    // Use this for initialization
    void Start() 
    {
        anim = gameObject.GetComponent<Animation>();
    }
    // Update is called once per frame
    void Update()
    {
        PlayAnim2();


    }


    void PlayAnim2()
    {
        if (!GetComponent<Animation>().IsPlaying("Death") && !GetComponent<Animation>().IsPlaying("Roll") && !GetComponent<Animation>().IsPlaying("Dash"))
        {
            if (!Input.anyKey)
            {
                anim.Play("Idle");
            }
            else if(Input.GetKey("q"))
            {
                anim.Play("Roll");
            }
            else if (Input.GetKey("f"))
            {
                anim.Play("Dash");
            }
            else if (Input.GetKey("e"))
            {
                anim.Play("Shoot");
            }
            else if ((Input.GetKey("w")))
            {
                anim.Play("Walk");
            }
            else if (!GetComponent<Animation>().IsPlaying("Walk") && !GetComponent<Animation>().IsPlaying("Shoot") && !GetComponent<Animation>().IsPlaying("Roll") && !GetComponent<Animation>().IsPlaying("Hurt") && !GetComponent<Animation>().IsPlaying("Heal") && !GetComponent<Animation>().IsPlaying("Boot") && !GetComponent<Animation>().IsPlaying("Dash") && !GetComponent<Animation>().IsPlaying("Death"))
            {
                anim.Play("Idle");
            }
        }
    }


}

