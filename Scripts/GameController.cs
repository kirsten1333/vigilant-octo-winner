using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int spellLenght = 3; //����� ����������
    int[] currentSpell = new int[spellLenght]; //������ �������� ����������

    private MobController mobController;
    private PlayerController player;

    public GameObject spellPanel;

    public GameObject rune1; //������� ���
    public GameObject rune2;
    public GameObject rune3;
    private GameObject[] runes = new GameObject[3]; //������ �������� ���
    public GameObject parentRune; //���� �������� ���� � ��������� �����



    private LineRenderer lightningRenderer;
    public GameObject vampireDamagePrefab;
    public GameObject fireballPrefab;
    public GameObject healPrefab;
    public GameObject sandGlass;
    private bool timeSlow = false;

    //����������
    public List<Spell> spells = new(){}; //���� ����������
    Spell spell0 = new("nothingspell", new int[] { 0, 0, 0 }, 0, 0, typeSpell.nothing);      //00 ����� -
    Spell spell1 = new("fireball", new int[] { 1, 1, 1 }, 9, 1, typeSpell.damage);           //01 ����� 1 ����
    Spell spell2 = new("spell2", new int[] { 2, 1, 1 }, 7, 2, typeSpell.lightning);             //02 ����� 2 ����
    Spell spell3 = new("spell3", new int[] { 2 , 2, 1 }, 5, 4, typeSpell.lightning);            //03 ����� 4 ����
    Spell spell4 = new("chainlightning", new int[] { 2, 2, 2 }, 3, 6, typeSpell.lightning);  //04 ����� 6 �����
    Spell spell5 = new("spell5", new int[] { 3, 1, 1 }, 5, 6, typeSpell.reversevampire);             //05 ����� �������� ���������?
    Spell spell6 = new("spell6", new int[] { 3, 2, 2 }, 5, 6, typeSpell.timestop);             //06 ����� ��������� �������
    Spell spell7 = new("spell7", new int[] { 3, 3, 1 }, 5, 6, typeSpell.vampire);             //07 ����� ���������?
    Spell spell8 = new("spell8", new int[] { 3, 3, 2 }, 5, 2, typeSpell.reverselightning);             //08 2 ���� � �������� �������
    Spell spell9 = new("heal", new int[] { 3, 3, 3 }, 4, 1, typeSpell.heal);                 //09 ����� ������
    Spell spell10 = new("menuspell", new int[] { 3, 2, 1 }, 0, 0, typeSpell.menuspell);      //10 ����� ����
 

    void Awake()
    {
        mobController = FindObjectOfType<MobController>();
        player = FindObjectOfType<PlayerController>();
        lightningRenderer = GetComponent<LineRenderer>();
        sandGlass.SetActive(false);
        spellPanel.SetActive(true);
        runes = new GameObject[3] { rune1, rune2, rune3};

        //�������� ����� ������
        AddSpell(spell0); 
        AddSpell(spell1); 
        AddSpell(spell2); 
        AddSpell(spell3); 
        AddSpell(spell4); 
        AddSpell(spell5);              
        AddSpell(spell6);                                    
        AddSpell(spell7);
        AddSpell(spell8);
        AddSpell(spell9);
        AddSpell(spell10);
        Time.timeScale = 0f;
    }

    public void Rune(int r) //���� ����
    {
        //�������� ��������� ����?--------------------------------------
        //���� ����----------------------------------------------------------
        if (r == 0)
        {
            return; //�������� ����� �������� ����/�� ������������ 
        }
        for (int i = 0; i < currentSpell.Length; i++) //������ � ������ ����������
        {
            if (currentSpell[i] == 0)
            {
                currentSpell[i] = r;
                Instantiate(runes[r-1], new Vector3(0,0,0), Quaternion.identity, parentRune.transform); //����� ������� ���� ������ ������
                if (i == currentSpell.Length - 1)
                {
                    Array.Sort(currentSpell); //������� ������������ ��� �� ������ ���������� 311 = 113 = 131
                    Array.Reverse(currentSpell);
                    CastSpell(ChooseSpell());
                    
                    for (int j = 0; j < spellLenght; j++)
                    {
                        currentSpell[j] = 0;   
                    }
                    foreach (GameObject rune in GameObject.FindGameObjectsWithTag("Rune")) //�������� ��� � �������� ������
                    {
                        Destroy(rune);
                    }
                }
                break;
            }
        }
    }

    public Spell ChooseSpell()
    {
        foreach (Spell i in spells)
        {
            if (i.spell.SequenceEqual(currentSpell))
            {
                return (i);
            }
        }

        return spell0;
    }

    public class Spell
    {
        public string name; //�������� ����������
        public int[] spell = new int[spellLenght]; //��� ����������
        public int damage; //���� ����������
        public int targets; //���������� ����� ����������
        public typeSpell type; //��� ����������

        public Spell(string name, int[] spell, int damage, int targets, typeSpell type) //���������� �������������
        {
            this.name = name;
            this.spell = spell;
            this.damage = damage;
            this.targets = targets;
            this.type = type;
        }
    }

    public void CastSpell(Spell s)
    {
        Debug.Log(s.name);//����� ���� � ������������� ������

        switch (s.type)
        {
            case typeSpell.nothing:
                Debug.Log("Nothing happend");
                break;

            case typeSpell.damage:
                if (mobController.listOfSlimes.Count > 0) //����� ���������� �������� ���� ������ �����-----------------------------!
                {
                    Instantiate(
                        fireballPrefab,
                        GameObject.FindGameObjectWithTag("Player").transform.position,
                        Quaternion.identity,
                        GameObject.FindGameObjectWithTag("BG").transform);
                }
                break;

            case typeSpell.heal:
                player.Heal(s.damage);
                Instantiate(
                    healPrefab,
                    GameObject.FindGameObjectWithTag("Player").transform.position,
                    Quaternion.identity,
                    GameObject.FindGameObjectWithTag("BG").transform);
                break;

            case typeSpell.lightning:
                lightningRenderer.positionCount = 0;
                if (mobController.listOfSlimes.Count > 0) //����� ���������� �������� ���� ������ �����-----------------------------!
                {
                    lightningRenderer.positionCount = mobController.listOfSlimes.Count + 1;
                    lightningRenderer.SetPosition(0, GameObject.FindGameObjectWithTag("Player").transform.position);
                    for (int i = 0; i < mobController.listOfSlimes.Count && i < s.targets; i++)
                    {
                        mobController.listOfSlimes[i].GetComponent<Mobs>().TakeDamage(s.damage);
                        lightningRenderer.SetPosition(i + 1, mobController.listOfSlimes[i].transform.position);
                    }
                }
                StartCoroutine(LightningSeconds());


                //lightningRenderer.positionCount = 0;
                break;

            case typeSpell.reverselightning:
                lightningRenderer.positionCount = 0;
                if (mobController.listOfSlimes.Count > 0) //����� ���������� �������� ���� ������ �����-----------------------------!
                {
                    lightningRenderer.positionCount = s.targets + 1;
                    lightningRenderer.SetPosition(0, GameObject.FindGameObjectWithTag("Player").transform.position);
                    for (int i = mobController.listOfSlimes.Count, j = 1; i > 0 && i > mobController.listOfSlimes.Count - s.targets; i--, j++)
                    {
                        mobController.listOfSlimes[i-1].GetComponent<Mobs>().TakeDamage(s.damage);
                        lightningRenderer.SetPosition(j, mobController.listOfSlimes[i-1].transform.position);
                    }
                }
                StartCoroutine(LightningSeconds());
                break;

            case typeSpell.vampire:
                if (mobController.listOfSlimes.Count > 0)
                {
                    for (int i = 0; i < mobController.listOfSlimes.Count && i < s.targets; i++)
                    {
                        mobController.listOfSlimes[i].GetComponent<Mobs>().TakeDamage(s.damage);
                        Instantiate(
                            vampireDamagePrefab,
                            mobController.listOfSlimes[i].GetComponent<Mobs>().transform.position,
                            Quaternion.identity,
                            GameObject.FindGameObjectWithTag("BG").transform);

                        player.Heal(Convert.ToInt32(Math.Round(s.damage*0.25, 0)));
                        Instantiate(
                            healPrefab,
                            GameObject.FindGameObjectWithTag("Player").transform.position,
                            Quaternion.identity,
                            GameObject.FindGameObjectWithTag("BG").transform);
                    }
                }
                break;

            case typeSpell.reversevampire:
                for (int i = 0; i < mobController.listOfSlimes.Count && i < s.targets; i++)
                {
                    mobController.listOfSlimes[i].GetComponent<Mobs>().TakeDamage(s.damage);
                    Instantiate(
                        vampireDamagePrefab,
                        mobController.listOfSlimes[i].GetComponent<Mobs>().transform.position,
                        Quaternion.identity,
                        GameObject.FindGameObjectWithTag("BG").transform);


                    player.TakeDamage(Convert.ToInt32(Math.Round(s.damage * 0.25, 0)));
                    Instantiate(
                        vampireDamagePrefab,
                        GameObject.FindGameObjectWithTag("Player").transform.position,
                        Quaternion.identity,
                        GameObject.FindGameObjectWithTag("BG").transform);
                }
                break;

            case typeSpell.timestop:
                if (timeSlow == false) {
                    StartCoroutine(TimeStop());
                }
                else
                {
                    StopCoroutine(TimeStop());
                    Debug.Log("Stoped by  Player Coroutine \"Timestop\" at timestamp :  " + Time.time);
                    StartCoroutine(TimeStop());
                }
                break;

            case typeSpell.menuspell:
                //�������� ����
                spellPanel.SetActive(true);
                //��������� ����� � �������
                Time.timeScale = 0f;
                break;

            default:

                break;
        }
    }

    public void AddSpell(Spell s) //�������� ���������� �����
    {
        spells.Add(s);
    }

    public enum typeSpell //���� ����������
    {
        nothing, damage, heal, lightning, menuspell, reversevampire, vampire, timestop, reverselightning
    }
    public void HideMenu()
    {
        spellPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    IEnumerator LightningSeconds()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine \"LightningSeconds\" at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1.5f);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine \"LightningSeconds\" at timestamp : " + Time.time);
        lightningRenderer.positionCount = 0;
        StopCoroutine(LightningSeconds());
    }
    IEnumerator TimeStop()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine \"Timestop\" at timestamp : " + Time.time);
        timeSlow = true;
        sandGlass.SetActive(true);
        Time.timeScale = 0.75f;
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(6f);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine \"Timestop\" at timestamp : " + Time.time);
        Time.timeScale = 1f;
        sandGlass.SetActive(false);
        timeSlow = false;
        StopCoroutine(LightningSeconds());
    }
}
