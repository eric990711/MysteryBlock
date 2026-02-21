using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SceneGrid : MonoBehaviour {

public bool                 m_isGrid = false;
    public int                  m_widthCount = 10;
    public int                  m_heightCount = 10;
	public Vector2				m_StartPoint= new Vector2(0,0);
	public Vector2				m_Size = new Vector2(100,100);
		
	
    ////------------------------------------------------------------------------------------------------------------------------
    //// private
    ////------------------------------------------------------------------------------------------------------------------------
    static Material lineMaterial;
    private float               m_widthInterval = 0f;
    private float               m_heightInterval = 0f;


	private bool _started = false; 
	private Camera _camera ; 
		
	// Use this for initialization
	void Start () {
	}
	
	
	// Update is called once per frame
	void Update () {
	}
	
		
	public void OnDrawGizmosSelected() 
	{
		if( m_isGrid == false ) return; 
				
		DrawGrid();
	}
		
	
	public void DrawGrid() {
		
		Gizmos.color = Color.white;
		
		m_widthInterval = ((m_StartPoint.x+m_Size.x) - m_StartPoint.x)/ m_widthCount ;
		m_heightInterval = (m_StartPoint.y - ( m_StartPoint.y+m_Size.y))/m_heightCount;
				
		       
		
        for (int i = 0; i < m_widthCount + 1; i++)
        {
			Gizmos.DrawLine(new Vector2(m_StartPoint.x + (i*m_widthInterval ), m_StartPoint.y), 
							new Vector2(m_StartPoint.x + (i*m_widthInterval ), m_StartPoint.y-m_Size.y ));
		}
						
        for (int i = 0; i < m_heightCount + 1; i++)
        {
			Gizmos.DrawLine(new Vector2(m_StartPoint.x  , 		  m_StartPoint.y + (i * m_heightInterval)), 
							new Vector2(m_StartPoint.x+m_Size.x , m_StartPoint.y + (i * m_heightInterval) ));
		}
		
	}
	
	 public void OnEnable() 
	{
		if (_started) return;
		_camera = camera;
		_camera.transparencySortMode = TransparencySortMode.Orthographic;
		_started = true;
	}
	
	
}
