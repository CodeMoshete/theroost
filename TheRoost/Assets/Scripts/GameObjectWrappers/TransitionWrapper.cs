//TransitionWrapper.cs
//Wrapper class for the transition screen.

using System;
using UnityEngine;
using Delegates;
using MonoBehaviors.Interfaces;
using MonoBehaviors;

namespace GameObjectWrappers
{
	public class TransitionWrapper : IAnimationEventObserver
	{
		private const string TRANSITION_OVER_EVENT = "transitionOver";
		private const string TRANSITION_IN_ID = "TransitionIn";
		private const string TRANSITION_OUT_ID = "TransitionOut";

		private GameObject m_wrappedObject;
		private DefaultDelegate m_onAnimationOver;
		private Animator m_animator;

		public TransitionWrapper (GameObject wrappedObject)
		{
			m_wrappedObject = wrappedObject;
			m_wrappedObject.GetComponent<AnimationEventHandler>().RegisterForAnimationEvents(this);
			m_animator = m_wrappedObject.GetComponent<Animator>();
		}

		public void PlayTransitionIn(DefaultDelegate onAnimationOver = null)
		{
			m_onAnimationOver = onAnimationOver;
			m_animator.Play(TRANSITION_IN_ID);
		}

		public void PlayTransitionOut(DefaultDelegate onAnimationOver = null)
		{
			m_onAnimationOver = onAnimationOver;
			m_animator.Play(TRANSITION_OUT_ID);
		}

		public void SetActive(bool active)
		{
			m_wrappedObject.SetActive(active);
		}

		public void OnAnimationEvent(string eventType)
		{
			if(eventType == TRANSITION_OVER_EVENT && m_onAnimationOver != null)
			{
				m_onAnimationOver();
				m_onAnimationOver = null;
			}
		}
	}
}

