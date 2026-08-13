
using UnityEngine;
using System.Collections.Generic;


public static class ParticlePool
{
    const int DEFAULT_POOL_SIZE = 3;

    private static Transform root;

    public static Transform Root
    {
        get
        {
            if (root == null)
            {
                PoolControler controler = GameObject.FindAnyObjectByType<PoolControler>();

                root = controler != null ? controler.transform : new GameObject("ParticlePool").transform;
            }

            return root;
        }
    }

    class Pool
    {
        Transform m_sRoot = null;

        List<ParticleSystem> inactive;

        ParticleSystem prefab;

        int index;

        public Pool(ParticleSystem prefab, int initialQty, Transform parent)
        {
#if UNITY_EDITOR
            if (prefab.main.loop)
            {
                ParticleSystem.MainModule main = prefab.main;
                main.loop = false;

                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Not Loop");
                Debug.Log(prefab.name + " ~ Fix To Not Loop");
            } 
            
            if (prefab.main.playOnAwake)
            {
                ParticleSystem.MainModule main = prefab.main;
                main.playOnAwake = false;

                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Not PlayAwake");
                Debug.Log(prefab.name + " ~ Fix To Not PlayAwake");
            }

            if (prefab.main.stopAction != ParticleSystemStopAction.None)
            {
                ParticleSystem.MainModule main = prefab.main;
                main.stopAction = ParticleSystemStopAction.None;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Stop Action None");
                Debug.Log(prefab.name + " ~ Fix To  Stop Action None");
            }   
            
            if (prefab.main.duration > 1)
            {
                ParticleSystem.MainModule main = prefab.main;
                main.duration = 1;

                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Duration By 1");
                Debug.Log(prefab.name + " ~ Fix To Duration By 1");
            }
#endif

            m_sRoot = parent;
            this.prefab = prefab;
            inactive = new List<ParticleSystem>(initialQty);

            for (int i = 0; i < initialQty; i++)
            {
                ParticleSystem particle = (ParticleSystem)GameObject.Instantiate(prefab, m_sRoot);
                particle.Stop();
                inactive.Add(particle);
            }
        }

        public int Count {
            get { return inactive.Count;}
        }

        public ParticleSystem Play(Vector3 pos, Quaternion rot)
        {
            index = index + 1 < inactive.Count ? index + 1 : 0;

            ParticleSystem obj = inactive[index];

            if (obj.isPlaying)
            {
                obj.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            obj.transform.SetPositionAndRotation( pos, rot);
            obj.Play();

            return obj;
        }
    }

    static Dictionary<int, Pool> pools = new Dictionary<int, Pool>();

    static void Init(ParticleSystem prefab = null, int qty = DEFAULT_POOL_SIZE, Transform parent = null)
    {
        if (prefab != null && !pools.ContainsKey(prefab.GetInstanceID()))
        {
            pools[prefab.GetInstanceID()] = new Pool(prefab, qty, parent);
        }
    }

    static public void Preload(ParticleSystem prefab, int qty = 1, Transform parent = null)
    {
        Init(prefab, qty, parent);
    }

    static public ParticleSystem Play(ParticleSystem prefab, Vector3 pos, Quaternion rot)
    {
#if UNITY_EDITOR
        if (prefab == null)
        {
            return null;
        }
#endif

        if (!pools.ContainsKey(prefab.GetInstanceID()))
        {
            Transform newRoot = new GameObject("VFX_" + prefab.name).transform;
            newRoot.SetParent(Root);
            pools[prefab.GetInstanceID()] = new Pool(prefab, 10, newRoot);
        }

        

        return pools[prefab.GetInstanceID()].Play(pos, rot);
    }

}

