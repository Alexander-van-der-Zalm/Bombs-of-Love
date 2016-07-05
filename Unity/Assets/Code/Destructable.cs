using UnityEngine;
using System.Collections;
using System;

public class Destructable : MonoBehaviour
{
    public Grid RegisteredGrid;

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
