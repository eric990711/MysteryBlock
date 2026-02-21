using UnityEngine;
using System.Collections;

public class FadeEffect : MonoBehaviour {

    ////------------------------------------------------------------------------------------------------------------------------------------------------------
    //// public
    ////------------------------------------------------------------------------------------------------------------------------------------------------------
    public float m_MinValFrom0to255 = 100f;
    public float m_Speed = 200f;


    ////------------------------------------------------------------------------------------------------------------------------------------------------------
    //// private
    ////------------------------------------------------------------------------------------------------------------------------------------------------------
    private float m_value = 1;
    private int m_direction = -1;
    private bool m_flag = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        if (!m_flag) return;

        if (m_value >= 255) m_direction = -1;
        else if (m_value <= m_MinValFrom0to255) m_direction = 1;

        m_value = (float)m_value + (m_direction * Time.deltaTime * m_Speed);
        gameObject.renderer.material.SetColor("_Color", new Color(m_value / 255, m_value / 255, m_value / 255));
	}

    public void EffectOn()
    {
        m_flag = true;
    }

    public void EffectOff()
    {
        m_flag = false;
        gameObject.renderer.material.SetColor("_Color", new Color(1, 1, 1));
    }
}
