using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vocabulary : MonoBehaviour
{
    Dictionary<string, string> vocab = new Dictionary<string, string>();

    public void setIt()
    {
        vocab.Add("poison", "A status effect which deals damage and debuffs the victim -1 attack and -1 defense per turn.");
        vocab.Add("burn", "A status effect which deals damage and increases in damage every turn.");
        vocab.Add("poisoned", "A status effect which deals damage and debuffs the victim -1 attack and -1 defense per turn.");
        vocab.Add("burned", "A status effect which deals damage and increases in damage every turn.");
        vocab.Add("lift", "A status effect allows Opals to float over tiles they move over, but causes them to be pushed further.");
        vocab.Add("armor", "Each armor will deflect damage from an ability once, then break.");
        vocab.Add("charge", "An extra stat electric types build that powers their abilities.");
        vocab.Add("overheal", "Healing that may heal the user above their max HP.");
        vocab.Add("water-rush", "Water rush attacks have their range extended by Flood tiles");
        vocab.Add("flame", "Fire type terrain, traversal causes burning.");
        vocab.Add("flood", "Water type terrain.");
        vocab.Add("growth", "Grass type terrain, Opals gain +2 attack and +2 defense while in Growth.");
        vocab.Add("miasma", "Plague type terrain, traversal causes poisoning, Opals gain +2 defense while in Miasma.");
        vocab.Add("boulder", "Ground type terrain, impassible, but can be destroyed by attacks.");
        vocab.Add("attack", "Add your attack value to damage you deal.");
        vocab.Add("defense", "Subtract your defense value from damage you take.");
        vocab.Add("speed", "The number of tiles you may move on your turn, and the order you move in.");
        vocab.Add("buff", "A positive stat change.");
        vocab.Add("debuff", "A negative stat change");
        vocab.Add("curse", "Spirit type mechanic that allows Opals to target victims of their curse remotely.");
        vocab.Add("adjacent", "The 4 tiles directly next to the target.");
        vocab.Add("surrounding", "The 8 tiles around the target.");
        vocab.Add("push", "Move the target a certain number of tiles in a certain direction.");
        vocab.Add("heal", "Raise the health of the target.");
    }


    public string getDefinition(string term)
    {
        if (!vocab.ContainsKey(term))
            return null;
        return vocab[term];
    }
}
