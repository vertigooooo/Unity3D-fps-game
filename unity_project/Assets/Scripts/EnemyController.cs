using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour {
    
    //角色状态参数对象
    public RoleStatus roleStatus;

    //玩家对象
    public GameObject player;

    //该对象的刚体组件
    public Rigidbody thisRig;

    //该对象的动画组件
    public Animation anim;

    //导航对象
    public NavMeshAgent navMeshAgent;

    //音效
    public AudioSource enemyAudio;

    //死亡音效片段
    public AudioClip dieAudioClip;

    //是否处于濒死状态
    public bool isDead = false;
    
    //计时器,控制调用频率
    private float timer = 0;

    //期望速度
    private Vector3 desireSpeed;

    //实际速度
    private Vector3 actSpeed;

    //速度权值
    //private float fakeMass = 20f;
    

	// Use this for initialization
	void Start () {
        //根据难度生成敌人
        //难度决定敌人的：速度、伤害(以及个数)
        player = GameObject.Find("Player");
        roleStatus.moveSpeed = player.GetComponent<RoleStatus>().moveSpeed - 1 + GameData.level;
        roleStatus.damage = 1 * GameData.level;//伤害量修改
	}
	
	// Update is called once per frame
	void Update () {
		//定时调用
        if((timer += Time.deltaTime) >= roleStatus.deltaTime)
        {
            //追逐玩家对象
            if(!isDead)
                chasePlayer();

            timer = 0;//清零
        }
	}


    /// <summary>
    /// 追逐行为
    /// </summary>
    void chasePlayer()
    {
        ////根据位置差求出期望速度
        //desireSpeed = (player.transform.position - transform.position).normalized * roleStatus.moveSpeed;

        ////根据期望和现实的差距求出一个力
        //Vector3 fakeAcceleration = desireSpeed - thisRig.velocity;
        //fakeAcceleration.y = 0;
        //thisRig.AddForce(fakeAcceleration * (fakeMass+ thisRig.drag));
        navMeshAgent.SetDestination(player.transform.position);
        navMeshAgent.speed = roleStatus.moveSpeed;
    }

    //记录玩家处在攻击范围内的时间
    private float atkTimer = 0;

    //攻击间隔
    private float atkDelay = 0.4f;
    
    //进入攻击范围时，
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PLAYER")
        {
            thisRig.velocity = Vector3.zero;
            navMeshAgent.SetDestination(gameObject.transform.position);
            //Debug.Log(other.name + "触发了" + gameObject.name + "的触发器");
        }
    }
    
    //更新计时器，判断是否要进行下一次攻击了
    private void OnTriggerStay(Collider other)
    {
        if(other.tag != "PLAYER")//其他物体不理他
        {
            //Debug.Log("碰到了其他物体：" + other.name);
            return;
        }

        
        //到达了攻击间隔
        if ((atkTimer += Time.deltaTime) >= atkDelay)
        {
            ATK();

            atkTimer = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        anim.Play("run");
        atkTimer = 0;
    }

    //攻击玩家
    private void ATK()
    {
        anim.Play("punch");//三倍速播放动画
        anim["punch"].normalizedSpeed = 3.0f;

        enemyAudio.time = 0.1f;
        enemyAudio.Play();//播放音效
        navMeshAgent.SetDestination(gameObject.transform.position);//停下
        player.GetComponent<PlayerController>().beATK();
        RoleStatus playerStatus = player.GetComponent<RoleStatus>();
        playerStatus.health -= roleStatus.damage;
        
            
        Debug.Log("玩家受到了攻击,当前生命值为" + playerStatus.health);

    }

    //被玩家攻击
    public void beAttacked(int damage)
    {
        if (roleStatus.health <= 0)
        {
            Die();
            return;
        }
        else
        {
            roleStatus.health -= damage;
        }

    }

    //死亡消失前调用的函数
    public void Die()
    {
        roleStatus.damage = 0;
        thisRig.velocity = Vector3.zero;
        navMeshAgent.SetDestination(gameObject.transform.position);//停下
        thisRig.constraints = RigidbodyConstraints.FreezeAll;
        enemyAudio.clip = dieAudioClip;
        enemyAudio.time = 0.9f;
        enemyAudio.Play();

        isDead = true;
        anim.Play("die1");
        Destroy(gameObject, 2.0f);
    }
}
