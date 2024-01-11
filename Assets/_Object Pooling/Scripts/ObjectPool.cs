using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Anonymous.Pooling
{
	public class ObjectPool
	{
		private static readonly Dictionary<string, Dictionary<int, GameObject>> spawns = new();
		private static Dictionary<string, Pool> pools;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void Initialize()
		{
			var installer = Resources.Load("Object Pooling/Installer") as Installer;
			if (installer != null)
				pools = installer.Pools;
		}

		public static Pool GetPool(string key)
		{
			return pools[key];
		}

		public static void SetPool(Pool pool)
		{
			var key = pool.Prefab.name;
			if (!pools.ContainsKey(key))
			{
				Debug.LogError("This pool does not exist in the list.");
				return;
			}

			pools[key] = pool;
		}

		public static void AddPool(Pool pool)
		{
			var key = pool.Prefab.name;
			if (!pools.TryAdd(key, pool))
				Debug.LogError("The pool already exists in the list.");
		}

		public static GameObject Rent(string key)
		{
			if (isNotBound(key))
				Debug.Log($"<color=green>Create new pool group : </color><color=yellow>{key}</color>");

			var rent = spawns[key].FirstOrDefault(spawn => !spawn.Value.activeSelf).Value;
			if (rent == null)
			{
				rent = Object.Instantiate(pools[key].Prefab);
				rent.name = $"{key} - {rent.GetInstanceID()}";

				spawns[key].Add(rent.GetInstanceID(), rent);
				if (spawns[key].Count > pools[key].MaintainCount)
				{
					var temporary = rent.AddComponent<ObjectPoolTemporary>();
					temporary.Enable(pools[key].RemoveDelayTime);
				}
			}
			else
			{
				var temporary = rent.GetComponent<ObjectPoolTemporary>();
				if (temporary != null)
					temporary.Enable(pools[key].RemoveDelayTime);
			}
			
			rent.transform.position = pools[key].Prefab.transform.position;
			rent.transform.rotation = pools[key].Prefab.transform.rotation;
			rent.transform.localScale = pools[key].Prefab.transform.localScale;

			rent.SetActive(true);
			return rent;
		}

		public static void Return(GameObject rent)
		{
			var split = rent.name.Split(" - ");

			var temporary = rent.GetComponent<ObjectPoolTemporary>();
			if (temporary != null)
				temporary.Disable(() => spawns[split[0]].Remove(int.Parse(split[1])));

			rent.SetActive(false);
		}

		private static bool isNotBound(string key)
		{
			if (spawns.ContainsKey(key))
				return false;

			spawns[key] = new Dictionary<int, GameObject>();
			return true;
		}
	}
}