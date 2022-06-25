using System.Collections;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera;

    public enum RotateTo { Back, Top};
    public RotateTo rotateTo;

    private float rotateSpeed = 20.0f;
    private bool didRotate = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!didRotate && rotateTo == RotateTo.Back)
        {
            StartCoroutine(MoveOverSeconds(player, Vector3.up, 2.25f, rotateSpeed*2f,RotateTo.Back));
            Move.viewSide = Move.ViewSide.fromSide;
            CameraMove.isFacingFromBack = true;
            CameraMove.isFacingFromTop = false;
        }
        if (!didRotate && rotateTo == RotateTo.Top)
        {
            StartCoroutine(MoveOverSeconds(player, Vector3.back, 1f, rotateSpeed*2.125f,RotateTo.Top));
            Move.viewSide = Move.ViewSide.fromSide;
            CameraMove.isFacingFromBack = false;
            CameraMove.isFacingFromTop = true;
        }
        didRotate = true;
    }


    public IEnumerator MoveOverSeconds(GameObject target, Vector3 axis, float seconds, float rotateSpeed, RotateTo rotateTo)
    {
        CameraMove.isRotating = true;
        float elapsedTime = 0;

        while (elapsedTime < seconds && (target.transform.rotation.y < 90 || target.transform.rotation.x < 42.889f))
        {
            //Rotate camera
            //Rotate player
            if (rotateTo == RotateTo.Back)
                target.transform.Rotate(Vector3.Slerp(Vector3.zero,Vector3.up,.8f));
            if (rotateTo == RotateTo.Top)
                target.transform.Rotate(Vector3.Slerp(Vector3.zero, Vector3.right, .8f));

            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (rotateTo == RotateTo.Back)
            target.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        if (rotateTo == RotateTo.Top)
            target.transform.rotation = Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(42.889f, Vector3.right);
        CameraMove.isRotating = false;
    }
}
