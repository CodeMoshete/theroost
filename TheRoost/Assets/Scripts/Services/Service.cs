﻿using UnityEngine;
using System.Collections;
using Game.Controllers;

namespace Services
{
	public static class Service
	{
		private static EventService eventService;
		public static EventService Events
		{
			get
			{
				if (eventService == null)
				{
					eventService = new EventService ();
				}
				return eventService;
			}
			private set{ }
		}

		private static NetService netService;
		public static NetService Network
		{
			get
			{
				return netService;
			}
			set 
			{
				// Set at game start by scene object.
				if (netService == null)
				{
					netService = value;
				}
			}
		}

		private static FrameTimeUpdateService frameService;
		public static FrameTimeUpdateService FrameUpdate
		{
			get
			{
				if (frameService == null)
				{
					frameService = new FrameTimeUpdateService ();
				}

				return frameService;
			}
		}

		private static FXService fxService;
		public static FXService FXService
		{
			get
			{
				if (fxService == null)
				{
					fxService = new FXService ();
				}

				return fxService;
			}
		}
	}
}