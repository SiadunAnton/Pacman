using UnityEngine;
using Zenject;

public class SoundEffectsInstaller : MonoInstaller
{
    [SerializeField] private SoundEffects _soundEffects;

    public override void InstallBindings()
    {
        Container.Bind<SoundEffects>().FromInstance(_soundEffects).AsSingle();
    }
}