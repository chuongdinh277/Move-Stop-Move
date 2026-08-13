using UnityEngine;
using System.Collections.Generic;
using System;

public static class SimplePool
{
    class Pool
    {
        Transform m_sRoot = null;
        bool m_collect;
        Queue<GameUnit> m_inactive;
        HashSet<GameUnit> m_active;
        GameUnit m_prefab;

        public bool IsCollect { get => m_collect; }
        public HashSet<GameUnit> Active => m_active;
        public int Count => m_inactive.Count + m_active.Count;
        public Transform Root => m_sRoot;

        public Pool(GameUnit prefab, int initialQty, Transform parent, bool collect)
        {
            m_inactive = new Queue<GameUnit>(initialQty);
            m_sRoot = parent;
            this.m_prefab = prefab;
            m_collect = collect;
            if (m_collect) m_active = new HashSet<GameUnit>();
        }

        public GameUnit Spawn(Vector3 pos, Quaternion rot)
        {
            GameUnit obj = Spawn();

            obj.TF.SetPositionAndRotation(pos, rot);

            return obj;
        }

        public GameUnit Spawn()
        {
            GameUnit obj;
            if (m_inactive.Count == 0)
            {
                obj = (GameUnit)GameObject.Instantiate(m_prefab, m_sRoot);
            }
            else
            {
                obj = m_inactive.Dequeue();

                if (obj == null)
                {
                    return Spawn();
                }
            }

            if (m_collect) m_active.Add(obj);

            obj.gameObject.SetActive(true);

            return obj;
        }

        public void Despawn(GameUnit obj)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
                m_inactive.Enqueue(obj);

                if (memberInParent.Contains(obj.GetInstanceID()))
                {
                    obj.TF.SetParent(GetPool(obj).Root);
                    memberInParent.Remove(obj.GetInstanceID());
                }
            }

