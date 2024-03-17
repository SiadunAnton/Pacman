using UnityEngine;
using Zenject;

public class PacmanInstaller : MonoInstaller
{
    [SerializeField] private Pacman _pacman;

    public override void InstallBindings()
    {
        Container.Bind<Pacman>().FromInstance(_pacman).AsSingle();
    }
}