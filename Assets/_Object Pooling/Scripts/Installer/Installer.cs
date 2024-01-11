using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anonymous.Pooling
{
	[Serializable]
	public class Pool
	{
		public GameObject Prefab;
		public int MaintainCount;
		public float RemoveDelayTime;
	}

	[CreateAssetMenu(fileName = "Installer", menuName = "Pooling/Installer")]
	public class Installer : ScriptableObject
	{
		[SerializeField] private List<Pool> PoolList;

		private readonly Dictionary<string, Pool> field = new();

		/// <summary>
		/// Key is the name of the object.
		/// </summary>
		public Dictionary<string, Pool> Pools
		{
			get
			{
				if (field.Count != 0)
					return field;

				foreach (var pool in PoolList)
					field.Add(pool.Prefab.name, pool);
				return field;
			}
		}
	}
}