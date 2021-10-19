using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CapFPS : MonoBehaviour
{
    [SerializeField] int m_TargetFramerate = 60;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = m_TargetFramerate;
    }

    void Update()
    {
        if (Application.targetFrameRate != m_TargetFramerate)
        {
            Application.targetFrameRate = m_TargetFramerate;
        }
    }
}
