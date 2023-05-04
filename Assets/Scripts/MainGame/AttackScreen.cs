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
    private string[] shapes = new string[] { "", "Mortar", "Unimplemented", "Water Rush", "Line of Sight", "Target Growths", "Laser Beam", "All Opals", "Diagonal Laser"};
    private SpriteRenderer sr;
    private GroundScript board;
    private string textDescription;
    public SpriteRenderer displayBackground;
    public Text num;
    private int myNum;
    private string myInfo;
    private bool controller = false;

    private bool isEnabled = false;

    private bool justMe = false;

    private void Awake()
    {
        board = GameObject.Find("Main Camera").GetComponent<GroundScript>();
        myPos = transform.localPosition;
        transform.position = new Vector3(-100, -100, -100);
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(toggleScreen(false));
        transform.localScale = new Vector3(30, 30, 2);
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

    public void updateScreen(OpalScript attacking, int attackNum, TileScript target, bool onlyOne)
    {
        bool checkText = false;
        name.text = "";
        damage.text = "";
        //description.text = "";
        range.text = "";
        mechanic.text = "";
        shape.text = "";
        transform.localScale = new Vector3(30, 30, 2);
        justMe = false;
        transform.localPosition = myPos;
        if (attacking != null)
        {
            if (attacking.getTeam() == "Red")
            {
                displayBackground.color = new Color(1, 0.7f, 0.7f);
            }else if (attacking.getTeam() == "Blue")
            {
                displayBackground.color = new Color(0.7f, 0.7f, 1);
            }
            else if (attacking.getTeam() == "Green")
            {
                displayBackground.color = new Color(0f, 1, 0f);
            }
            else if (attacking.getTeam() == "Orange")
            {
                displayBackground.color = new Color(1, 0.4f, 0f);
            }
            displayBackground.enabled = true;

            if (!isEnabled)
            {
                isEnabled = true;
                //StartCoroutine(toggleScreen(true));
            }
            if (attackNum != -1 && onlyOne)
            {
                transform.localScale = new Vector3(45,45,2);
                transform.localPosition = new Vector3(myPos.x+80, myPos.y-80, myPos.z+80);
                justMe = true;
            }
        }
        if (attackNum == -1)
        {
            if (isEnabled)
            {
                isEnabled = false;
                //StartCoroutine(toggleScreen(false));
            }
            
            transform.position = new Vector3(-100, -100, -100);
            return;

        }


        if(attacking.Attacks[attackNum].getDesc() != textDescription)
        {
            checkText = true;
            textDescription = attacking.Attacks[attackNum].getDesc();
        }

        
        name.text = attacking.Attacks[attackNum].aname;
        string[] desc = attacking.Attacks[attackNum].getDesc().Split('\n');
        if (desc.Length == 2)
        {
            mechanic.text = "" + desc[0].Substring(0, desc[0].Length - 1).Substring(1);
        }
        if (checkText)
        {
            if (desc.Length == 2)
            {
                mechanic.text = "" + desc[0].Substring(0, desc[0].Length - 1).Substring(1);
                description.text = "" + desc[1];
            }
            else
                description.text = "" + desc[0];
        }
        num.text = myInfo;
        myNum = attackNum;
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

        if (checkText)
        {
            string[] eachWord = description.text.Split(' ');
            string checkVocab = "";

            foreach (string s in eachWord)
            {
                if (board.isVocabWord(s) != null)
                {
                    
                    checkVocab += "<b><color=navy>" + s +"</color></b> ";
                }
                else
                    checkVocab += s + " ";
            }
            description.text = checkVocab;
        }
    }

        public IEnumerator toggleScreen(bool enable)
    {
        if (enable)
        {
            displayBackground.enabled = true;
            transform.localPosition = new Vector3(myPos.x-1000, myPos.y,myPos.z);
            for(int i = 0; i < 10; i++)
            {
                transform.localPosition = new Vector3(transform.localPosition.x +100f, transform.localPosition.y, transform.localPosition.z);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            transform.localPosition = new Vector3(myPos.x, myPos.y, myPos.z);
            for (int i = 0; i < 10; i++)
            {
                transform.localPosition = new Vector3(transform.localPosition.x - 100f, transform.localPosition.y, transform.localPosition.z);
                yield return new WaitForFixedUpdate();
            }
            displayBackground.enabled = false;
            num.text = "";
        }
    }

    public void toggleTooltip(bool controller, int myNum)
    {
        this.controller = controller;
        if (controller)
        {
            switch (myNum) {
                case 1:
                    myInfo = "dUP";
                    break;
                case 2:
                    myInfo = "dRIGHT";
                    break;
                case 3:
                    myInfo = "dDOWN";
                    break;
                case 4:
                    myInfo = "dLEFT";
                    break;
                case -1:
                    myInfo = "";
                    break;
            }

        }
        else
        {
            myInfo = "Press " + myNum;
        }
    }
}
