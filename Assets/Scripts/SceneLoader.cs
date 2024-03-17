using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader : MonoBehaviour
{
    private EventProvider _provider;

    [Inject]
    public void Initialize(EventProvider provider)
    {
        _provider = provider;
    }

    private void Start()
    {
        _provider.Subscribe("OnPacmanDie",() => ReloadCurrentAfter(1.5f));
        _provider.Subscribe("OnPelletsAreOut", () => ReloadCurrentAfter(1.5f));
    }

    public void ReloadCurrentAfter(float gap)
    {
        StartCoroutine(ReloadProcess(gap));
    }

    IEnumerator ReloadProcess(float gap)
    {
        yield return new WaitForSeconds(gap);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
