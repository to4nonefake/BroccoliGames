using UnityEngine;

public abstract class CameraInput : MonoBehaviour
{
    public abstract bool DragButtonPressed();
    public abstract Vector3 PointerPosition();
    public abstract float ZoomDelta();
}
