using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public ItemSlot[] itemSlots;
    public List<TextMeshProUGUI> textLabels;
    public GameObject dialogueBox;
    public GameObject Shop;
    private Queue<string> sentences;
    private bool dialogueStarted;

    [Header("Player Status UI")]
    public GameObject playerStat;
    public GameObject radiationBar;
    public GameObject inventoryUI;

    [Header("Event Listener")]
    public SceneLoadEventSO loadEvent;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sentences = new Queue<string>();
        dialogueStarted = false;
    }

    public int AddNewItem(ItemData item)
    {
        int index = 0;
        while(index < itemSlots.Length && itemSlots[index].countText.text != "")
        {
            index++;
        }
        if(index >= itemSlots.Length)
        {
            Debug.Log("no space in inventory UI for new item");
            return -1;
        }
        ItemSlot slot = itemSlots[index];
        slot.AddItem(item);
        return index;
    }

    public void SetItemCount(ItemData item)
    {
        ItemSlot slot = Array.Find(itemSlots, element => element.item.itemName == item.itemName);
        if(slot != null)
        {
            if(item.count <= 0)
                slot.ClearItem();
            else
                slot.countText.text = item.count.ToString();
        }
    }

    private void OnEnable() 
    {
        loadEvent.loadRequestEvent += OnLoadEvent;
    }

    private void OnDisable() 
    {
        loadEvent.loadRequestEvent -= OnLoadEvent;
    }

    private void OnLoadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        // if cur scene is menu, hide ui
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        playerStat.SetActive(!isMenu);
        radiationBar.SetActive(!isMenu);
        inventoryUI.SetActive(!isMenu);
        
    }

    public void SetText(string textLabelName, string text)
    {
        TextMeshProUGUI textLabel = textLabels.Find(x => x.name == textLabelName);
        if (textLabel != null)
        {
            textLabel.text = text;
        } else
        {
            Debug.Log("TextLabel: " + textLabelName + " not found.");
        }
    }

    public void LoadDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        dialogueBox.SetActive(true);
    }

    public void NextDialogue(Dialogue dialogue)
    {   
        // Debug.Log("next sentence.");
        if(!dialogueStarted)
        {
            LoadDialogue(dialogue);
            dialogueStarted = true;
        }

        if(sentences.Count == 0) 
        {
            // Debug.Log("Dialogue over");
            // AudioManager.Instance.ToggleSFX();
            dialogueStarted = false;
            dialogueBox.SetActive(false);
            return;
        }

        string sentence = sentences.Dequeue();
        if(sentence == "Take a look! Left Click to buy!")
        {
            OpenShop();
            dialogueBox.SetActive(false);
        }
        SetText("Dialogue", sentence);
    }

    public void OpenShop()
    {
        AudioManager.Instance.PlayMusic("click", false);
        Shop.SetActive(true);
    }

    public void CloseShop()
    {
        AudioManager.Instance.PlayMusic("click", false);
        Shop.SetActive(false);
    }
}
