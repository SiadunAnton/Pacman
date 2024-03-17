using UnityEngine.Tilemaps;
using UnityEngine;
using Zenject;

public class TilemapInstaller : MonoInstaller
{
    [SerializeField] private Tilemap _level;

    public override void InstallBindings()
    {
        Container.Bind<Tilemap>().WithId("level").FromInstance(_level).AsSingle();
    }
}