using UnityEngine;
using Zenject;

public class GhostsInstaller : MonoInstaller
{
    [SerializeField] private Ghost _shadow;
    [SerializeField] private Ghost _speedy;
    [SerializeField] private Ghost _bashful;
    [SerializeField] private Ghost _pokey;

    public override void InstallBindings()
    {
        Ghost[] allGhosts = {_shadow, _speedy, _bashful, _pokey};
        Container.Bind<Ghost[]>().WithId("all").FromInstance(allGhosts);

        Ghost[] threeGhosts = { _speedy, _bashful, _pokey };
        Container.Bind<Ghost[]>().WithId("3last").FromInstance(threeGhosts).AsSingle();

        Container.Bind<Ghost>().WithId("shadow").FromInstance(_shadow);
        Container.Bind<Ghost>().WithId("speedy").FromInstance(_speedy);
        Container.Bind<Ghost>().WithId("bashful").FromInstance(_bashful);
        Container.Bind<Ghost>().WithId("pokey").FromInstance(_pokey);
    }
}