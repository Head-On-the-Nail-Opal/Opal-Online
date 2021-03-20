using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescriptions : MonoBehaviour
{
    List<string> items = new List<string>();
    List<string> justNames = new List<string>();


    public void setUp()
    {
        items.Clear();
        justNames.Clear();
        items.Add("Defense Orb,Opal gains +4 defense at the start of the match but loses -1 defense whenever it takes damage.");
        items.Add("Jade Figure,When taking damage Opal gains +1 attack and defense if standing on a growth.");
        items.Add("Makeshift Shield,Start the game with 1 armor.");
        items.Add("Cursed Ring,Start the game poisoned.");
        items.Add("Heat-Proof Cloth,Burning damage stacks by 1 instead of 2.");
        items.Add("Lightweight Fluid,Heal +2 health at the start of each turn. If lifted then overheal.");
        items.Add("Cloak of Whispers,Gain +1 speed. Whenever you take damage lose -2 defense.");
        items.Add("Balloon of Light,Gain +1 speed while lifted");
        //items.Add("Metal Scrap,If you didn't move on your turn then gain +1 armor");
        items.Add("Death Wish,Lose -2 attack and defense.");
        items.Add("Insect Husk,Whenever you are burned instead you are poisoned, and vice versa");
        items.Add("Comfortable Padding,Whenever you gain armor also gain +2 attack. Whenever you lose armor lose -2 attack.");
        items.Add("Broken Doll,When taking damage from an ally gain +2 speed for 1 turn.");
        items.Add("Potion of Gratitude,After taking damage for the first time heal by 10 health.");
        items.Add("Mysterious Leaf,If standing on a growth at the end of your turn all adjacent Opals gain +1 attack and defense.");
        items.Add("Grieving Shrimp,When you die your teammates each gain +7 attack and defense for 1 turn.");
        items.Add("Taunting Mask,Gain +5 defense. When you take damage your attacker gains +2 health.");
        items.Add("Spectre Essence,When you would've died from an attack survive on 1 health once per game.");
        items.Add("Damp Machine,Gain +2 defense for ending your turn on a Flood");
        items.Add("Hovering Bandage,Gaining Lift will cure your other status conditions.");
        items.Add("Death's Skull,If an attack deals enough damage to kill you the attacker takes it too."); //Potential LEGENDARY
        items.Add("Golden Figure,Whenever you take damage heal by 1 health."); 
        items.Add("Azurite Figure, At the start of your turn place a Flood at your feet"); 
        items.Add("Garnet Figure, When you take damage the attacker's current tile turns to flame."); 
        items.Add("Jasper Figure, All incoming healing is overheal."); // Potential LEGENDARY
        items.Add("Juniper Necklace, At the start of the game gain +1 attack defense and speed for each void type ally."); //Potential LEGENDARY
        items.Add("Dripping Candle,At the start of your turn burn adjacent Opals."); 
        items.Add("Whetstone,When you end your turn next to a Boulder you and it gain +2 defense.");
        items.Add("Suffering Crown,When you take damage from an Opal with higher attack than you gain +5 attack."); //Potential LEGENDARY
        items.Sort();


        foreach (string s in items)
        {
            justNames.Add(s.Split(',')[0]);
        }
    }

    public string getItem(int num)
    {
        return items[num].Split(',')[0];
    }

    public string getDesc(int num)
    {
        return items[num].Split(',')[1];
    }

    public int itemsCount()
    {
        return items.Count;
    }

    public string getDescFromItem(string name)
    {
        return items[justNames.IndexOf(name)].Split(',')[1];
    }

    public string getRandomCharmName()
    {
        return justNames[Random.Range(0, justNames.Count)];
    }

}
