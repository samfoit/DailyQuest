using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject lootBag;

    public string[] itemName;
    public int[] itemQuantity;
    public Item[] refItems;
    public string[] dropItemNames;

    public bool isTalking, isLooting, isInventory, isShopping = false;

    public GameObject iceEffect;
    public GameObject fireEffect;

    public GameObject movementJoystick;
    public GameObject attackJoystick;

    public bool hasHealthPotion;
    public bool hasManaPotion;

    public float expFromQuests;

    public int deaths = 0;
    public float time = 0;

    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (PlayerPrefs.HasKey("QUEST_EXP"))
        {
            expFromQuests = PlayerPrefs.GetFloat("QUEST_EXP");
        }
        if (PlayerPrefs.HasKey("ITEM_NAME_0"))
        {
            LoadInventory();
        }
        if (PlayerPrefs.HasKey("TIME"))
        {
            time = PlayerPrefs.GetFloat("TIME");
        }
        if (PlayerPrefs.HasKey("DEATHS"))
        {
            deaths = PlayerPrefs.GetInt("DEATHS");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dropItemNames = new string[refItems.Length];
        SortItems();
        for (int i = 0; i < refItems.Length; i++)
        {
            dropItemNames[i] = refItems[i].itemName;
        }
        CheckForPotions();
    }

    private void Update()
    {
        time += Time.unscaledDeltaTime;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetFloat("TIME", time);
        }
    }

    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < itemName.Length; i++)
        {
            if (itemName[i] == "" || itemName[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemName.Length;
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < refItems.Length; i++)
            {
                if (refItems[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    i = refItems.Length;
                }
            }

            if (itemExists)
            {
                itemName[newItemPosition] = itemToAdd;
                itemQuantity[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " Does Not Exist");
            }
        }
        GameMenu.instance.ShowItems();
        SortItems();
        AddMoney(itemToAdd);
        CheckForPotions();
        SaveInventory();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for (int i = 0; i < itemName.Length; i++)
        {
            if (itemName[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = itemName.Length;
            }
        }

        if (foundItem)
        {
            itemQuantity[itemPosition]--;

            if (itemQuantity[itemPosition] <= 0)
            {
                itemName[itemPosition] = "";
                GameMenu.instance.activeItem = null;
            }

            SortItems();
            GameMenu.instance.ShowItems();
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
        SaveInventory();
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for (int i = 0; i < itemName.Length - 1; i++)
            {
                if (itemName[i] == "")
                {
                    itemName[i] = itemName[i + 1];
                    itemName[i + 1] = "";

                    itemQuantity[i] = itemQuantity[i + 1];
                    itemQuantity[i + 1] = 0;

                    if (itemName[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for (int i = 0; i < refItems.Length; i++)
        {
            if (refItems[i].itemName == itemToGrab)
            {
                return refItems[i];
            }
        }
        return null;
    }

    public int GetItemQuantity(string itemToFind)
    {
        for (int i = 0; i < itemName.Length; i++)
        {
            if (itemName[i] == itemToFind)
            {
                return itemQuantity[i];
            }
        }

        return 0;
    }

    public void DropLootChance(Transform position)
    {
        int chance = 2;
        int roll = Random.Range(1, 11);

        if (chance >= roll)
        {
            SpawnLoot(position);
        }
    }

    public void SpawnLoot(Transform transform)
    {
        GameObject loot = Instantiate(lootBag);
        loot.transform.position = transform.position;

        loot.GetComponent<LootBag>().SetLootDrops(dropItemNames);
        LootMenu.instance.lootBags.Add(loot);
    }

    public void SetDropItems(string[] drops)
    {
        dropItemNames = new string[drops.Length];
        for (int i = 0; i < drops.Length; i++)
        {
            dropItemNames[i] = drops[i];
        }
    }

    public void TutorialLoot(Transform transform)
    {
        GameObject loot = Instantiate(lootBag);
        loot.transform.position = transform.position;

        loot.GetComponent<LootBag>().GuaranteedLoot(dropItemNames);
        LootMenu.instance.lootBags.Add(loot);
    }

    public void UpdateDeaths()
    {
        deaths++;
        PlayerPrefs.SetInt("DEATHS", deaths);
    }

    public void ClearPlayerRecords()
    {
        deaths = 0;
        time = 0;
        PlayerPrefs.SetInt("DEATHS", deaths);
        PlayerPrefs.SetFloat("TIME", time);
    }


    /// <summary>
    /// Instantiates ice image and unstuns the object over a set period of time
    /// </summary>
    /// <param name="enemy">Enemy's position to spawn the ice</param>
    /// <param name="lifeTime">How long to freeze</param>
    /// <returns></returns>
    public IEnumerator Freeze(Transform enemy, float lifeTime)
    {
        GameObject ice = Instantiate(iceEffect, Vector2.zero, Quaternion.identity);
        ice.transform.parent = enemy;
        ice.transform.localPosition = Vector2.zero;

        yield return new WaitForSeconds(lifeTime);
        if (ice != null)
        {
            Destroy(ice);
        }
        if (enemy.gameObject != null)
        {
            if (enemy.gameObject.GetComponent<Enemy>() != null)
            {
                enemy.gameObject.GetComponent<Enemy>().UnStun();
            }
            if (enemy.gameObject.GetComponent<PlayerStats>() != null)
            {
                PlayerController.instance.stun = false;
            }
        }
    }

    /// <summary>
    /// Instantiates fire image for a set period of time
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="lifeTime"></param>
    /// <returns></returns>
    public IEnumerator Burn(Transform enemy, float lifeTime)
    {
        GameObject fire = Instantiate(fireEffect, Vector2.zero, Quaternion.identity);
        fire.transform.parent = enemy;
        fire.transform.localPosition = Vector2.zero;

        yield return new WaitForSeconds(lifeTime);
        if (fire != null)
        {
            Destroy(fire);
        }
    }

    private void AddMoney(string item)
    {
        for (int i = 0; i < refItems.Length; i++)
        {
            if (item == refItems[i].itemName && refItems[i].isCurrency)
            {
                PlayerStats.instance.money += refItems[i].value;
                RemoveItem(item);
            }
        }
    }

    public void ClearItems()
    {
        for (int i = 0; i < itemName.Length; i++)
        {
            itemName[i] = "";
            itemQuantity[i] = 0;
        }

        SaveInventory();
    }

    public void DeactivateJoysticks()
    {
        PlayerController.instance.stun = true;
        PlayerController.instance.StopMovement();
        Image[] spritesA = attackJoystick.GetComponentsInChildren<Image>();
        Image[] spritesM = movementJoystick.GetComponentsInChildren<Image>();
        for (int i = 0; i < spritesA.Length; i++)
        {
            spritesA[i].color = new Color(spritesA[i].color.r, spritesA[i].color.g, spritesA[i].color.b, 0f);
        }
        for (int i = 0; i < spritesM.Length; i++)
        {
            spritesM[i].color = new Color(spritesM[i].color.r, spritesM[i].color.g, spritesM[i].color.b, 0f);
        }
    }

    public void ActivateJoysticks()
    {
        Image[] spritesA = attackJoystick.GetComponentsInChildren<Image>();
        Image[] spritesM = movementJoystick.GetComponentsInChildren<Image>();
        for (int i = 0; i < spritesA.Length; i++)
        {
            if (i == 1)
            {
                spritesA[i].color = new Color(spritesA[i].color.r, spritesA[i].color.g, spritesA[i].color.b, 0.745f);
                continue;
            }
            spritesA[i].color = new Color(spritesA[i].color.r, spritesA[i].color.g, spritesA[i].color.b, 1f);
        }
        for (int i = 0; i < spritesM.Length; i++)
        {
            if (i == 1)
            {
                spritesM[i].color = new Color(spritesM[i].color.r, spritesM[i].color.g, spritesM[i].color.b, 0.745f);
                continue;
            }
            spritesM[i].color = new Color(spritesM[i].color.r, spritesM[i].color.g, spritesM[i].color.b, 1f);
        }
        PlayerController.instance.stun = false;
    }

    public void CheckForPotions()
    {
        CheckForHealthPotions();
        CheckForManaPotions();
    }

    private void CheckForHealthPotions()
    {
        for (int i = 0; i < itemName.Length; i++)
        {
            if (itemName[i] == "Health Potion")
            {
                hasHealthPotion = true;
                return;
            }
            hasHealthPotion = false;
        }
    }

    private void CheckForManaPotions()
    {
        for (int i = 0; i < itemName.Length; i++)
        {
            if (itemName[i] == "Mana Potion" || itemName[i] == "Big Mana Potion")
            {
                hasManaPotion = true;
                return;
            }
            hasManaPotion = false;
        }
    }

    public void SaveInventory()
    {
        for (int i = 0; i < itemName.Length; i++)
        {
            PlayerPrefs.SetString("ITEM_NAME_" + i, itemName[i]);
            PlayerPrefs.SetInt("ITEM_QUANTITY_" + i, itemQuantity[i]);
        }
        PlayerStats.instance.SaveStats();
    }

    private void LoadInventory()
    {
        for (int i = 0; i < itemName.Length; i++)
        {
            itemName[i] = PlayerPrefs.GetString("ITEM_NAME_" + i);
            itemQuantity[i] = PlayerPrefs.GetInt("ITEM_QUANTITY_" + i);
        }
    }

    public void GodRoll()
    {
        string[] items = { "Iron Sword", "Fire Sword", "Lethal Poison Dagger", "Ice Sword", "Lightning Spear",
            "Ring of Fire", "Ring of Frost", "Book of Healing", "Book of Strength","Book of Poison", "Book of Lifesteal"};
        AddItem(items[Random.Range(0, items.Length)]);
        SaveInventory();
    }

    public void RandomSpell()
    {
        string[] items = { "Ring of Fire", "Ring of Frost", "Book of Healing", "Book of Strength", "Book of Poison", "Book of Lifesteal" };
        AddItem(items[Random.Range(0, items.Length)]);
        SaveInventory();
    }
}
