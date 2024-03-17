using Zenject;

public class EventProviderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<EventProvider>().FromNew().AsSingle();
    }
}