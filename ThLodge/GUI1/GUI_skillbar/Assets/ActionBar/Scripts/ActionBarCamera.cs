using UnityEngine;

[ExecuteInEditMode]
public class ActionBarCamera : MonoBehaviour
{
    static ActionBarCamera instance = null;

    public static ActionBarCamera Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(ActionBarCamera)) as ActionBarCamera;
            }

            return instance;
        }
    }

    [SerializeField]
    float viewArea = 500f;

    void Start()
    {
        if (!GetComponent<Camera>())
        {
            gameObject.AddComponent<Camera>();
        }
        
        instance = this;
        GetComponent<Camera>().orthographic = true;
        GetComponent<Camera>().nearClipPlane = 0;
        GetComponent<Camera>().farClipPlane = viewArea;
        GetComponent<Camera>().depth = 1;
        GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        GetComponent<Camera>().cullingMask = 1 << gameObject.layer;
        transform.position = new Vector3(0, 0, -(viewArea/2f));
    }

    void Awake()
    {
        Start();
    }

    void OnEnable()
    {
        Start();
    }

    void Update()
    {
        if (GetComponent<Camera>())
        {
            GetComponent<Camera>().orthographicSize = Screen.height / 2;
        }
    }

    void OnRenderObject()
    {
        Update();
    }
}