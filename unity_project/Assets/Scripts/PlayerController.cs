using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //角色控制器组件
    public CharacterController characterController;

    //主摄像机
    public Camera mainCam;

    //角色状态参数对象
    public RoleStatus roleStatus;

    //移动速度
    //public float speed;

    //默认武器
    public GameObject defaultWeapon;

    //武器声（枪声）
    public AudioSource weaponAudioSource;

    //玩家声（平常是脚步声）
    public AudioSource playerAudioSource;

    //枪口火焰
    public ParticleSystem shotFire;

    //血迹
    public GameObject bloodPrefab;

    //打击效果
    public GameObject shotCrush;

    //转动速度
    private float rotateSpeedX = 0.6f;
    private float rotateSpeedY = 0.6f;

    //玩家弹药量值显示
    public Text AmmoText;

    //玩家血量值显示
    public Text HealthText;

    //武器的默认信息
    private WeaponInfo weaponInfo;

    //上下观察范围
    private float maxY = 60;
    private float minY = -60;

    //观察的变化量
    private float rotateX;
    private float rotateY;

    //下一个可以开火的时间
    private float fireTimer;

    //当前弹药量
    private int nowAmmo;

    //初始化
    private void Start()
    {
        //Cursor.visible = false;
        //获取默认武器的参数信息
        weaponInfo = defaultWeapon.GetComponent<WeaponInfo>();
        roleStatus.moveSpeed = 5.5F;
        roleStatus.damage = 10;
        nowAmmo = weaponInfo.ammo;
    }

    // Update is called once per frame
    void FixedUpdate () {
        
        #region 1,控制玩家移动

        //播放脚步音效
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
        {
            if (!playerAudioSource.isPlaying)
            {
                playerAudioSource.Play();
            }
        }
        else
        {
            playerAudioSource.Pause();
        }

        //前后
        Vector3 fwd = mainCam.transform.TransformDirection(Vector3.forward);
        characterController.SimpleMove(fwd * (roleStatus.moveSpeed * Input.GetAxis("Vertical")));
        //左右
        Vector3 right = mainCam.transform.TransformDirection(Vector3.right);
        characterController.SimpleMove(right * (roleStatus.moveSpeed * Input.GetAxis("Horizontal")));
        
        #endregion

        #region 2,控制视角

        rotateX = (rotateX + Input.GetAxis("Mouse X") * rotateSpeedX)%360;
        rotateY = Mathf.Clamp(rotateY + Input.GetAxis("Mouse Y") * rotateSpeedY, minY, maxY);
        this.transform.localEulerAngles = new Vector3(-rotateY, rotateX, 0);

        #endregion

        #region 3,控制开火

        if (Input.GetMouseButton(0)  &&(fireTimer += Time.deltaTime) >= weaponInfo.fireDelay)
        {
            //控制射速
            fireTimer = 0;

            //播放枪声
            weaponAudioSource.Play();

            //播放火焰
            shotFire.Play();

            //弹药减少
            nowAmmo = (nowAmmo - 1) % 30;


            //发射射线并检测碰撞，相当于射击
            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            RaycastHit hitInfo;
            
            //如果发生了碰撞
            if(Physics.Raycast(ray, out hitInfo))
            {
                //创建打到物体上的粒子特效
                Quaternion tempQuaternion = hitInfo.transform.rotation;
                tempQuaternion.SetLookRotation(mainCam.transform.position);

                GameObject colObj = hitInfo.collider.gameObject;                
                //如果打中了敌人
                if (colObj.tag == "ENEMY")
                {
                    GameObject tempCrush = Instantiate(shotCrush, hitInfo.point, tempQuaternion);
                    tempCrush.GetComponent<ParticleSystem>().startColor = Color.red;
                    Destroy(tempCrush, 1.0f);
                    Vector3 bloodPosition = hitInfo.point;
                    bloodPosition.y = 0;
                    Instantiate(bloodPrefab, bloodPosition, tempQuaternion);
                    ATK(colObj);//攻击敌人
                }
                else
                {
                    GameObject tempCrush = Instantiate(shotCrush, hitInfo.point, tempQuaternion);
                    Destroy(tempCrush, 1.0f);
                }
            }
            
        }

        #endregion

        #region 4,UI更新

        //更新血量和弹药量
        HealthText.text = roleStatus.health < 0 ? "0" : roleStatus.health.ToString();
        AmmoText.text = nowAmmo < 0 ? "0" : nowAmmo.ToString();

        #endregion
    }


    //攻击敌人
    private void ATK(GameObject colObj)
    {
        EnemyController ec = colObj.GetComponent<EnemyController>();
        if (!ec.isDead)
            ec.beAttacked(roleStatus.damage);
    }

    //被攻击
    public void beATK()
    {
        
    }
}
