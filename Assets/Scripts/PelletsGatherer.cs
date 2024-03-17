using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class PelletsGatherer : CustomUpdatable
{
    [SerializeField] private GameObject _pelletsHolder;
    [SerializeField] private Tilemap _level;
    [SerializeField] private GameObject _pacman;

    private Transform[] _pellets;
    private EventProvider _provider;
    private bool _pelletsAreOut = false;

    [Inject]
    public void Initialize(EventProvider provider)
    {
        _provider = provider;
    }

    private void Awake()
    {
        _pellets = _pelletsHolder.GetComponentsInChildren<Transform>();
        _provider.Register("OnPelletsAreOut");
    }

    public override void CustomUpdate()
    {
        if (_pelletsAreOut)
            return;

        if (_pellets.Length == 0)
        {
            _pelletsAreOut = true;
            _provider.Invoke("OnPelletsAreOut");
        }

        foreach(var pellet in _pellets)
        {
            if (pellet != null && _level.WorldToCell(_pacman.transform.position) == _level.WorldToCell(pellet.position))
            {
                Destroy(pellet.gameObject);
                Score.Value += 10;
            }
        }
    }
}
