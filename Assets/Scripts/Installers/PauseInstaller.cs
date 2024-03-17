using UnityEngine;
using Zenject;

public class PauseInstaller : MonoInstaller
{
    [SerializeField] private Pause _pause;

    public override void InstallBindings()
    {
        Container.Bind<Pause>().FromInstance(_pause).AsSingle();
    }
}