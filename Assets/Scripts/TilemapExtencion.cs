using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapExtencion 
{
    public static bool AreOnTheSameTile(this Tilemap map, GameObject obj1, GameObject obj2)
    {
        return map.WorldToCell(obj1.transform.position) == map.WorldToCell(obj2.transform.position);
    }
}
