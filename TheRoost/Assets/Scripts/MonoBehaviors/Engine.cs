//Engine.cs
//Description: This serves as the entry point for the program

using UnityEngine;
using System.Collections;
using Controllers;
using Game.Controllers;
using Services;

public class Engine : MonoBehaviour
{
	private FlowController m_gameFlow;

	public void Start ()
	{
		m_gameFlow = new FlowController();
		m_gameFlow.StartGame();
	}

	public void Update()
	{
		Service.FrameUpdate.Update(this);
	}
}
