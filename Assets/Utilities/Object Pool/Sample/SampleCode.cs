using System.Collections;
using Anonymous.Pooling;
using UnityEngine;

public class SampleCode : MonoBehaviour
{
	private GameObject one, two;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			var go = ObjectPool.Rent("Cube");
			
			StartCoroutine(Return(go));
		}
	}

	private IEnumerator Return(GameObject go)
	{
		yield return new WaitForSeconds(3.0f);

		ObjectPool.Return(go);
	}
}