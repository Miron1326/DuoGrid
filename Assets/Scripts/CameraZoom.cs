using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera m_Camera;
    private float minZoom = 10f;
    private float maxZoom = 20f;
    private bool canZoom = true;
    private void Start()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canZoom = !canZoom;
        }
        if (scroll != 0 && canZoom)
        {
            m_Camera.orthographicSize -= scroll * 2f;
            m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, minZoom, maxZoom);
        }
    }
}
