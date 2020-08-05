using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackScreen : MonoBehaviour
{
    public Text name;
    public Text damage;
    public Text shape;
    public Text description;
    public Text range;
    public Text mechanic;
    private Vector3 myPos;
    private string[] shapes = new string[] { "", "Mortar", "Unimplemented", "Water Rush", "Line of Sight", "Target Growths", "Laser Beam", "All Opals"};
    private SpriteRenderer sr;

    private void Awake()
    {
        myPos = transform.localPosition;
        transform.position = new Vector3(-100, -100, -100);
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
 * Attack state
 * 0 = movement, no attack
 * 1 = normal shaped attack, no line of sight needed
 * 2 = normal shaped attack, line of sight needed
 * 3 = "adjacent attack", normal shaped but flood tiles extend range
 * 4 = line attack, line of sight needed
 * 5 = target just growth tiles
 * 6 = laser beam, line which ignores line of sight
 * 7 = target all opals
 * */

    public void updateScreen(OpalScript attacking, int attackNum, TileScript target)
    {
        if(attacking != null)
        {
            if(attacking.getTeam() == "Red")
            {
                sr.color = new Color(1, 0.7f, 0.7f);
            }else if (attacking.getTeam() == "Blue")
            {
                sr.color = new Color(0.7f, 0.7f, 1);
            }
            else if (attacking.getTeam() == "Green")
            {
                sr.color = new Color(0f, 1, 0f);
            }
            else if (attacking.getTeam() == "Orange")
            {
                sr.color = new Color(1, 0.4f, 0f);
            }
        }
        if (attackNum == -1)
        {
            //doattacking = false;
            transform.position = new Vector3(-100, -100, -100);
            name.text = "";
            damage.text = "";
            description.text = "";
            range.text = "";
            mechanic.text = "";
            shape.text = "";
            return;

        }
        transform.localPosition = myPos;
        name.text = attacking.Attacks[attackNum].aname;
        string[] desc = attacking.Attacks[attackNum].getDesc().Split('\n');
        if (desc.Length == 2)
        {
            mechanic.text = "" + desc[0].Substring(0, desc[0].Length-1).Substring(1);
            description.text = "" + desc[1];
        }
        else
            description.text = "" + desc[0];
        range.text = "" + attacking.Attacks[attackNum].getRange();
        if(attacking.Attacks[attackNum].getRange() == 0 && attacking.Attacks[attackNum].getShape() == 0)
            shape.text = "Self Target";  
        else
            shape.text = shapes[attacking.Attacks[attackNum].getShape()];
        if(target == null)
        {
            damage.text = "" + attacking.Attacks[attackNum].getBaseDamage();
        }
        else if (attacking.getAttackDamage(attackNum, target) > -1)
            damage.text = "" + attacking.getAttackDamage(attackNum, target);
        else
            damage.text = "0";
    }
}
