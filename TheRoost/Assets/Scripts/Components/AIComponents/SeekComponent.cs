using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class SeekComponent : IComponent
{
	private AIShipEntity entity;

	public SeekComponent(AIShipEntity entity)
	{
		this.entity = entity;
	}

	public void Update(float dt)
	{
		if (entity.CurrentTarget != null)
		{

		}
	}

	public void OnComponentEvent(ComponentEventId evt)
	{

	}
}
