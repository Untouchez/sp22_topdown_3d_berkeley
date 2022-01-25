using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Health 
{
    public Blink blink;
    public Player player;
        
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        blink.BlinkME(0.15f, 5f, Color.white);
    }
}
