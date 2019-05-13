using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    //规定一个矩形，敌人在这片矩形区域中出现
    public Transform rectStart;
    public Transform rectEnd;


    //运行的时钟周期
    public float timeCycle = 1.0f;

    //敌人预制体
    public GameObject enemyPrefab;

    //玩家状态信息
    public RoleStatus playerStatus;

    //敌人计数器，减到0的时候玩家胜利
    private int EnemyCount;

    //计时器，保证时钟周期
    private float timer;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        //隐藏鼠标指针
        Cursor.visible = false;

        //难度影响敌人个数(当然，还有卡顿的程度)
        EnemyCount = GameData.level * 3;

        //敌人生成器
        EnemyProvider();
        
    }
	
	// Update is called once per frame
	void Update () {
        if ((timer += Time.deltaTime) >= timeCycle)
        {
            if (!GameObject.FindWithTag("ENEMY"))
            {
                //显示鼠标指针
                Cursor.visible = true;
                GameData.gameResult = GameData.GameResult.win;
                SceneManager.LoadScene("FinishScene");
            }
            else if (playerStatus.health <= 0)//生命值小于0.原地死亡，进入结算界面
            {
                //显示鼠标指针
                Cursor.visible = true;
                GameData.gameResult = GameData.GameResult.lose;
                SceneManager.LoadScene("FinishScene");
            }
            else
            {
                ;//还未结束
            }

            timer = 0;
        }
	}

    //敌人生成器
    void EnemyProvider()
    {
        //游戏开始时，在矩形内生成一大波敌人
        //上下左右的生成边界
        float constantY = rectEnd.position.y;
        float leftB = rectStart.position.x, rightB = rectEnd.position.x;
        float upB = rectStart.position.z, downB = rectEnd.position.z;
        float width = rightB - leftB, height = upB - downB;

        StartCoroutine("createEnemy", new Vector3(Random.value * width + leftB, constantY, Random.value * height + downB));
    }

    //生成单个的敌人
    IEnumerator createEnemy(Vector3 position)
    {
        while (EnemyCount-- > 0)
        {
            Instantiate(enemyPrefab, position, new Quaternion());
            yield return new WaitForSeconds(2);
        }
    }

}
