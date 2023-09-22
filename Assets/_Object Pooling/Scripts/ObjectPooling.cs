using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Anonymous.Pooling
{
    public class ObjectPooling
    {
        private static ObjectPooling _default;
        private readonly Dictionary<string, ObjectPool> pools;
        private readonly Dictionary<string, List<GameObject>> spawns = new();

        private ObjectPooling()
        {
            var installer = Resources.Load("Object Pooling/Installer") as Installer;
            pools = installer.Pools;
        }

        public static ObjectPooling Default => _default ??= new ObjectPooling();

        public ObjectPool GetPool(string key)
        {
            return pools[key];
        }

        public void SetPool(ObjectPool pool)
        {
            var key = pool.Pool.name;
            if (!pools.ContainsKey(key))
            {
                Debug.LogError("This pool does not exist in the list.");
                return;
            }

            pools[key] = pool;
        }

        public void AddPool(ObjectPool pool)
        {
            var key = pool.Pool.name;
            if (pools.ContainsKey(key))
            {
                Debug.LogError("The pool already exists in the list.");
                return;
            }

            pools.Add(key, pool);
        }

        public GameObject Rent(string key)
        {
            if (isNotBound(key))
                return spawns[key][0];

            var rent = spawns[key].FirstOrDefault(spawn => !spawn.activeSelf);
            if (rent == null)
            {
                rent = Object.Instantiate(pools[key].Pool);   
                spawns[key].Add(rent);
            }
            
            var temporary = rent.AddComponent<ObjectPoolingTemporary>();
            temporary.Enable(pools[key].RemoveWaitTime);
            
            rent.SetActive(true);
            return rent;
        }

        public void Return(GameObject rent)
        {
            var temporary = rent.GetComponent<ObjectPoolingTemporary>();
            if (temporary != null)
                temporary.Disable();
            
            rent.SetActive(false);
        }

        private bool isNotBound(string key)
        {
            if (spawns.ContainsKey(key))
                return false;

            var list = new List<GameObject>();
            for (var i = 0; i < pools[key].StartSpawnCount; i++)
            {
                var go = Object.Instantiate(pools[key].Pool);
                go.SetActive(i == 0);
                list.Add(go);
            }

            spawns[key] = list;
            return true;
        }
    }
}