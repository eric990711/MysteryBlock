

using UnityEngine;
using System.Collections;

public class CMemberBase : MonoBehaviour
{


    public UITexture    UserTexture;
    public UILabel      NameLabel;
    public UILabel      TicketLabel;
    public UILabel      RankLabel;
    public UISprite     GradeSprite;

    GameObject          lineSpriteobj; 

    Texture DefaultTexture;

    /*
    public string ImgUrl; 
    public string Name;
    public string Ticket;
    public string Rank;
    */

    void Awake()
    {
        DefaultTexture = UserTexture.mainTexture;
        NameLabel.text = "";
        TicketLabel.text = "";
        lineSpriteobj = transform.Find("LineSprite").gameObject;
    }

    void Start()
    { }

    public void Reset()
    {
        NameLabel.text = "";
        TicketLabel.text = "";
        
        //NameLabel.gameObject.SetActive(false);
        //TicketLabel.gameObject.SetActive(false);
        
        UserTexture.gameObject.SetActive(false);
        RankLabel.gameObject.SetActive(false);
        GradeSprite.gameObject.SetActive(false);
        lineSpriteobj.SetActive(false);

        
        //RankLabel.text = "";
        
        SetGrade("0");
        UserTexture.mainTexture = DefaultTexture;
        //gameObject.SetActive(false);
    }

    public void SetRankData(string rank, string name, string ticket, string url)
    {
        //NameLabel.gameObject.SetActive(false);
        //TicketLabel.gameObject.SetActive(false);
        
        UserTexture.gameObject.SetActive(true);
        RankLabel.gameObject.SetActive(true);
        GradeSprite.gameObject.SetActive(true);
        lineSpriteobj.SetActive(true);   
        
        NameLabel.text = name;
        TicketLabel.text = MPUtil.MoneyFormatString(ticket);
        RankLabel.text = rank;
        SetGrade(rank);
        // TicketLabel.text = url; 


        StartCoroutine(UpdateUserSprite(url));

    }


    void SetGrade(string number)
    {

        if (number == "0")
            GradeSprite.spriteName = "";
        else if (number == "1")
            GradeSprite.spriteName = "Img_RankGold";
        else if (number == "2")
            GradeSprite.spriteName = "Img_RankSilver";
        else if (number == "3")
            GradeSprite.spriteName = "Img_RankBronz";
        else
            GradeSprite.spriteName = "Img_RankElse";

    }

    IEnumerator UpdateUserSprite(string url)
    {
        if (url == string.Empty)
        {
            yield break;
        }
        else
        {
            WWW www = new WWW(url);
            yield return www;
            UserTexture.mainTexture = www.texture;
        }
    }

}
