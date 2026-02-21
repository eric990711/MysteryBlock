using UnityEngine;
using System.Collections;


public class UI_LobbyButton : MonoBehaviour {

    bool bPush = false;

    public GameObject ArrowLeftObj = null;
    public GameObject ArrowRightObj = null;
    
    void Start()
    {

        //2014.04.15 이재우 로비창에서 0번머신에서 좌측버튼 클릭시 이벤트 머신으로 이동
        //First select machine
        /*if (GameClient.instance.mSelectMachine == 0)
        {
            ArrowLeftObj.SetActive(false);
        }*/
    }


    void OnClick()
    {
        if (GameClient.instance.isVisitor == true)
        {
            return;
        }

        if (LoadingBar.GetInstance().GetLock() == true)
            return;

        if (GameObject.Find("GameLobby/LobbyCamera").GetComponent<LobbyCamera>().CameraMove != emCameraMove.None) return;
        GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_Button);

        if (gameObject.name == "ArrowLeft")
        {
            if (GameClient.instance.mSelectMachine != 0)
            {
                GameObject.Find("GameLobby/LobbyCamera").GetComponent<LobbyCamera>().CameraMove = emCameraMove.Left;
            }
            else
            {
                GameObject.Find("GameLobby/LobbyCamera").GetComponent<LobbyCamera>().CameraMove = emCameraMove.LeftSkip;
            }

        }
        else if (gameObject.name == "ArrowRight")
        {
            if (GameClient.instance.mSelectMachine != 8)
            {
                GameObject.Find("GameLobby/LobbyCamera").GetComponent<LobbyCamera>().CameraMove = emCameraMove.Right;
            }
            else
            {
                GameObject.Find("GameLobby/LobbyCamera").GetComponent<LobbyCamera>().CameraMove = emCameraMove.RightSkip;
            }

        }
            
        


        //if (GameClient.instance.tutorial.isVisitor == true) return;
        



        //2014.04.15 이재우 로비창에서 0번머신에서 좌측버튼 클릭시 이벤트 머신으로 이동
        /*if (GameClient.instance.mSelectMachine > 0 && GameClient.instance.mSelectMachine < Global.MAXMACHINE)
        {
            ArrowRightObj.SetActive(true);
            ArrowLeftObj.SetActive(true);
        }
        
        if (GameClient.instance.mSelectMachine == Global.MAXMACHINE)
        {
            ArrowRightObj.SetActive(false);
        }
        else if (GameClient.instance.mSelectMachine == 0)
        {
            ArrowLeftObj.SetActive(false);
        }*/

    }

    void OnPress()
    {
        if (GameClient.instance.isVisitor == true)
        {
            return;
        }
        //if (GameClient.instance.tutorial.isVisitor == true) return;
        bPush ^= true;

        if (gameObject.name == "ArrowLeft")
        {
            ButtonActive(gameObject, bPush);
        }
        else if (gameObject.name == "ArrowRight")
        {
            ButtonActive(gameObject, bPush);
        }
    }

    void ButtonActive(GameObject obj, bool bPush)
    {

        if (bPush == true)
        {
            obj.transform.Find("NormalButton").gameObject.SetActive(false);
            obj.transform.Find("PressButton").gameObject.SetActive(true);
        }
        else
        {
            obj.transform.Find("NormalButton").gameObject.SetActive(true);
            obj.transform.Find("PressButton").gameObject.SetActive(false);
        }
    }

}
