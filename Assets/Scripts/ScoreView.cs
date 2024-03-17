using UnityEngine.UI;
using UnityEngine;

public class ScoreView : CustomUpdatable
{
    [SerializeField] private Text _view;

    private void Start()
    {
        Score.Value = 0;
    }

    public override void CustomUpdate()
    {
        _view.text = Score.Value.ToString();
    }
}
