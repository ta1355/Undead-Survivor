using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public int level;

    public Weapon weapon;

    public Gear gear;

    Image icon;

    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = itemData.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = itemData.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (itemData.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(itemData.itemDesc, itemData.damages[level]* 100, itemData.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(itemData.itemDesc, itemData.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(itemData.itemDesc);
                break;
        }

    }


    public void onClick()
{
    switch (itemData.itemType)
    {
        case ItemData.ItemType.Melee:
        case ItemData.ItemType.Range:
            float nextDamage = itemData.baseDamage;
            int nextCount = 0;

            for (int i = 0; i <= level; i++)
            {
                if (i < itemData.counts.Length)
                    nextCount += itemData.counts[i];
            }
            nextCount += itemData.baseCount; // baseCount 더해주기
            nextDamage += itemData.baseDamage * itemData.damages[level];

            if (level == 0)
            {
                GameObject newWeapon = new GameObject();
                weapon = newWeapon.AddComponent<Weapon>();
                weapon.Init(itemData);
            }
            weapon.LevelUp(nextDamage, nextCount);
            level++;
            break;

        case ItemData.ItemType.Glove:
        case ItemData.ItemType.Shoe:
            if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(itemData);
                }else
                {
                    float nextRate = itemData.damages[level];
                    gear.LevelUp(nextRate);    
                }
            level++;
            break;
        case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
            break;
    }

    if (level == itemData.damages.Length)
    {
        GetComponent<Button>().interactable = false;
    }
    
}

}
