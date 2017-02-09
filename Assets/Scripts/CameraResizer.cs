using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    public Bounds targetBounds;
    Resolution res;

    void Start()
    {
        ResizeCamera();
        res = Screen.currentResolution;
    }

    void LateUpdate()
    {
        if (res.width != Screen.currentResolution.width || res.height != Screen.currentResolution.height)
        {
            ResizeCamera();
            res = Screen.currentResolution;
        }
    }

    void ResizeCamera()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = targetBounds.size.x / targetBounds.size.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = targetBounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = targetBounds.size.y / 2 * differenceInSize;
        }

        transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, -15f);
    }

}