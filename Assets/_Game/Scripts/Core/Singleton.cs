using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Ins;

    public static T Ins
    {
        get
        {
            if (m_Ins == null)
            {
                m_Ins = FindAnyObjectByType<T>();

                if (m_Ins == null)
                {
                    GameObject singletonObject = new GameObject();
                    m_Ins = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";

                }

            }
            return m_Ins;
        }
    }

}
