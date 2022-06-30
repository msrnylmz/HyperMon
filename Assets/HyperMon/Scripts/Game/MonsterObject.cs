using UnityEngine;

[CreateAssetMenu(menuName = "Create Monster")]

public class MonsterObject : ScriptableObject
{
    public string Name;
    public int Power;
    public GameObject Prefab;
}
