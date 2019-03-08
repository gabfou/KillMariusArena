using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GamePref {

    [Header("Controle")]
    public KeyCode attack1 = KeyCode.RightControl;
    public KeyCode attack2 = KeyCode.Joystick1Button2;
    public KeyCode jump1 = KeyCode.Space;
    public KeyCode jump2 = KeyCode.Joystick1Button0;
    public KeyCode dash1 = KeyCode.RightShift;
    public KeyCode dash2 = KeyCode.Joystick1Button6;

	public void SetAttack1()
	{

	}
}