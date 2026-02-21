using UnityEngine;
using System.Collections;

public class CMPException : MonoBehaviour
{
    public string output = "";
    public string stack = "";
    
    void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }
    void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        //stack = stackTrace;
        Global.MPDebug("Handle" + logString);
    }
}
