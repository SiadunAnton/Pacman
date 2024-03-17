using UnityEngine;

public abstract class CustomUpdatable : MonoBehaviour
{
    public float Speed = 0.3f;
    public abstract void CustomUpdate();
}
