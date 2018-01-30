using UnityEngine;

public class pin : MonoBehaviour
{

    public god m_godscript;

    private Rigidbody m_rigid;
    private bool m_is_registed;

    // Use this for initialization
    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_is_registed = false;
    }

    // Update is called once per frame
    void Update()
    {
        var rot = m_rigid.rotation;
        if (!m_is_registed && (Mathf.Abs(rot.x) >= 45 || Mathf.Abs(rot.z) >= 45))
        {
            m_is_registed = true;
            m_godscript.AddPoints(10);
        }

    }
}
