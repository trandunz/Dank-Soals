using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CallBackSystem : MonoBehaviour
{
    public delegate void OnMessageRecieved();
    public event OnMessageRecieved m_OnComplete;

    // Start is called before the first frame update
    void Start()
    {
        // EXAMPLE ONE (no event)
        //OnMessageRecieved test = WriteMessage;
        //test();

        // EXAMPLE TWO (event)
        m_OnComplete += WriteMessage;
        m_OnComplete();
        m_OnComplete -= WriteMessage;
    }

    void WriteMessage()
    {
        Debug.Log("WriteMessage() Called");
    }
}
