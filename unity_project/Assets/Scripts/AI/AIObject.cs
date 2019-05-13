using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AIObject是一个基类
/// 所有可移动的智能对象都继承于它
/// 它封装一些数据，用来描述基本的智能对象
/// </summary>
public class AIObject : MonoBehaviour
{
    /// <summary>
    /// 对象的最大移动速度
    /// </summary>
    public float maxSpeed = 10;

    /// <summary>
    /// 给对象施加的最大力值
    /// </summary>
    public float maxForce = 100;

    /// <summary>
    /// 对象质量
    /// </summary>
    public float mass = 1;
    
    /// <summary>
    /// 对象速度
    /// </summary>
    public Vector3 velocity;
    
    /// <summary>
    /// 旋转的最大速率
    /// </summary>
    public float damping = 0.9f;
    
    /// <summary>
    /// 操控力的计算间隔时间
    /// </summary>
    public float computeInterval = 0.2f;

    /// <summary>
    /// 加速度
    /// </summary>
    protected Vector3 acceleration;

    protected float sqrMaxSpeed;

    /// <summary>
    /// AI对象行为列表
    /// </summary>
    private AIBehaviour[] behaviours;
    
    /// <summary>
    /// 计时器
    /// </summary>
    private float timer;
    
    /// <summary>
    /// 控制力
    /// </summary>
    private Vector3 steeringForce;

    // Use this for initialization
    protected void Start()
    {
        //获取对象的行为
        behaviours = GetComponents<AIBehaviour>();
        steeringForce = Vector3.zero;
        sqrMaxSpeed = maxSpeed * maxSpeed;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //运行计时器
        timer += Time.deltaTime;
        steeringForce = Vector3.zero;

        //如果计时器时间大于设定的间隔时间才计算
        if (timer > computeInterval)
        {
            //计算已激活的行为的操控力
            foreach (AIBehaviour behaviour in behaviours)
            {
                if (behaviour.enabled)
                    steeringForce += behaviour.Force() * behaviour.weight;
            }
            //确保控制力不超过最大上限
            steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
            //获得加速度  加速度 = 力量 / 质量
            acceleration = steeringForce / mass;
            //刷新计时器
            timer = 0;
        }
    }
}
