using UnityEngine;
using System.Collections;
using System;

public class Destructable : MonoBehaviour
{
    public Grid RegisteredGrid;
    public AudioClip DestructionSoundFX;
    public float MaxRandomAudioOffset = 0.15f;

    private Animator anim;
    private int animDestroy = Animator.StringToHash("Explode");
    private int animRefresh = Animator.StringToHash("Refresh");
    private int animFinished = Animator.StringToHash("Finished");

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    public void StartDestruction()
    {
        StartCoroutine(WaitForAnimation());
        StartCoroutine(WaitForRandomOffsetWithAudio());
    }

    private IEnumerator WaitForRandomOffsetWithAudio()
    {
        float randomOffset = UnityEngine.Random.Range(0, MaxRandomAudioOffset);

        yield return new WaitForSeconds(randomOffset);

        AudioSource.PlayClipAtPoint(DestructionSoundFX, transform.position);
    }

    private IEnumerator WaitForAnimation()
    {
        anim.SetTrigger(animDestroy);
        

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        while(info.shortNameHash != animFinished)
        {
            info = anim.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        CleanUp();
    }

    public void CleanUp()
    {
        anim.SetTrigger(animRefresh);
        

        // unregister from grid
        if (RegisteredGrid != null)
            RegisteredGrid.RemoveMe(GetComponent<GridElement>());

        // delete
        GameObject.Destroy(this.gameObject);
    }

}
