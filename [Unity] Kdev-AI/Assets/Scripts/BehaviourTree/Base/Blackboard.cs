using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard {
	public Dictionary<string, object> Data = new Dictionary<string, object>();

	public T GetData<T>(string name) {
		return Data.ContainsKey(name) ? (T) Data[name] : default(T);
	}

	public void SetData<T>(string name, T value) {
		if(Data.ContainsKey(name)) {
			Data[name] = value;
		} else {
			Data.Add(name, value);
		}
	}
}
