using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anonymous.Pooling
{
	[Serializable]
	public class ObjectPool
	{
		public GameObject Pool;
		public int StartSpawnCount;
		public float RemoveWaitTime;
	}

	[CreateAssetMenu(fileName = "Installer", menuName = "Pooling/Installer")]
	public class Installer : ScriptableObject
	{
		[SerializeField] private List<ObjectPool> PoolList;

		private readonly Dictionary<string, ObjectPool> field = new();

		/// <summary>
		/// Key is the name of the object.
		/// </summary>
		public Dictionary<string, ObjectPool> Pools
		{
			get
			{
				if (field.Count != 0)
					return field;

				foreach (var pool in PoolList)
					field.Add(pool.Pool.name, pool);
				return field;
			}
		}
	}
}