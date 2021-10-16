using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Player : Script_CharacterMotor
{
    #region Singleton
    private static Script_Player _instance;
    public static Script_Player Instance;
    #endregion

    void Start()
    {
        InitCursorLockMode(CursorLockMode.Locked);
        InitMotor();
    }

    void Update()
    {
        Motor();
    }
}
