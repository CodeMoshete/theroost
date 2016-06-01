//TransitionWrapper.cs
//Wrapper class for the transition screen.

using System;
using Controllers;
using Controllers.Interfaces;
using Delegates;
using Events;
using MonoBehaviors;
using MonoBehaviors.Interfaces;
using Services;
using UnityEngine;

namespace GameObjectWrappers
{
	public class TransitionWrapper : IUpdateObserver
	{
		private const string TRANSITION_OVER_EVENT = "transitionOver";
		private const string TRANSITION_IN_ID = "fadeIn";
		private const string TRANSITION_OUT_ID = "fadeOut";
		private const float FADE_DURATION = 1.5f; //seconds

		private GameObject m_wrappedObject;
		private DefaultDelegate m_onAnimationOver;
		private Animator m_animator;
		private float currentTime;

		private Transform vrCam;

		public TransitionWrapper (GameObject wrappedObject)
		{
			m_wrappedObject = wrappedObject;
			m_animator = m_wrappedObject.GetComponent<Animator>();
			vrCam = GameObject.Find ("Camera (head)").transform;
			ReParentTransitionWrapper (vrCam);
			Service.FrameUpdate.RegisterForUpdate (this);
			Service.Events.AddListener (EventId.DebugCameraControlsActive, OnDebugControlsToggled);
		}

		public void PlayTransitionIn(DefaultDelegate onAnimationOver = null)
		{
			m_onAnimationOver = onAnimationOver;
			m_animator.SetTrigger(TRANSITION_IN_ID);
			currentTime = FADE_DURATION;
		}

		public void PlayTransitionOut(DefaultDelegate onAnimationOver = null)
		{
			m_onAnimationOver = onAnimationOver;
			m_animator.SetTrigger(TRANSITION_OUT_ID);
			currentTime = FADE_DURATION;
		}

		public void SetActive(bool active)
		{
			m_wrappedObject.SetActive(active);
		}

		public void Update(float dt)
		{
			if (currentTime > 0f)
			{
				currentTime -= dt;

				if (currentTime <= 0f && m_onAnimationOver != null)
				{
					m_onAnimationOver();
					m_onAnimationOver = null;
				}
			}
		}

		public void OnAnimationEvent(string eventType)
		{
			if(eventType == TRANSITION_OVER_EVENT && m_onAnimationOver != null)
			{
				m_onAnimationOver();
				m_onAnimationOver = null;
			}
		}

		private void ReParentTransitionWrapper(Transform newParent)
		{
			m_wrappedObject.transform.SetParent (newParent);
			m_wrappedObject.transform.localPosition = Vector3.zero;
		}

		private void OnDebugControlsToggled(object cookie)
		{
			bool isActive = (bool)cookie;
			if (isActive)
			{
				Transform debugCamera = GameObject.Find (DebugCameraController.DEBUG_CAMERA_NAME).transform;
				ReParentTransitionWrapper (debugCamera);
			}
			else
			{
				ReParentTransitionWrapper (vrCam);
			}
		}
	}
}

