//Engine.cs
//Description: This serves as the entry point for the program

using UnityEngine;
using System.Collections;
using Controllers;
using Game.Controllers;

public class Engine : MonoBehaviour
{
	private FrameTimeUpdateController m_updateController;
	private FlowController m_gameFlow;

	public void Start ()
	{
		m_updateController = FrameTimeUpdateController.GetInstance();
		m_gameFlow = new FlowController();
		m_gameFlow.StartGame();
	}

	public void Update()
	{
		m_updateController.Update(this);
	}
}
