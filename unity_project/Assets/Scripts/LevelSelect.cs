using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {
    private Slider slider;
    public Text text;

	// Use this for initialization
	void Start () {
        slider = gameObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnLevelChanged);
	}
	
    void OnLevelChanged(float value)
    {
        GameData.level = (int)value;
        switch ((int)value)
        {
            case 1:
                text.text = "蔡徐坤";
                break;
            case 2:
                text.text = "菜鸟";
                break;
            case 3:
                text.text = "新手";
                break;
            case 4:
                text.text = "传说";
                break;
            case 5:
                text.text = "炼狱";
                break;
            default:
                text.text = "神秘";
                break;
        }
    }
}
