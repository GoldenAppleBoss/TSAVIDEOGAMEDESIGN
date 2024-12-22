using UnityEngine;
using TMPro;
using System.Collections.Generic;
using GlobalClasses;

public class InventoryState : IState
{
    private PlayerMenuState stateManager;
    private GameObject instantiatedInventoryUI; // Holds the instantiated Inventory UI
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
    private int inventoryIndexSelected = 0;

    public InventoryState(PlayerMenuState stateManager)
    {
        this.stateManager = stateManager;
    }

    public void OnEnter()
    {
        Debug.Log("Entered InventoryState");
        InitializeInventory();
    }

    public void OnExit()
    {
        if (instantiatedInventoryUI != null)
        {
            Object.Destroy(instantiatedInventoryUI);
            instantiatedInventoryUI = null;
        }
    }

    public void OnUpdate()
    {
        int moveX = 0;
        int moveY = 0;
        if (stateManager._controller.PlayerControls.ContainsKey("MenuUp") && Input.GetKeyDown(stateManager._controller.PlayerControls["MenuUp"]))
            moveY--;
        if (stateManager._controller.PlayerControls.ContainsKey("MenuDown") && Input.GetKeyDown(stateManager._controller.PlayerControls["MenuDown"]))
            moveY++;
        if (stateManager._controller.PlayerControls.ContainsKey("MenuLeft") && Input.GetKeyDown(stateManager._controller.PlayerControls["MenuLeft"]))
            moveX--;
        if (stateManager._controller.PlayerControls.ContainsKey("MenuRight") && Input.GetKeyDown(stateManager._controller.PlayerControls["MenuRight"]))
            moveX++;

        if (moveX != 0 || moveY != 0)
            Move(moveX, moveY);
    }

    private void InitializeInventory()
    {
        if (stateManager._InventoryUIPrefab == null)
        {
            Debug.LogError("_InventoryUIPrefab is not assigned in PlayerMenuState!");
            return;
        }

        // Instantiate the Inventory UI prefab
        instantiatedInventoryUI = Object.Instantiate(stateManager._InventoryUIPrefab);
        instantiatedInventoryUI.transform.SetParent(stateManager._controller.MenuUI.transform, false); // Optional: Set a parent transform if needed
        instantiatedInventoryUI.SetActive(true);

        // Find the InventorySlots container
        Transform inventorySlots = instantiatedInventoryUI.transform.Find("InventorySlots");
        if (inventorySlots == null)
        {
            Debug.LogError("InventorySlots GameObject not found in _InventoryUIPrefab!");
            return;
        }

        // Populate the inventory UI
        int index = 0;
        foreach (Transform panel in inventorySlots)
        {
            if (index >= stateManager.gInventory.slots.Count)
                break;

            var slot = stateManager.gInventory.slots[index];
            if (slot.item != null)
            {
                CreateInventorySlot(panel, slot, index);
            }
            index++;
        }

        HighlightSelectedSlot();
        ViewSelectedSlot();
    }

    private void CreateInventorySlot(Transform panel, InventorySlot slot, int index)
    {
        // Add a gray overlay if the slot contains an item
        GameObject grayPanel = new GameObject("GrayPanel");
        grayPanel.transform.SetParent(panel);
        grayPanel.transform.localPosition = Vector3.zero;
        grayPanel.transform.localScale = Vector3.one;

        RectTransform grayRect = grayPanel.AddComponent<RectTransform>();
        grayRect.anchorMin = Vector2.zero;
        grayRect.anchorMax = Vector2.one;
        grayRect.offsetMin = Vector2.zero;
        grayRect.offsetMax = Vector2.zero;

        UnityEngine.UI.Image grayImage = grayPanel.AddComponent<UnityEngine.UI.Image>();
        grayImage.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);

        // Add item count text
        GameObject textBox = new GameObject("ItemCountText");
        textBox.transform.SetParent(panel);
        textBox.transform.localPosition = new Vector3(0, -20, 0);
        textBox.transform.localScale = Vector3.one;

        RectTransform textRect = textBox.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0);
        textRect.anchorMax = new Vector2(0.5f, 0);
        textRect.pivot = new Vector2(0.5f, 0);
        textRect.anchoredPosition = new Vector2(0, -10);

        UnityEngine.UI.Text textComponent = textBox.AddComponent<UnityEngine.UI.Text>();
        textComponent.text = "x" + slot.Count;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.color = Color.black;
        textComponent.fontSize = 24;
        textComponent.alignment = TextAnchor.MiddleCenter;
    }

    private void HighlightSelectedSlot()
    {
        Transform inventorySlots = instantiatedInventoryUI.transform.Find("InventorySlots");
        if (inventorySlots == null)
        {
            Debug.LogError("InventorySlots GameObject not found in _InventoryUIPrefab!");
            return;
        }

        int index = 0;
        foreach (Transform panel in inventorySlots)
        {
            UnityEngine.UI.Image panelImage = panel.GetComponent<UnityEngine.UI.Image>();
            if (panelImage == null)
            {
                panelImage = panel.gameObject.AddComponent<UnityEngine.UI.Image>();
            }

            panelImage.color = (index == inventoryIndexSelected) ? Color.yellow : Color.white;
            index++;
        }
    }

    private void ViewSelectedSlot()
    {
        Transform viewingTransform = instantiatedInventoryUI.transform.Find("Viewing");
        if (viewingTransform == null)
        {
            Debug.LogError("Viewing GameObject not found in _InventoryUIPrefab!");
            return;
        }

        UnityEngine.UI.Image itemImage = viewingTransform.Find("ItemImage")?.GetComponent<UnityEngine.UI.Image>();
        TextMeshProUGUI itemName = viewingTransform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemDetails = viewingTransform.Find("ItemDetails")?.GetComponent<TextMeshProUGUI>();

        if (itemImage == null || itemName == null || itemDetails == null)
        {
            Debug.LogError("One or more child components are missing in Viewing!");
            return;
        }

        if (inventoryIndexSelected < 0 || inventoryIndexSelected >= stateManager.gInventory.slots.Count)
        {
            Debug.LogError("Selected index is out of bounds!");
            return;
        }

        var selectedSlot = stateManager.gInventory.slots[inventoryIndexSelected];
        if (selectedSlot.item == null)
        {
            Debug.LogWarning("Selected slot does not contain an item!");
            itemImage.sprite = null;
            itemImage.color = new Color(0, 0, 0, 0);
            itemName.text = "";
            itemDetails.text = "";
            return;
        }

        itemImage.sprite = LoadSpriteFromResources(selectedSlot.item.Image);
        itemImage.color = itemImage.sprite != null ? Color.white : new Color(0, 0, 0, 0);
        itemName.text = selectedSlot.item.Name;
        itemDetails.text = $"Count: {selectedSlot.Count}\n{selectedSlot.item.Description}";
    }

    private Sprite LoadSpriteFromResources(string path)
    {
        if (spriteCache.ContainsKey(path))
            return spriteCache[path];

        Sprite loadedSprite = Resources.Load<Sprite>(path);
        if (loadedSprite != null)
        {
            spriteCache[path] = loadedSprite;
        }
        return loadedSprite;
    }

    public void Move(int xMovement, int yMovement)
    {
        int totalMovement = xMovement + (yMovement * 9);
        int newIndex = inventoryIndexSelected + totalMovement;

        if (newIndex >= 0 && newIndex < stateManager.gInventory.slots.Count)
        {
            inventoryIndexSelected = newIndex;
            Debug.Log($"Selected slot index: {inventoryIndexSelected}");
        }
        HighlightSelectedSlot();
        ViewSelectedSlot();
    }
}
