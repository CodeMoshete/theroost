namespace Events
{
	public enum EventId
	{
		ApplicationExit,
		NetPlayerDisconnected,
		NetPlayerConnected,
		NetPlayerIdentified,
		PlayerAimed,
		EntitySpawned,
		EntityDestroyed,
		EntityHealthUpdate,
		EntityMoved,
		EntityFired,
		EntityFiredLocal,
		EntityTransformUpdated,
		VRHandCollisionEnter,
		VRHandCollisionExit,
		VRControllerPulse,
		VRControllerTriggerPress,
		VRControllerTriggerRelease,
		VRControllerTouchpadPress,
		VRControllerTouchpadDrag,
		VRControllerTouchpadRelease,
		DebugCameraControlsActive
	}
}
