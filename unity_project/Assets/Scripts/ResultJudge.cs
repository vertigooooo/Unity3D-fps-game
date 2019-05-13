using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultJudge : MonoBehaviour {

    //结果参数
    public Text text;

	// Use this for initialization
	void Start () {
        switch (GameData.gameResult)
        {
            case GameData.GameResult.lose:
                text.text = "你输了，要再试一次吗";
                break;
            case GameData.GameResult.win:
                text.text = "你赢了";
                break;
            case GameData.GameResult.not_sure:
                text.text = "还没有结果";
                break;
            case GameData.GameResult.time_out:
                text.text = "结果无法读取，到底发生了什么？";
                break;
            default:
                text.text = "结果无法读取，到底发生了什么？";
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
