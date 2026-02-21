using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/*
static class _UPROTOCOL
{
    public const string LOGIN = "http://www.hello5.kr:8080/spaceticket/module/
 * .jsp";
    public const string ENTERGAME = "http://www.hello5.kr:8080/spaceticket/module/enterGame.jsp";
}
*/


public class UI_InGameTermOKBtn : MonoBehaviour
{

	// Use this for initialization


    void OnClick()
    {
        if (gameObject.name == "AgreeBtn")
        {
            HelpBox.Instance.CloseTermView();
        }

    }

}
