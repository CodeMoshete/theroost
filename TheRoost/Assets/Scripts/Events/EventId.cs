namespace Events
{
	public enum EventId
	{
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
