using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescriptions : MonoBehaviour
{
    List<string> items = new List<string>();
    List<string> justNames = new List<string>();


    public void setUp()
    {
        items.Add("Defense Orb,Opal gains +6 defense at the start of the match but loses -2 defense whenever it attacks.");
        items.Add("Jade Figure,When taking damage Opal gains +1 attack and defense if standing on a growth.");
        items.Add("Makeshift Shield,Start the game with 1 armor.");
        items.Add("Cursed Ring,Start the game poisoned.");
        items.Add("Heat-Proof Cloth,Burning damage stacks by 1 instead of 2.");
        items.Add("Lightweight Fluid,Heal +1 health at the end of each turn. If lifted then overheal.");
        items.Add("Raging Totem,Gain +2 speed. Take 6 damage if you make a full move.");
        items.Add("Balloon of Light,Gain +1 speed while lifted");
        items.Add("Metal Scrap,If you didn't move on your turn then gain +1 armor");
        items.Add("Death Wish,Lose -2 attack and defense.");
        items.Add("Cleaving Blade,Gain +4 attack when killing an enemy Opal");
        items.Add("Insect Husk,Whenever you are burned instead you are poisoned, and vice versa.");
        items.Add("Puzzle Piece,Can be chosen multiple times per team. If all Opals on the team carry a puzzle piece at the start of a turn, they all gain +5 defense");
        items.Add("Comfortable Padding,Whenever you gain armor also gain +2 attack. Whenever you lose armor lose -2 attack.");
        items.Add("Broken Doll,When taking damage from an ally, gain +1 speed for 1 turn.");
        items.Add("Potion of Gratitude,After taking damage for the first time, heal by 10 health.");
        items.Add("Mysterious Leaf,If standing on a growth at the end of your turn all nearby Opals gain +1 attack and defense.");
        items.Add("Grieving Shrimp,When you die your teammates each gain +7 attack and defense for 1 turn.");

        foreach(string s in items)
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

}
