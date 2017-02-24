using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour {

    public Camera m_Camera = null;

    void Update()
    {
        if(m_Camera != null)
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);

    }
}
