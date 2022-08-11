using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    public GameObject prefabSlime; //Префаб слизня
    public GameObject prefabFlower; //Префаб слизня
    [SerializeField]
    GameObject timer; //Экземпляр таймера на сцене
    public int timeSpawn = 6; //Каждые сколько секунд спавнится моб

    public GameObject parent; //Место в иерархии сцены куда спавнятся мобы

    //Позиции спавна монстров
    public GameObject position1; 
    public GameObject position2;
    public GameObject position3;
    public GameObject position4;
    public GameObject position5;
    public GameObject position6;


    public bool spawnedThisTime = false;
    public float time;
    public List<GameObject> listOfSlimes= new(6);
    public List<GameObject> positions = new(6);

   
    

    // Start is called before the first frame update
    void Awake()
    {
        positions.Add(position1);
        positions.Add(position2);
        positions.Add(position3);
        positions.Add(position4);
        positions.Add(position5);
        positions.Add(position6);
    }

    // Update is called once per frame
    void Update()
    {
        time = timer.GetComponent<GlobalTimer>().timer;
        if (time % timeSpawn == 1 && spawnedThisTime == false)
        {
            if(Random.value > 0.5)
            {
                SpawnMob(prefabFlower);
            }
            else
            {
                SpawnMob(prefabSlime);
            }
            spawnedThisTime = true;
        }
        if (time % timeSpawn == 2)
        {
            spawnedThisTime = false;
        }
    }
    public void SpawnMob(GameObject prefab)
    {
        if (listOfSlimes.Count < listOfSlimes.Capacity)
        {
            Instantiate(prefab, position1.transform.position, Quaternion.identity, parent.transform);
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject i in enemys)
            {
                if (!listOfSlimes.Contains(i))
                {
                    listOfSlimes.Add(i);
                    UpdatePosition();
                }
            }
        }
    }

    public void UpdatePosition()
    {
        foreach (GameObject i in listOfSlimes)
        {
            i.transform.position = positions[listOfSlimes.IndexOf(i)].transform.position;
            i.GetComponent<Mobs>().position = listOfSlimes.IndexOf(i);
            if (i.GetComponent<Mobs>().position < 2 && !i.GetComponent<Mobs>().isAttack)
            {
                i.GetComponent<Mobs>().isAttack = true;
                StartCoroutine(i.GetComponent<Mobs>().AttackTimer());
            }
        }
    }
}
