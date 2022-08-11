using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int spellLenght = 3; //Длина заклинания
    int[] currentSpell = new int[spellLenght]; //Состав текущего заклинания

    private MobController mobController;
    private PlayerController player;

    public GameObject spellPanel;

    public GameObject rune1; //Префабы рун
    public GameObject rune2;
    public GameObject rune3;
    private GameObject[] runes = new GameObject[3]; //Массив префабов рун
    public GameObject parentRune; //Куда спавнить руны в структуре файла



    private LineRenderer lightningRenderer;
    public GameObject vampireDamagePrefab;
    public GameObject fireballPrefab;
    public GameObject healPrefab;
    public GameObject sandGlass;
    private bool timeSlow = false;

    //Заклинания
    public List<Spell> spells = new(){}; //Лист заклинаний
    Spell spell0 = new("nothingspell", new int[] { 0, 0, 0 }, 0, 0, typeSpell.nothing);      //00 спелл -
    Spell spell1 = new("fireball", new int[] { 1, 1, 1 }, 9, 1, typeSpell.damage);           //01 спелл 1 цель
    Spell spell2 = new("spell2", new int[] { 2, 1, 1 }, 7, 2, typeSpell.lightning);             //02 спелл 2 цели
    Spell spell3 = new("spell3", new int[] { 2 , 2, 1 }, 5, 4, typeSpell.lightning);            //03 спелл 4 цели
    Spell spell4 = new("chainlightning", new int[] { 2, 2, 2 }, 3, 6, typeSpell.lightning);  //04 спелл 6 целей
    Spell spell5 = new("spell5", new int[] { 3, 1, 1 }, 5, 6, typeSpell.reversevampire);             //05 спелл обратный вампиризм?
    Spell spell6 = new("spell6", new int[] { 3, 2, 2 }, 5, 6, typeSpell.timestop);             //06 спелл замедлние времени
    Spell spell7 = new("spell7", new int[] { 3, 3, 1 }, 5, 6, typeSpell.vampire);             //07 спелл вампиризм?
    Spell spell8 = new("spell8", new int[] { 3, 3, 2 }, 5, 2, typeSpell.reverselightning);             //08 2 цели с обратной стороны
    Spell spell9 = new("heal", new int[] { 3, 3, 3 }, 4, 1, typeSpell.heal);                 //09 спелл лечние
    Spell spell10 = new("menuspell", new int[] { 3, 2, 1 }, 0, 0, typeSpell.menuspell);      //10 спелл меню
 

    void Awake()
    {
        mobController = FindObjectOfType<MobController>();
        player = FindObjectOfType<PlayerController>();
        lightningRenderer = GetComponent<LineRenderer>();
        sandGlass.SetActive(false);
        spellPanel.SetActive(true);
        runes = new GameObject[3] { rune1, rune2, rune3};

        //Создание книги спелов
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

    public void Rune(int r) //Ввод руны
    {
        //Анимация появления руны?--------------------------------------
        //Звук руны----------------------------------------------------------
        if (r == 0)
        {
            return; //действие ввода неверной руны/не распознанной 
        }
        for (int i = 0; i < currentSpell.Length; i++) //Запись в массив заклинания
        {
            if (currentSpell[i] == 0)
            {
                currentSpell[i] = r;
                Instantiate(runes[r-1], new Vector3(0,0,0), Quaternion.identity, parentRune.transform); //Спавн префаба руны вверху экрана
                if (i == currentSpell.Length - 1)
                {
                    Array.Sort(currentSpell); //Влияние перестановки рун на разные заклинания 311 = 113 = 131
                    Array.Reverse(currentSpell);
                    CastSpell(ChooseSpell());
                    
                    for (int j = 0; j < spellLenght; j++)
                    {
                        currentSpell[j] = 0;   
                    }
                    foreach (GameObject rune in GameObject.FindGameObjectsWithTag("Rune")) //Удаление Рун с верхнего экрана
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
        public string name; //Название заклинания
        public int[] spell = new int[spellLenght]; //Вид заклинания
        public int damage; //Урон заклинания
        public int targets; //Количество целей заклинания
        public typeSpell type; //Тип заклинания

        public Spell(string name, int[] spell, int damage, int targets, typeSpell type) //Нормальный инициализатор
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
        Debug.Log(s.name);//Выбор типа и инициализация метода

        switch (s.type)
        {
            case typeSpell.nothing:
                Debug.Log("Nothing happend");
                break;

            case typeSpell.damage:
                if (mobController.listOfSlimes.Count > 0) //Здесь необходимо добавить кейс выбора спела-----------------------------!
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
                if (mobController.listOfSlimes.Count > 0) //Здесь необходимо добавить кейс выбора спела-----------------------------!
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
                if (mobController.listOfSlimes.Count > 0) //Здесь необходимо добавить кейс выбора спела-----------------------------!
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
                //Открытие меню
                spellPanel.SetActive(true);
                //остановка атаки и таймера
                Time.timeScale = 0f;
                break;

            default:

                break;
        }
    }

    public void AddSpell(Spell s) //Оболочка добавления спела
    {
        spells.Add(s);
    }

    public enum typeSpell //Типы заклинаний
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
