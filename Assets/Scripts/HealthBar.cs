using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Player player;
    RectTransform rectTransform;
    Slider slider;

    public void Initialize(Player player)
    {
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponent<Slider>();
        this.player = player;
        
    }
    public void SetPosition()
    {
        Vector2 healthBarPosition = new Vector2(500, 100);
        if (player.index == 0)
        {
            healthBarPosition.x = -1 * healthBarPosition.x;
        }
        rectTransform.anchoredPosition = healthBarPosition;
    }
    public void SetHP()
    {
        slider.maxValue = player.character.maxHP;
        
    }
    public void Refresh()
    {

    }
}
