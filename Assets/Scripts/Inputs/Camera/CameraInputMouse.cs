using UnityEngine;

public class CameraInputMouse : CameraInput 
{
    public override bool DragButtonPressed() {
        if (Input.GetMouseButton(0)) {
            return true;
        }

        return false;
    }

    public override Vector3 PointerPosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public override float ZoomDelta() {
        return Input.mouseScrollDelta.y;
    }
}
