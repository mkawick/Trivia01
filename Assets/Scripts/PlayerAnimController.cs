using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class PlayerAnimController : MonoBehaviour
{
    protected Animator animator;

    [SerializeField]
    float timeInIdle = 10.0f, varianceInIdle = 3.0f;
    [SerializeField]
    float timeInNonIdle = 0.75f, varianceNonIdle = 0.25f;

    float timeUntilIdleEnds = 0;
    float timeUntilNonIdleEnds = 0;

    enum BasicState
    {
        Idle,
        NonIdle
    }
    enum AnimationPlay
    {
        Run, 
        Walk,
        Wave,
        PanicIdle,
        Twitch,
        Talk,
        Throw,
        Interact,
        Idle
    }

    BasicState animState = BasicState.Idle;
    private void Start()
    {
        animator = GetComponent<Animator>();
        timeUntilIdleEnds = Time.time + timeInIdle;
    }
    void PlayAnim(AnimationPlay clip)
    {
        if (animator == null)
            return;

        switch (clip)
        {

            case AnimationPlay.Run:
                animator.Play("Run");
                break;
            case AnimationPlay.Walk:
                animator.Play("Walk");
                break;
            case AnimationPlay.Wave:
                animator.Play("Wave");
                break;
            case AnimationPlay.PanicIdle:
                animator.Play("PanicIdle");
                break;
            case AnimationPlay.Twitch:
                animator.Play("Twitch");
                break;
            case AnimationPlay.Talk:
                animator.Play("Talk");
                break;
            case AnimationPlay.Throw:
                animator.Play("Throw");
                break;
            case AnimationPlay.Interact:
                animator.Play("Interact");
                break;
            case AnimationPlay.Idle:
                animator.Play("Idle");
                break;
        }
    }

    public void Celebrate()
    {
        if(UnityEngine.Random.Range(0, 2) != 0)
            PlayAnim(AnimationPlay.Throw);
        else 
            PlayAnim(AnimationPlay.Wave);
    }

    private void Update()
    {
        switch(animState)
        {
            case BasicState.Idle:
                if (timeUntilIdleEnds < Time.time)
                {
                    ChoseNextAnim();
                }
                break;
            case BasicState.NonIdle:
                if (timeUntilNonIdleEnds < Time.time)
                {
                    ReturnToIdle();
                }
                break;
        }
      /*  if (timeUntilStateChange < Time.time)
        {
            ChoseNextAnim();
        }
        else
        {
            PlayAnim(AnimationPlay.Idle);
        }*/
    }

    public void ChoseNextAnim()
    {
        timeUntilNonIdleEnds = Time.time + timeInNonIdle + UnityEngine.Random.Range(-varianceNonIdle, varianceNonIdle);
        if (timeUntilNonIdleEnds < Time.time)
            timeUntilNonIdleEnds = Time.time + 0.5f;

        AnimationPlay randomBar;
        do
        {
            Array values = Enum.GetValues(typeof(AnimationPlay));
            int choice = (int)(UnityEngine.Random.value * (float)values.Length);
            randomBar = (AnimationPlay)values.GetValue(choice);

        } while (randomBar == AnimationPlay.Idle);
        PlayAnim(randomBar);
        animState = BasicState.NonIdle;
    }
    public void ReturnToIdle()
    {
        PlayAnim(AnimationPlay.Idle);
        timeUntilIdleEnds = Time.time + timeInIdle + UnityEngine.Random.Range(-varianceInIdle, varianceInIdle);
        animState = BasicState.Idle;
    }
}
