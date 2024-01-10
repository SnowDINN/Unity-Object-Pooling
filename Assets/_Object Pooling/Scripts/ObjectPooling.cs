using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Anonymous.Pooling
{
	public class ObjectPooling
	{
		private static readonly Dictionary<string, Dictionary<int, GameObject>> spawns = new();
		private static Dictionary<string, ObjectPool> pools;

		public static void Initialize()
		{
			var installer = Resources.Load("Object Pooling/Installer") as Installer;
			if (installer != null)
				pools = installer.Pools;
		}

		public static ObjectPool GetPool(string key)
		{
			return pools[key];
		}

		public static void SetPool(ObjectPool pool)
		{
			var key = pool.Pool.name;
			if (!pools.ContainsKey(key))
			{
				Debug.LogError("This pool does not exist in the list.");
				return;
			}

			pools[key] = pool;
		}

		public static void AddPool(ObjectPool pool)
		{
			var key = pool.Pool.name;
			if (pools.ContainsKey(key))
			{
				Debug.LogError("The pool already exists in the list.");
				return;
			}

			pools.Add(key, pool);
		}

		public static GameObject Rent(string key)
		{
			if (isNotBound(key))
				return spawns[key].First().Value;

			var rent = spawns[key].FirstOrDefault(spawn => !spawn.Value.activeSelf).Value;
			if (rent == null)
			{
				rent = Object.Instantiate(pools[key].Pool);
				rent.name = $"{key}={rent.GetInstanceID()}";
				
				spawns[key].Add(rent.GetInstanceID(), rent);
				if (spawns[key].Count > pools[key].StartSpawnCount)
				{
					var temporary = rent.AddComponent<ObjectPoolingTemporary>();
					temporary.Enable(pools[key].RemoveWaitTime);
				}
				else
				{
					var temporary = rent.GetComponent<ObjectPoolingTemporary>();
					if (temporary != null)
						temporary.Enable(pools[key].RemoveWaitTime);
				}
			}

			rent.SetActive(true);
			return rent;
		}

		public static void Return(GameObject rent)
		{
			var split = rent.name.Split("=");
			
			var temporary = rent.GetComponent<ObjectPoolingTemporary>();
			if (temporary != null)
				temporary.Disable(() => spawns[split[0]].Remove(int.Parse(split[1])));

			rent.SetActive(false);
		}

		private static bool isNotBound(string key)
		{
			if (spawns.ContainsKey(key))
				return false;

			var list = new Dictionary<int, GameObject>();
			for (var i = 0; i < pools[key].StartSpawnCount; i++)
			{
				var go = Object.Instantiate(pools[key].Pool);
				go.name = $"{key}={go.GetInstanceID()}";
				go.SetActive(i == 0);
				
				list.Add(go.GetInstanceID(), go);
			}

			spawns[key] = list;
			return true;
		}
	}
}