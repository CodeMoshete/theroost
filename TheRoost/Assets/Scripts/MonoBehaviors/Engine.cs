﻿//Engine.cs
//Description: This serves as the entry point for the program

using UnityEngine;
using System.Collections;
using Controllers;
using Game.Controllers;
using Services;

public class Engine : MonoBehaviour
{
	private FlowController gameFlow;
	private DebugCameraController debugCam;

	public void Start ()
	{
		gameFlow = new FlowController();
		gameFlow.StartGame();
		debugCam = new DebugCameraController ();
	}

	public void Update()
	{
		Service.FrameUpdate.Update(this);
	}
}
