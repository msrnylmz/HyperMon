using UnityEngine;

[CreateAssetMenu(menuName = "Create Monster Card")]
public class MonsterCardObject : MonsterObject 
{
    public string Type;
    public Color32 Color;
    public int Price;
    public Sprite Sprite;
}
