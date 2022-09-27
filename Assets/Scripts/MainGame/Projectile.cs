using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour{
    int shape;
    bool head = true;
    Color mainCol;
    Color secondCol;
    bool rainbow = false;
    List<Color> rainbows = new List<Color>() { Color.red, new Color(219 / 255f, 11 / 255f, 0), Color.yellow, Color.green, Color.blue, new Color(75 / 255f, 0 / 255f, 130 / 255f), new Color(238 / 255f, 130 / 238, 0)};
    int r = 0;
    CursorScript cs;
    ParticleSystem hurtParticle;
    float speed = 1;

    /**
     * Shape state
     * 0 = movement, no attack
     * 1 = normal shaped attack, no line of sight needed
     * 2 = normal shaped attack, line of sight needed
     * 3 = "adjacent attack", normal shaped but flood tiles extend range
     * 4 = line attack, line of sight needed
     * 5 = target just growth tiles
     * */
    public void setUp(int s, string type1)
    {
        shape = s;
        if(shape == 4 || shape == 6 || shape == 5)
        {
            transform.localScale *= 0.8f;
            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                t.localScale *= 0.8f;
            }
        }
        mainCol = getColorFromType(type1, true);
        secondCol = getColorFromType(type1, false);

        hurtParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/ParticleDamage");
        //GetComponent<Renderer>().material.color = mainCol;
        if(head)
            cs = FindObjectOfType<CursorScript>();
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = secondCol;
        }
        GetComponent<Renderer>().material.color = mainCol;
    }

    private Color getColorFromType(string type, bool first)
    {
        Color projColor = Color.white;
        if (type == "Grass")
        {
            if (first)
                return new Color(7/255f, 200/255f, 0);
            return new Color(9/255f, 255/255f, 0);
        }
        else if (type == "Fire")
        {
            if (first)
                return new Color(219 / 255f, 11 / 255f, 0);
            return new Color(255 / 255f, 136 / 255f, 0);
        }
        else if (type == "Water")
        {
            if (first)
                return new Color(0, 77 / 255f, 219 / 255f);
            return new Color(0, 212 / 255f, 245 / 255f);
        }
        else if (type == "Light")
        {
            if (first)
                return new Color(255 / 255f, 255 / 255f, 255 / 255f);
            return new Color(255 / 255f, 254 / 255f, 240 / 255f);
        }
        else if (type == "Dark")
        {
            if (first)
                return new Color(0, 0, 0);
            return new Color(25 / 255f, 0, 71 / 255f);
        }
        else if (type == "Plague")
        {
            if (first)
                return new Color(78 / 255f, 0, 22 / 255f);
            return new Color(141 / 255f, 0, 222 / 255f);
        }
        else if (type == "Air")
        {
            if (first)
                return new Color(255 / 255f, 251 / 255f, 201 / 255f);
            return new Color(224 / 255f, 255 / 255f, 253 / 255f);
        }
        else if (type == "Ground")
        {
            if (first)
                return new Color(235 / 255f, 121 / 255f, 0);
            return new Color(89 / 255f, 66 / 255f, 42 / 255f);
        }
        else if (type == "Void")
        {
            if (first)
                return new Color(105 / 255f, 105 / 255f, 105 / 255f);
            return new Color(200 / 255f, 200 / 255f, 200 / 255f);
        }
        else if (type == "Metal")
        {
            if (first)
                return new Color(191 / 255f, 182 / 255f, 163 / 255f);
            return new Color(163 / 255f, 191 / 255f, 182 / 255f);
        }
        else if (type == "Electric")
        {
            if (first)
                return new Color(255 / 255f, 255 / 255f, 102 / 255f);
            return new Color(255 / 255f, 255 / 255f, 204 / 255f);
        }
        else if (type == "Laser")
        {
            if (first)
                return new Color(255 / 255f, 0 / 255f, 0 / 255f);
            return new Color(200 / 255f, 0 / 255f, 0 / 255f);
        }
        else if (type == "Swarm")
        {
            if (first)
                return new Color(184 / 255f, 109 / 255f, 39 / 255f);
            return new Color(149 / 255f, 166 / 255f, 36 / 255f);
        }
        else if (type == "Spirit")
        {
            if (first)
                return new Color(157 / 255f, 0 / 255f, 166 / 255f);
            return new Color(98 / 255f, 0 / 255f, 143 / 255f);
        }
        return projColor;
    }

    public void setHead(bool h)
    {
        head = h;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (rainbow)
        {
            GetComponent<Renderer>().material.color = rainbows[r];
            r++;
            if(r > 6)
            {
                r = 0;
            }
        }
    }

    public void fire(OpalScript from, OpalScript to, int attackNum)
    {
        if(head)
            from.preFire(attackNum, to.getCurrentTile());
        transform.position = from.getPos();
        float angle = -1 * (Mathf.Atan2((to.getPos().z - from.getPos().z), (to.getPos().x - from.getPos().x)) * 180 / Mathf.PI);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //print(from.getPos() + " and " + to.getPos());
        StartCoroutine(shoot(from, to, attackNum));
    }

    public void fire(OpalScript from, TileScript to, int attackNum)
    {
        if(head)
            from.preFire(attackNum, to);
        transform.position = from.getPos();
        //print(from.getPos() + " and " + to.getPos());
        float angle = -1 * (Mathf.Atan2((to.getPos().z - from.getPos().z), (to.getPos().x - from.getPos().x)) * 180 / Mathf.PI);
        transform.rotation = Quaternion.Euler(0f, angle,0f);
        StartCoroutine(shootTile(from, to, attackNum));
    }

    public void doRainbow()
    {
        rainbow = true;
    }

    public void setSpeed(float s)
    {
        speed = s;
    }

    private IEnumerator shoot(OpalScript from, OpalScript target, int attackNum)
    {
        foreach (TileScript t in from.getSurroundingTiles(true))
        {
            if(t.currentPlayer == target)
            {
                shape = 0;
                speed += 4;
            }
        }
        Vector3 mine = transform.position;
        Vector3 theirs = target.getPos();
        float xdiff = mine.x - theirs.x;
        float zdiff = mine.z - theirs.z;
        int i = 0;
        while (transform.position.x < theirs.x - 0.1f || transform.position.z < theirs.z -0.1f || transform.position.x > theirs.x  + 0.1f|| transform.position.z > theirs.z + 0.1f)
        {
            mine.x -= xdiff/30 *speed;
            mine.z -= zdiff/30 *speed;
            if(shape == 1 || shape == 5)
            {
                mine.y += 0.01f * (12-i);
                if (i == 0)
                    transform.Rotate(new Vector3(0, 0, 45));
                else
                    transform.Rotate(new Vector3(0, 0, -3));
            }if(head && (shape == 5 || shape == 4 || shape == 6))
            {
                Projectile tempProj = Instantiate(this);
                tempProj.setUp(from.getAttacks()[attackNum].getShape(), from.getMainType());
                if(i % 2 == 0)
                {
                    tempProj.GetComponent<Renderer>().material.color = secondCol;
                }
                tempProj.setHead(false);
                tempProj.setSpeed(speed);
                tempProj.fire(from, target, attackNum);
            }
            transform.position = mine;
            i++;
            yield return new WaitForFixedUpdate();
        }
        if (head)
        {
            summonParticleDamage(mainCol, target.getPos());
            target.takeDamage(from.getAttackEffect(attackNum, target), true, true);
            cs.updateData();
            if (from != target)
                cs.restartAttack();
            cs.checkWin();
        }
        DestroyImmediate(this.gameObject);
    }

    private IEnumerator shootTile(OpalScript from, TileScript target, int attackNum)
    { 
        Vector3 mine = transform.position;
        Vector3 theirs = target.getPos();
        float xdiff = mine.x - theirs.x;
        float zdiff = mine.z - theirs.z;
        int i = 0;
        while (transform.position.x < theirs.x - 0.1f || transform.position.z < theirs.z - 0.1f || transform.position.x > theirs.x + 0.1f || transform.position.z > theirs.z + 0.1f)
        {
            mine.x -= xdiff / 30 * speed;
            mine.z -= zdiff / 30 * speed;
            if (shape == 1 || shape == 5)
            {
                mine.y += 0.01f * (12 - i);
                if (i == 0)
                    transform.Rotate(new Vector3(0, 0, 45));
                else
                    transform.Rotate(new Vector3(0, 0, -3));
            }
            if (head && (shape == 5 || shape == 4))
            {
                Projectile tempProj = Instantiate(this);
                tempProj.setUp(from.getAttacks()[attackNum].getShape(), from.getMainType());
                if (i % 2 == 0)
                {
                    tempProj.GetComponent<Renderer>().material.color = secondCol;
                }
                tempProj.setHead(false);
                tempProj.setSpeed(speed);
                tempProj.fire(from, target, attackNum);
            }
            transform.position = mine;
            i++;
            yield return new WaitForFixedUpdate();
        }
        if (head)
        {
            summonParticleDamage(mainCol, target.transform.position);
            from.getAttackEffect(attackNum, target);
            cs.updateData();
            if (from != target.currentPlayer)
                cs.restartAttack();
            cs.checkWin();
        }
        
        DestroyImmediate(this.gameObject);
    }

    private void summonParticleDamage(Color clr, Vector3 pos)
    {
        ParticleSystem pD = Instantiate<ParticleSystem>(hurtParticle);
        var m = pD.main;
        m.startColor = clr;
        pD.transform.position = new Vector3(pos.x, 0.5f, pos.z);
    }
}
