using UnityEngine;

public class AnimatorSwitcher : MonoBehaviour
{
    [SerializeField] protected RuntimeAnimatorController frightenedController;
    [SerializeField] protected RuntimeAnimatorController deadController;

    private RuntimeAnimatorController _defaultAnimController;
    private Animator _animator;
    private Ghost _ghost;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _ghost = GetComponent<Ghost>();
    }

    private void Update()
    {
        if (_ghost.IsFrightened && _ghost.enabled && !_ghost.IsDead)
            SetFrightenedController();
    }

    private void Start()
    {
        _defaultAnimController = _animator.runtimeAnimatorController;

        _ghost.OnDead += SetDeadController;
        _ghost.OnPrevious += SetDefaultController;
    }

    public void SetDeadController()
    {
        _animator.runtimeAnimatorController = deadController;
    }

    public void SetFrightenedController()
    {
        if(_ghost.enabled)
            _animator.runtimeAnimatorController = frightenedController;
    }

    public void SetDefaultController()
    {
        _animator.runtimeAnimatorController = _defaultAnimController;
    }
}
