using System;
using UnityEngine;

public class HeartState : MonoBehaviour
{
    private Animator _animator;

    [NonSerialized] public bool isGone = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_animator)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Destroy"))
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void BreakHeart()
    {
        _animator.SetTrigger("BreakHeart");
    }

    public void SplitHeart()
    {
        _animator.SetTrigger("SplitHeart");
    }
}
