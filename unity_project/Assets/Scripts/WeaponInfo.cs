using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour {

    //射速,几秒一发
    public float fireDelay = 0.1f;

    //伤害
    public float damage = 10.0f;

    //后坐力
    public float recoilForce = 0.0f;

    //子弹数量
    public int ammo = 30;
    
}
