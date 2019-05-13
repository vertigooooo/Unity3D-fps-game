using UnityEngine;
using System.Collections;

/// <summary>
/// AI对象行为基类
/// </summary>
public class AIBehaviour : MonoBehaviour
{

    /// <summary>
    /// 操控力的权重
    /// </summary>
    public float weight = 1;
    
    /// <summary>
    /// 期望速度
    /// </summary>
    protected Vector3 desiredVelocity;
    
    /// <summary>
    /// AI对象
    /// </summary>
    protected AIObject aiObject;
    
    /// <summary>
    /// 最大速度
    /// </summary>
    protected float maxSpeed;

    void Start()
    {
        //获取被操控的角色
        aiObject = GetComponent<AIObject>();
        //获取最大速度
        maxSpeed = aiObject.maxSpeed;
    }
    
    /// <summary>
    /// 由子类来实现具体行为的方法
    /// </summary>
    public virtual Vector3 Force()
    {
        return Vector3.zero;
    }

}
