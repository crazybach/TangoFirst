using UnityEngine;
using System.Collections;
using Tango;

public class GameManager : MonoBehaviour, ITangoLifecycle {

    public TangoApplication m_tangoApplication;
    public TangoPointCloud m_pointCloud;
  

	// Use this for initialization
	void Start () {
        if (m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
        }

        if (m_tangoApplication.IsServiceConnected)
        {
            OnTangoServiceConnected();
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTangoPermissions(bool permissionsGranted) 
    {
    }

    public void OnTangoServiceConnected()
    {
    }

    public void OnTangoServiceDisconnected()
    {
    }


    public void CreateWorld()
    {
        if (m_tangoApplication == null)
        {
            return;
        }
        if (m_pointCloud == null)
        {
            return;
        }

        Vector2 screenPos = new Vector2(Screen.width/2, Screen.height/2);
        Camera cam = Camera.main;
        Vector3 planeCenter;
        Plane plane;

        Debug.Log(string.Format("screenPos x={0}, y={1}", screenPos.x, screenPos.y));
        //if (!m_pointCloud.FindPlane(cam, screenPos, out planeCenter, out plane))
        //{
        //    return;
        //}
    }
}
