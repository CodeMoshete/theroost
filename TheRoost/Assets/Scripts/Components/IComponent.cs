using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponent
{
	void Update(float dt);
	void OnComponentEvent (ComponentEventId evt);
}
