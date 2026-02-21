using UnityEngine;
using System.Collections;

public class TutorialBtnMgr : MonoBehaviour
{
    float fTutorialDelay = 0f;
    bool bPickMachine = false;

    void Start()
    {
        
    }

    void OnClick()
    {
        Debug.Log("Click");
        if (gameObject.name == "TutoRightArrow")
        {
            LobbyMgr.Instance.tutoLobby.FingerTouch[0].SetActive(false);
            fTutorialDelay = GameObject.Find("GameLobby/LobbyCamera").GetComponent<LobbyCamera>().mTwRotation.duration;
            GameObject.Find("GameLobby/LobbyCamera").GetComponent<LobbyCamera>().CameraMove = emCameraMove.Right;
            LobbyMgr.Instance.tutoLobby.eventArrow.GetComponent<Collider>().enabled = false;
            Invoke("ClearEvent", fTutorialDelay);
        }
        else if (gameObject.name == "SkipBtn")
        {
            MessageBox.Instance.OpenMessageBox(emMsgType.TUTO_OUT);
        }
        else if (gameObject.name == "StartButton")
        {
            MysteryMgr.Instance.tutoIngame.isClearMission = true;
            MysteryMgr.Instance.tutoIngame.Finger[0].SetActive(false);
            MysteryMgr.Instance.tutoIngame.dlg++;
            MysteryMgr.Instance.tutoIngame.Ingamedialog = MysteryMgr.Instance.tutoIngame.dlg;
        }
        

    }


    void ClearEvent()
    {
        LobbyMgr.Instance.tutoLobby.isClearMission = true;
        LobbyMgr.Instance.tutoLobby.eventArrow.SetActive(false);
    }

}
