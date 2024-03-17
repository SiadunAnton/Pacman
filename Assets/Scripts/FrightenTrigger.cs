using System.Collections;
using UnityEngine;

public class FrightenTrigger : MonoBehaviour
{
    public bool isActive => !_invoked;

    private SpriteRenderer _renderer;
    private bool _invoked = false;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Hide() => StartCoroutine(HideForTime());

    IEnumerator HideForTime()
    {
        _invoked = true;
        _renderer.enabled = false;
        yield return new WaitForSeconds(40f);
        _invoked = false;
        _renderer.enabled = true;
    }
}