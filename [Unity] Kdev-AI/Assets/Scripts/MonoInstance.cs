using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoInstance : MonoBehaviour {
	public static MonoInstance instance;

	private void Start() {
		instance = this;
	}

	/// <summary>
	/// Code source:
	/// https://forum.unity.com/threads/waitforseconds-without-monobehaviour.216081/
	/// </summary>
	public IEnumerator DoCoroutine(IEnumerator _coroutine) {
		while(_coroutine.MoveNext()) {
			yield return _coroutine.Current;
		}
	}
}
