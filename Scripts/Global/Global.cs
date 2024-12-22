using System.Collections.Generic;
using System.Net;
using UnityEngine;
using GlobalClasses;
public class Global : MonoBehaviour
{
    public static Global Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateInstance()
    {
        if (Instance == null)
        {
            GameObject globalObject = new GameObject("GlobalManager");
            Instance = globalObject.AddComponent<Global>();
            DontDestroyOnLoad(globalObject);
        }

        // Initialize Players before any scene is loaded
        Instance.Initialize();
    }

    public List<Eon> gEons = new List<Eon>();
    public List<Echo> gEchos = new List<Echo>();
    public List<Item> gItems = new List<Item>();
    public List<Quest> gQuests = new List<Quest>();
    public List<Player> gPlayers = new List<Player>();
    public Inventory gInventory = new Inventory();
    public List<Dialogue> gNpcDialogues = new List<Dialogue>();
    public Dictionary<string, DialogueNpc> npcDialoguesById = new Dictionary<string, DialogueNpc>();


    // Initialize the lists before any scene is loaded
    private void Initialize()
    {
        // Initialize Echos
        gEchos = new List<Echo>
        {
            new Echo("Echo1", "Function1"),
            new Echo("Echo2", "Function2"),
            new Echo("Echo3", "Function3"),
            new Echo("Echo4", "Function4"),
            new Echo("Echo5", "Function5"),
            new Echo("Echo6", "Function6"),
            new Echo("Echo7", "Function7"),
            new Echo("Echo8", "Function8"),
            new Echo("Echo9", "Function9"),
            new Echo("Echo10", "Function10"),
            new Echo("Echo11", "Function11"),
            new Echo("Echo12", "Function12"),
            new Echo("Echo13", "Function13"),
            new Echo("Echo14", "Function14"),
            new Echo("Echo15", "Function15"),
            new Echo("Echo16", "Function16"),
            new Echo("Echo17", "Function17"),
            new Echo("Echo18", "Function18"),
            new Echo("Echo19", "Function19"),
            new Echo("Echo20", "Function20")
        };

        // Initialize Eons
        gEons = new List<Eon>
        {
            new Eon("Eon1", "Function1"),
            new Eon("Eon2", "Function2"),
            new Eon("Eon3", "Function3"),
            new Eon("Eon4", "Function4"),
            new Eon("Eon5", "Function5"),
            new Eon("Eon6", "Function6"),
            new Eon("Eon7", "Function7"),
            new Eon("Eon8", "Function8"),
            new Eon("Eon9", "Function9"),
            new Eon("Eon10", "Function10"),
            new Eon("Eon11", "Function11"),
            new Eon("Eon12", "Function12"),
            new Eon("Eon13", "Function13"),
            new Eon("Eon14", "Function14"),
            new Eon("Eon15", "Function15"),
            new Eon("Eon16", "Function16"),
            new Eon("Eon17", "Function17"),
            new Eon("Eon18", "Function18"),
            new Eon("Eon19", "Function19"),
            new Eon("Eon20", "Function20")
        };

        // Initialize Items (40 items with numeric IDs)
        gItems = new List<Item>
        {
            new Item("Health Potion", "1", "health_potion.png", "Restores 50 health points.", "UseHealthPotion"),
            new Item("Mana Potion", "2", "mana_potion.png", "Restores 50 mana points.", "UseManaPotion"),
            new Item("Fireball Scroll", "3", "fireball_scroll.png", "Cast a fireball spell.", "UseFireball"),
            new Item("Ice Wand", "4", "ice_wand.png", "Freezes enemies in place for a short time.", "UseIceWand"),
            new Item("Healing Herb", "5", "healing_herb.png", "Restores 25 health points over time.", "UseHealingHerb"),
            new Item("Elixir of Strength", "6", "elixir_strength.png", "Increases strength by 10 for 5 minutes.", "UseStrengthElixir"),
            new Item("Silver Sword", "7", "silver_sword.png", "A sword forged from silver, effective against undead.", "EquipSilverSword"),
            new Item("Gold Shield", "8", "gold_shield.png", "A shield made of gold, increases defense by 20.", "EquipGoldShield"),
            new Item("Magic Crystal", "9", "magic_crystal.png", "A crystal that boosts spell power.", "UseMagicCrystal"),
            new Item("Teleportation Stone", "10", "teleportation_stone.png", "Teleports you to a known location.", "UseTeleportStone"),
            new Item("Thunderbolt Staff", "11", "thunderbolt_staff.png", "Summons a bolt of lightning to strike enemies.", "UseThunderboltStaff"),
            new Item("Emerald Amulet", "12", "emerald_amulet.png", "Increases resistance to magic attacks.", "EquipEmeraldAmulet"),
            new Item("Vampire Fang", "13", "vampire_fang.png", "A rare item that drains health from enemies.", "UseVampireFang"),
            new Item("Dragon Scale", "14", "dragon_scale.png", "A scale from an ancient dragon, can be used in crafting.", "UseDragonScale"),
            new Item("Mystic Rune", "15", "mystic_rune.png", "A rune that can be used to enhance spells.", "UseMysticRune"),
            new Item("Frostbite Dagger", "16", "frostbite_dagger.png", "A dagger that deals frost damage.", "EquipFrostbiteDagger"),
            new Item("Elven Bow", "17", "elven_bow.png", "A longbow made by elves, deals precision damage.", "EquipElvenBow"),
            new Item("Silver Ingot", "18", "silver_ingot.png", "A piece of pure silver, can be used for crafting.", "UseSilverIngot"),
            new Item("Golden Necklace", "19", "golden_necklace.png", "Increases charisma and resistance to charm.", "EquipGoldenNecklace"),
            new Item("Boots of Speed", "20", "boots_of_speed.png", "Increases movement speed by 20%.", "EquipBootsOfSpeed"),
            new Item("Phoenix Feather", "21", "phoenix_feather.png", "Can be used to revive a player once.", "UsePhoenixFeather"),
            new Item("Potion of Invisibility", "22", "potion_invisibility.png", "Grants invisibility for 30 seconds.", "UseInvisibilityPotion"),
            new Item("Sunstone", "23", "sunstone.png", "A stone that can heal 100 health points when used.", "UseSunstone"),
            new Item("Moonstone", "24", "moonstone.png", "A stone that regenerates 50 mana points when used.", "UseMoonstone"),
            new Item("Elixir of Wisdom", "25", "elixir_wisdom.png", "Increases intelligence by 10 for 30 minutes.", "UseWisdomElixir"),
            new Item("Necromancer's Staff", "26", "necromancers_staff.png", "A staff that raises the dead to fight for you.", "EquipNecromancersStaff"),
            new Item("Knight's Armor", "27", "knights_armor.png", "An armor that increases defense by 50.", "EquipKnightsArmor"),
            new Item("Berserker's Axe", "28", "berserkers_axe.png", "An axe that increases damage when health is low.", "EquipBerserkersAxe"),
            new Item("Starlight Gem", "29", "starlight_gem.png", "A gem that increases the potency of healing spells.", "UseStarlightGem"),
            new Item("Ice Crystal", "30", "ice_crystal.png", "A rare crystal that freezes enemies in place.", "UseIceCrystal"),
            new Item("Fire Essence", "31", "fire_essence.png", "An essence that can be used to summon a fire elemental.", "UseFireEssence"),
            new Item("Thunderstone", "32", "thunderstone.png", "A stone that summons lightning in a storm.", "UseThunderstone"),
            new Item("Dark Soulstone", "33", "dark_soulstone.png", "An artifact that absorbs souls to power dark magic.", "UseDarkSoulstone"),
            new Item("Cloak of Shadows", "34", "cloak_of_shadows.png", "A cloak that grants you stealth and evasion.", "EquipCloakOfShadows"),
            new Item("Runed Hammer", "35", "runed_hammer.png", "A magical hammer that can break through most defenses.", "EquipRunedHammer"),
            new Item("Dragon's Heart", "36", "dragons_heart.png", "A rare item that increases health regeneration by 50%.", "UseDragonsHeart"),
            new Item("Ring of Power", "37", "ring_of_power.png", "A ring that grants +5 to all attributes.", "EquipRingOfPower"),
            new Item("Witch's Brew", "38", "witchs_brew.png", "A potion that grants temporary resistance to poison.", "UseWitchsBrew"),
            new Item("Treasure Map", "39", "treasure_map.png", "Leads to a hidden treasure if followed.", "UseTreasureMap"),
            new Item("Golden Key", "40", "golden_key.png", "A key to open a mysterious chest.", "UseGoldenKey")
        };

        // Initialize Quests (15 Total: 12 Levels, 1 Tutorial, 1 Prologue, 1 Lore Beginning)
        gQuests = new List<Quest>
        {
            new Quest(1, "The Rift Incident (Prologue)", "A dark Rift has appeared on a battlefield, it is your duty to investigate and close the Rift before the world is torn apart.", "The Rift", "rift_incident_image"),
            new Quest(2, "Entering A New World", "Explore a world distorted by temporal forces. Discover the new threats and secrets hidden deep within.", "A New Fractured World", "new_world_image"),
            new Quest(3, "The Beginning", "Familiarize yourself with the environment. Start your journey through the rocky paths and learn the basics of survival.", "The Rocky Red Pathway", "tutorial_image"),
            new Quest(4, "Origins", "Venture through a place marked by echoes of the past, uncover the mysteries of this world’s creation.", "Lake Of Rebirth", "origins_image"),
            new Quest(5, "Encounters", "Explore a forgotten site, where the remnants of ancient battles are etched into the land. Be cautious of lurking dangers.", "Ruins of The Heroes' Graveyard", "encounters_image"),
            new Quest(6, "Mystery of the Lost City", "A lost city lies beyond the desert. It’s said that unimaginable riches and secrets lie within, but so do deadly traps.", "Lost City of Eloria", "lost_city_image"),
            new Quest(7, "The Guardian's Trial", "A powerful guardian blocks the path to a mystical temple. Only those who prove their strength can enter.", "Temple of the Guardian", "guardian_trial_image"),
            new Quest(8, "Betrayal", "Someone within your ranks has betrayed you. Discover the truth and stop the betrayal before it's too late.", "The Shattered Trust", "betrayal_image"),
            new Quest(9, "The Dark Forest", "The Dark Forest is said to be cursed, filled with creatures that defy the laws of nature. Find the truth behind its curse.", "The Cursed Forest", "dark_forest_image"),
            new Quest(10, "The Dragon's Lair", "A powerful dragon has taken residence in the mountains. Prepare yourself for the ultimate battle and claim its hoard.", "The Dragon's Den", "dragons_lair_image"),
            new Quest(11, "A Time for Heroes", "The world needs new heroes to rise and face the challenges ahead. Join forces with others and prove your worth.", "The Hero’s Call", "heroes_image"),
            new Quest(12, "Shadows Over the Kingdom", "Dark forces have taken over the kingdom. The royal family is in hiding, and you must defeat the invaders.", "Royal Palace", "shadows_kingdom_image"),
            new Quest(13, "The Eternal Night", "An ancient vampire has awoken, threatening to plunge the world into eternal darkness. Put an end to his reign.", "Vampire’s Castle", "eternal_night_image"),
            new Quest(14, "The Siege of Stormhold", "Stormhold is under siege by an army of monstrous creatures. Defend the castle at all costs.", "Stormhold Fortress", "siege_stormhold_image"),
            new Quest(15, "The Lost Heir", "A young prince has gone missing, and only you can uncover the truth. Where did he go, and why?", "The Forgotten Kingdom", "lost_heir_image"),
            new Quest(16, "Forbidden Magic", "Dark magic is on the rise again. Discover who is behind it and stop them before the world is consumed by chaos.", "The Forbidden Lab", "forbidden_magic_image"),
            new Quest(17, "The Hunt for the Oracle", "An oracle holds the key to defeating an ancient evil. Find her and uncover the prophecy.", "Oracle’s Temple", "hunt_oracle_image"),
            new Quest(18, "Revenge of the Fallen", "The fallen heroes from the past have returned as powerful undead warriors. Defeat them to restore peace.", "The Graveyard of Heroes", "revenge_fallen_image"),
            new Quest(19, "The War of the Gods", "The gods are waging war against each other, and the mortal world is caught in the crossfire. Choose your side carefully.", "The Battlefields of Eternity", "war_gods_image"),
            new Quest(20, "The Final Stand", "The end is near. The ultimate enemy is here, and only you can stop them. Gather allies and prepare for the final battle.", "The Last Battle", "final_stand_image")
        };

        // Initialize Players
        gPlayers = new List<Player>
        {
            new Player("1", 100),
            new Player("2", 100)
        };

        // Assign Echos and Eons to each player
        foreach (var player in gPlayers)
        {
            player.Echos = new List<Echo>
        {
            gEchos[0],
            gEchos[1],
            gEchos[2],
            gEchos[3],
        };

            player.Eons = new List<Eon>
        {
            gEons[0],
            gEons[1],
            gEons[2],
            gEons[3],
        };
        }

        // Initialize Inventory
        gInventory.slots = new List<InventorySlot>();
        for (int i = 0; i < 27; i++)
        {
            if (i != 24)
            {
                // Ensure the Id is an integer, and then pass the correct parameters to the constructor
                gInventory.slots.Add(new InventorySlot(i + 1, gItems[Random.Range(0, gItems.Count)], Random.Range(1, 100)));
            }
            else
            {
                gInventory.slots.Add(new InventorySlot(i + 1, null, 0)); // For empty slot
            }
        }

        // Initialize NpcDialogues
        gNpcDialogues = LoadDialoguesFromJSON("Data/NPCConversations.json");

        // Populate the dictionary for quick lookup
        foreach (var dialogue in gNpcDialogues)
        {
            foreach (var npc in dialogue.Root)
            {
                npcDialoguesById[npc.NpcId] = npc;
            }
        }
    }

    public static List<Dialogue> LoadDialoguesFromJSON(string relativePath)
    {
        List<Dialogue> dialogues = new List<Dialogue>();

        try
        {
            string fullPath = System.IO.Path.Combine(Application.dataPath, relativePath);
            string jsonText = System.IO.File.ReadAllText(fullPath);
            Dialogue dialogue = new Dialogue(jsonText);
            dialogues.Add(dialogue);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load dialogues from JSON: {ex.Message} at path: {relativePath}");
        }

        return dialogues;
    }


}
