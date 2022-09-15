using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlayer : MonoBehaviour
{
    public static TowerPlayer Instance;

    public int life;
    public int money;



    public void SetUP(int life, int money)
    {
        this.life = life;
        this.money = money;
    }

    private void Awake()
    {
        Instance = this;
    }
}
