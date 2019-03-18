using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GamePref {

    public KeyCode _attack1 = KeyCode.Return;
    public KeyCode attack1{get{return _attack1;} set{_attack1 = value; PlayerPrefs.SetInt("attack1", (int)value);}}
    public KeyCode _attack2 = KeyCode.Joystick1Button2;
    public KeyCode attack2{get{return _attack2;} set{_attack2 = value; PlayerPrefs.SetInt("attack2", (int)value);}}
    public KeyCode _jump1 = KeyCode.Space;
    public KeyCode jump1{get{return _jump1;} set{_jump1 = value; PlayerPrefs.SetInt("jump1", (int)value);}}
    public KeyCode _jump2 = KeyCode.Joystick1Button0;
    public KeyCode jump2{get{return _jump2;} set{_jump2 = value; PlayerPrefs.SetInt("jump2", (int)value);}}
    public KeyCode _dash1 = KeyCode.LeftShift;
    public KeyCode dash1{get{return _dash1;} set{_dash1 = value; PlayerPrefs.SetInt("dash1", (int)value);}}
    public KeyCode _dash2 = KeyCode.Joystick1Button6;
    public KeyCode dash2{get{return _dash2;} set{_dash2 = value; PlayerPrefs.SetInt("dash2", (int)value);}}

    public void init()
    {
        attack1 = (KeyCode)PlayerPrefs.GetInt("attack1", (int)KeyCode.Return);
        attack2 = (KeyCode)PlayerPrefs.GetInt("attack2", (int)KeyCode.Joystick1Button2);
        jump1 = (KeyCode)PlayerPrefs.GetInt("jump1", (int)KeyCode.Space);
        jump2 = (KeyCode)PlayerPrefs.GetInt("jump2", (int)KeyCode.Joystick1Button0);
        dash1 = (KeyCode)PlayerPrefs.GetInt("dash1", (int)KeyCode.LeftShift);
        dash2 = (KeyCode)PlayerPrefs.GetInt("dash2", (int)KeyCode.Joystick1Button6);
    }

	public void SetAttack1()
	{

	}
}