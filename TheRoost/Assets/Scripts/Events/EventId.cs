namespace Events
{
	public enum EventId
	{
		ApplicationExit,
		NetPlayerDisconnected,
		NetPlayerConnected,
		NetPlayerIdentified,
		EntitySpawned,
		EntityDestroyed,
		EntityHealthUpdate,
		EntityMoved,
		EntityTransformUpdated,
		VRHandCollisionEnter,
		VRHandCollisionExit,
		VRControllerPulse,
		VRControllerTriggerPress,
		VRControllerTriggerRelease,
		DebugCameraControlsActive
	}
}
