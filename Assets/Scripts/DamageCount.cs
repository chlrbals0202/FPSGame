using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DamageCount : MonoBehaviour
{
    public static DamageCount instance;

    //인스펙터에서 countText UI 오브젝트를 연결
    public TextMeshProUGUI countText;

    int totalDamage = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (countText == null)
        {
            Debug.LogError("DamageCount: countText가 연결되지 않았습니다. 인스펙터에서 연결해주세요.");
            return;
        }

        countText.text = "데미지 : 0";
    }

    //적에게 피해를 줄 때 호출 — 누적 피해량 갱신
    public void AddDamage(int amount)
    {
        totalDamage += amount;
        if (countText != null)
            countText.text = "데미지 : " + totalDamage;
    }
}