            if (m_collect) m_active.Remove(obj);
        }

        public void Release()
        {
            while (m_inactive.Count > 0)
            {
                GameUnit go = m_inactive.Dequeue();
                GameObject.DestroyImmediate(go);
            }
            m_inactive.Clear();
        }

        public void Collect()
        {

            HashSet<GameUnit> units = new HashSet<GameUnit>(m_active);
            List<GameUnit> unitsList = new List<GameUnit>(units);
            for (int i = 0; i < unitsList.Count; i++)
            {
                Despawn(unitsList[i]);
            }
        }
    }

    public const int DEFAULT_POOL_SIZE = 3;

    static Dictionary<PoolType, GameUnit> poolTypes = new Dictionary<PoolType, GameUnit>();

    static HashSet<int> memberInParent = new HashSet<int>();

    private static Transform root;

    public static Transform Root
    {
        get
        {
            if (root == null)
            {
                root = new GameObject("Pool").transform;
            }

            return root;
        }
    }

    static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();

    static public void Preload(GameUnit prefab, int qty = 1, Transform parent = null, bool collect = false)
    {
        if (!poolTypes.ContainsKey(prefab.poolType))
        {
            poolTypes.Add(prefab.poolType, prefab);
        }

        if (prefab == null)
        {
            Debug.LogError(parent.name + " : IS EMPTY!!!");
            return;
        }

        InitPool(prefab, qty, parent, collect);

        GameUnit[] obs = new GameUnit[qty];
        for (int i = 0; i < qty; i++)
        {
            obs[i] = Spawn(prefab);
        }

        for (int i = 0; i < qty; i++)
        {
            Despawn(obs[i]);
        }
    }

    static void InitPool(GameUnit prefab = null, int qty = DEFAULT_POOL_SIZE, Transform parent = null, bool collect = false)
    {
        if (prefab != null && !IsHasPool(prefab))
        {
            poolInstance.Add(prefab.poolType, new Pool(prefab, qty, parent, collect));
        }
    }


    static private bool IsHasPool(GameUnit obj)
    {
        return poolInstance.ContainsKey(obj.poolType);
    }

    static private Pool GetPool(GameUnit obj)
    {
        return poolInstance[obj.poolType];
    }

    public static GameUnit GetPrefabByType(PoolType poolType)
    {
        if (!poolTypes.ContainsKey(poolType) || poolTypes[poolType] == null)
        {
            GameUnit[] resources = Resources.LoadAll<GameUnit>("Pool");

            for (int i = 0; i < resources.Length; i++)
            {
                poolTypes[resources[i].poolType] = resources[i];
            }
        }

        return poolTypes[poolType];
    }

    #region Get List object ACTIVE
    public static HashSet<GameUnit> GetAllUnitIsActive(GameUnit obj)
    {
        return IsHasPool(obj) ? GetPool(obj).Active : new HashSet<GameUnit>();
    }
    public static HashSet<GameUnit> GetAllUnitIsActive(PoolType poolType)
    {
        return GetAllUnitIsActive(GetPrefabByType(poolType));
    }  

    #endregion

    #region Spawn
    static public T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T : GameUnit
    {
        return Spawn(GetPrefabByType(poolType), pos, rot) as T;
    }
    static public T Spawn<T>(PoolType poolType) where T : GameUnit
    {
        return Spawn<T>(GetPrefabByType(poolType));
    }
    static public T Spawn<T>(GameUnit obj, Vector3 pos, Quaternion rot) where T : GameUnit
    {
        return Spawn(obj, pos, rot) as T;
    }
    static public T Spawn<T>(GameUnit obj) where T : GameUnit
    {
        return Spawn(obj) as T;
    }

    static public T Spawn<T>(GameUnit obj, Transform parent) where T : GameUnit
    {
        return Spawn<T>(obj, obj.TF.localPosition, obj.TF.localRotation, parent);
    }

    static public T Spawn<T>(GameUnit obj, Vector3 localPoint, Quaternion localRot, Transform parent) where T : GameUnit
    {
        T unit = Spawn<T>(obj);
        unit.TF.SetParent(parent);
        unit.TF.localPosition = localPoint;
        unit.TF.localRotation = localRot;
        unit.TF.localScale = Vector3.one;
        memberInParent.Add(unit.GetInstanceID());
        return unit;
    }

    static public T Spawn<T> (PoolType poolType, Vector3 localPoint, Quaternion localRot, Transform parent) where T : GameUnit
    {
        return Spawn<T>(GetPrefabByType(poolType), localPoint, localRot, parent);
    }
    static public T Spawn<T> (PoolType poolType, Transform parent) where T : GameUnit
    {
        return Spawn<T>(GetPrefabByType(poolType), parent);
    }

    static public GameUnit Spawn(GameUnit obj, Vector3 pos, Quaternion rot)
    {
        if (!IsHasPool(obj))
        {
            Transform newRoot = new GameObject(obj.name).transform;
            newRoot.SetParent(Root);
            Preload(obj, 1, newRoot, true);
        }

        return GetPool(obj).Spawn(pos, rot);
    }
    static public GameUnit Spawn(GameUnit obj)
    {
        if (!IsHasPool(obj))
        {
            Transform newRoot = new GameObject(obj.name).transform;
            newRoot.SetParent(Root);
            Preload(obj, 1, newRoot, true);
        }

        return GetPool(obj).Spawn();
    }
    #endregion

    #region Despawn
    static public void Despawn(GameUnit obj)
    {
        if (obj.gameObject.activeSelf)
        {
            if (IsHasPool(obj))
            {
                GetPool(obj).Despawn(obj);
            }
            else
            {
                GameObject.Destroy(obj.gameObject);
            }
        }
    }
    #endregion

    #region Release
    static public void Release(GameUnit obj)
    {
        if (IsHasPool(obj))
        {
            GetPool(obj).Release();
        }
    }
    static public void Release(PoolType poolType)
    {
        Release(GetPrefabByType(poolType));
    }

    static public void ReleaseAll()
    {
        List<Pool> pools = new List<Pool>(poolInstance.Values);
        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].Release();
        }
    }
    #endregion

    #region Collect
    static public void Collect(GameUnit obj)
    {
        if (IsHasPool(obj)) GetPool(obj).Collect();
    }
    static public void Collect(PoolType poolType)
    {
        Collect(GetPrefabByType(poolType));
    }

    static public void CollectAll()
    {
        List<Pool> pools = new List<Pool>(poolInstance.Values);
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].IsCollect)
            {
                pools[i].Collect();
            }
        }
    }
    #endregion
}
