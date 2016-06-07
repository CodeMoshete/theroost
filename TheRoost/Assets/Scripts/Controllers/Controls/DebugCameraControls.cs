using UnityEngine;
using Services;
using Events;

namespace HammerEditor.Main.Cameras.ControlSystems
{
    public class DebugCameraControls
    {
        private readonly Vector3 CAM_START_POS = new Vector3(15f, 11f, 12f);
        private readonly Vector3 CAM_START_ROT = new Vector3(35f, 225f, 0f);
        private const string ORBIT_CONTAINER_NAME = "OrbitContainer";
        private const float MOVE_SPEED = 0.1f;
        private const float MOVE_SPEED_MULT = 3f;
        private const float CAM_LOOK_MULT = 0.25f;

        private bool isDragging = false;
        private bool isDraggingOrbit = false;
        private bool isOrbiting = false; 
        private bool canScroll = true;
        private float speedMult = 1f;
        private Vector3 oldMousePos;
        private Transform transform;
        private Transform orbitContainer;

        public void Initialize(Camera cam)
        {
            // TODO: We need the following core utility methods created before we can use Midcore UnityCamera:
            // - Transform.Translate(Vector3)
            // - Transform.eulerAngles
            // - Quaternion.Euler(Vector3)
			transform = cam.transform;
            transform.position = CAM_START_POS;
            transform.eulerAngles = CAM_START_ROT;
        }

        public void Update(float dt)
        {
            // TODO: Consider using InputManager from Midcore Library.
            if (Input.GetKey(KeyCode.W) && isDragging || isDraggingOrbit)
            {
                Vector3 fwdSpeed = new Vector3(0f, 0f, MOVE_SPEED * speedMult);
                transform.Translate(fwdSpeed);
            }
            
            if (Input.GetKey(KeyCode.A) && isDragging)
            {
                Vector3 fwdSpeed = new Vector3(-MOVE_SPEED * speedMult, 0f, 0f);
                transform.Translate(fwdSpeed);
                ClearCamTarget();
            }
            
            if (Input.GetKey(KeyCode.S) && isDragging || isDraggingOrbit)
            {
                Vector3 fwdSpeed = new Vector3(0f, 0f, -MOVE_SPEED * speedMult);
                transform.Translate(fwdSpeed);
            }
            
            if (Input.GetKey(KeyCode.D) && isDragging)
            {
                Vector3 fwdSpeed = new Vector3(MOVE_SPEED * speedMult, 0f, 0f);
                transform.Translate(fwdSpeed);
                ClearCamTarget();
            }
            
            if (Input.GetKey(KeyCode.Q) && isDragging)
            {
                Vector3 fwdSpeed = new Vector3(0f, -MOVE_SPEED * speedMult, 0f);
                transform.Translate(fwdSpeed);
                ClearCamTarget();
            }
            
            if (Input.GetKey(KeyCode.E) && isDragging)
            {
                Vector3 fwdSpeed = new Vector3(0f, MOVE_SPEED * speedMult, 0f);
                transform.Translate(fwdSpeed);
                ClearCamTarget();
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speedMult = MOVE_SPEED_MULT;
            }
            
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speedMult = 1f;
            }
            
            if (isDragging && !isOrbiting)
            {
                ClearCamTarget();
                
                Vector3 diff = oldMousePos - Input.mousePosition;
                diff *= CAM_LOOK_MULT;
                oldMousePos = Input.mousePosition;
                Vector3 euler = transform.eulerAngles;
                if (Mathf.RoundToInt(euler.z) != 180)
                {
                    euler.y -= diff.x;
                }
                else
                {
                    euler.y += diff.x;
                }
                
                transform.eulerAngles = euler;
                transform.rotation *= Quaternion.Euler(new Vector3(diff.y, 0f, 0f));
            }
            
            if (isDraggingOrbit && isOrbiting)
            {
                Vector3 diff = oldMousePos - Input.mousePosition;
                diff *= CAM_LOOK_MULT;
                oldMousePos = Input.mousePosition;
                Vector3 euler = orbitContainer.transform.eulerAngles;
                if (Mathf.RoundToInt(euler.z) != 180)
                {
                    euler.y -= diff.x;
                }
                else
                {
                    euler.y += diff.x;
                }
                
                orbitContainer.transform.eulerAngles = euler;
                orbitContainer.transform.rotation *= Quaternion.Euler(new Vector3(-diff.y, 0f, 0f));
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                isDraggingOrbit = false;
                isDragging = true;
                oldMousePos = Input.mousePosition;
            }
            
            if (Input.GetMouseButtonUp(1))
            {
                isDragging = false;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                isDraggingOrbit = true;
                oldMousePos = Input.mousePosition;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingOrbit = false;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftAlt) && orbitContainer != null)
            {
                isOrbiting = true;
            }
            
            if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                isOrbiting = false;
            }
            
			float scrollAmt = Input.GetAxis("Mouse ScrollWheel");
            if (scrollAmt != 0f && canScroll)
            {
				VRTouchpadInteraction fakePress = 
					new VRTouchpadInteraction(transform.gameObject, Vector2.zero);
				Service.Events.SendEvent (EventId.VRControllerTouchpadPress, fakePress);

				VRTouchpadInteraction fakeDrag = 
					new VRTouchpadInteraction(transform.gameObject, new Vector2(0f, scrollAmt));
				Service.Events.SendEvent (EventId.VRControllerTouchpadDrag, fakeDrag);
            }
        }
        
        private void ClearCamTarget()
        {
            if (orbitContainer != null)
            {
                transform.SetParent(null);
                GameObject.Destroy(orbitContainer.gameObject);
                orbitContainer = null;
            }
        }
        
        public void SetCamTarget(Transform target)
        {
            GameObject tmpGo = new GameObject();
            tmpGo.name = ORBIT_CONTAINER_NAME;
            orbitContainer = tmpGo.transform;
            orbitContainer.position = target.position;
            orbitContainer.LookAt(transform.position);
            transform.LookAt(target.position);
            transform.SetParent(orbitContainer);
        }

        public void Unload()
        {
            ClearCamTarget();
            transform = null;
        }
    }
}
