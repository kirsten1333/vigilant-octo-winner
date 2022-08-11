using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    MobController mobController;
    GameController game;
    public GameObject self;
    Animator animator;
    public float speed = 0.001f;
    float position = 0f;
    // Start is called before the first frame update
    void Start()
    {
        mobController = FindObjectOfType<MobController>();
        animator = GetComponent<Animator>();
        game = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (position >= 1)
        {
            animator.SetTrigger("Destroy");
        }
        
        if (position < 1)
        {
            self.transform.position = Vector3.Lerp(GameObject.FindGameObjectWithTag("Player").transform.position, mobController.position1.transform.position, position);
            position += speed;
        }
    }
    void DealDamage()
    {
        if (mobController.listOfSlimes.Count > 0) {
            mobController.listOfSlimes[0].GetComponent<Mobs>().TakeDamage(game.spells[1].damage);
        }
    }
    private void Destroy()
    {
        Destroy(self);
    }

}
