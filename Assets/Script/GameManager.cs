using UnityEngine;
using System.Collections;
using Tango;

public class GameManager : MonoBehaviour, ITangoLifecycle, ITangoDepth {

    public TangoApplication m_tangoApplication;
    public TangoPointCloud m_pointCloud;
    public GameObject m_prefabMarker;

    private bool m_bDepthDataAvailable;
    public bool WaitingCreateWorld { get; set; }

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

        DebugConsole.Log("Game Manager Started");
	}
	
	// Update is called once per frame
	void Update () {
        if(WaitingCreateWorld){
            StartCoroutine(CreateWorld());
            WaitingCreateWorld = false;
        }	    
	}


    // ITangoLifecycle
    public void OnTangoPermissions(bool permissionsGranted) 
    {
    }

    public void OnTangoServiceConnected()
    {
        m_tangoApplication.SetDepthCameraRate(TangoEnums.TangoDepthCameraRate.DISABLED);
    }

    public void OnTangoServiceDisconnected()
    {
    }

    // ITangoDepth
    public void OnTangoDepthAvailable(Tango.TangoUnityDepth tangoDepth)
    {
        m_bDepthDataAvailable = true;
    }

    private IEnumerator CreateWorld()
    {
        m_bDepthDataAvailable = false;
        if (m_tangoApplication == null)
        {
            yield break;
        }
        if (m_pointCloud == null)
        {
            yield break;
        }

        m_tangoApplication.SetDepthCameraRate(TangoEnums.TangoDepthCameraRate.MAXIMUM);
        while (!m_bDepthDataAvailable)
        {
            yield return null;
        }

        DebugConsole.Log("m_bDepthDataAvailable true");
        //if flag set, then the depth data is ok for finding plane
        m_tangoApplication.SetDepthCameraRate(TangoEnums.TangoDepthCameraRate.DISABLED);

        Vector2 screenPos = new Vector2(Screen.width/2, Screen.height/2);
        Camera cam = Camera.main;
        Vector3 planeCenter;
        Plane plane;

        
        if (!m_pointCloud.FindPlane(cam, screenPos, out planeCenter, out plane))
        {
            DebugConsole.Log("fail to find plane on position");
            yield break;
        }
        DebugConsole.Log(string.Format("screenPos x={0}, y={1}", screenPos.x, screenPos.y));

        //calc the game world face and location
        Vector3 up = plane.normal;
        Vector3 forward;
        if(Vector3.Angle(plane.normal, cam.transform.forward) < 175){
            Vector3 right = Vector3.Cross(up, cam.transform.forward).normalized;
            forward = Vector3.Cross(up, right);
        }else{
            forward = Vector3.Cross(up, cam.transform.right);
        }

        Vector3 dropPos = planeCenter + plane.normal * 0.3f;

        Instantiate(m_prefabMarker, dropPos, Quaternion.LookRotation(forward, up));

        WaitingCreateWorld = false;
    }
}
