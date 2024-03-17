using UnityEngine;

public class DirectionAnimator : CustomUpdatable
{
    private Animator _animator;
    private IDirectable _directable;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _directable = GetComponent<IDirectable>();
    }

    public override void CustomUpdate()
    {
        if(_directable.Direction == Vector3Int.up)
        {
            _animator.SetTrigger("up");
        }
        else if(_directable.Direction == Vector3Int.left)
        {
            _animator.SetTrigger("left");
        }
        else if(_directable.Direction == Vector3Int.down)
        {
            _animator.SetTrigger("down");
        }
        else if(_directable.Direction == Vector3Int.right)
        {
            _animator.SetTrigger("right");
        }
    }
}
