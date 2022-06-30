using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonMonsterCard : MonoBehaviour
{
    public int ID;
    public Button ButtonMonster;

    [SerializeField] private Image m_MonsterImage;
    [SerializeField] private Image m_BGImage;

    [SerializeField] private TextMeshProUGUI m_MonsterPower;

    public void SetButtonCard(Sprite sprite, int power, Color32 color)
    {
        m_MonsterImage.sprite = sprite;
        m_MonsterPower.text = power.ToString();
        m_BGImage.color = new Color32(color.r, color.g, color.b, (byte)175);
    }
}
