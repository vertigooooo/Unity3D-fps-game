using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 是游戏玩家（和NPC）的一些状态信息，每个角色都应该带有此脚本
/// （如生命值、攻击力等等）
/// </summary>
public class RoleStatus : MonoBehaviour {

    //初始生命值
    public int health = 100;

    //攻击力
    public int damage = 30;

    //移动速度
    public float moveSpeed = 10f;

    //更新的时间间隔(也是对象的反应速度)，实现定时调用
    [HideInInspector]
    public float deltaTime = 0.05f;
    
}
