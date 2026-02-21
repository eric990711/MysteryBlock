using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TalkPos
{
    Left, Right, None
}


public class TutorialModelController : MonoBehaviour {


    public GameObject TalkLeft;
    public GameObject TalkRight;

    public TalkPos modelPos = TalkPos.None;

    public List<string> AniList = new List<string>();

    

    void Start()
    {
        

    }

    // 모델 교체



    // 모델이 말하는 애니메이션

    public IEnumerator PlayTalkAnim()
    {
        GameObject model = null;
        switch (modelPos)
        {
            case TalkPos.Left: model = TalkLeft; break;
            case TalkPos.Right: model = TalkRight; break;
            case TalkPos.None: model = null; break;
        }

        if (model == null) yield break;

        Animation modelAni = model.GetComponent<Animation>();
        if (modelAni == null) yield break;

        AniList.Clear();
        foreach (AnimationState state in modelAni)
        {
            AniList.Add(state.name);
        }

        if (AniList.Count < 2) yield break;

        // AniList 범위 안에서만 랜덤 선택 (인덱스 1부터 끝까지)
        int rnd = Random.Range(1, AniList.Count);

        Debug.Log(AniList[rnd]);
        model.GetComponent<Animation>().Play(AniList[rnd]);
        AnimationClip clip = model.GetComponent<Animation>().GetClip(AniList[rnd]);
        if (clip != null)
            yield return new WaitForSeconds(clip.length);
    }

    public IEnumerator PlayIdleAnim()
    {
        GameObject model = null;

        switch (modelPos)
        {
            case TalkPos.Left: model = TalkLeft; break;
            case TalkPos.Right: model = TalkRight; break;
            case TalkPos.None: model = null; break;
        }

        if (model == null) yield break;
        if (AniList.Count == 0) yield break;

        model.GetComponent<Animation>().Play(AniList[0]);
        yield return null;
    }

    public IEnumerator PlayAnimCombination()
    {
        yield return StartCoroutine(PlayTalkAnim());
        yield return StartCoroutine(PlayIdleAnim());
    }

    // 터치시 액션



    //애니메이션 리스트 초기화
    public void ResetAniList(GameObject newObj)
    {

        ClearAniList();
        SetAniList(newObj);
    }

    
    //애니메이션 세팅
    void SetAniList(GameObject obj)
    {
        Animation anim = obj.GetComponent<Animation>();
        foreach (AnimationState anistate in anim)
        {
            AniList.Add(anistate.name);
        }
    }

    //애니메이션 리스트 클리어
    void ClearAniList()
    {
        AniList.Clear();
    }




}
