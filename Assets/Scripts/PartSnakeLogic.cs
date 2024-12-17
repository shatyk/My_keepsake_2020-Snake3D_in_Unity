using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSnakeLogic : MonoBehaviour
{
    [SerializeField] private bool isHead = false;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Transform modelAngle;
    public DirectionsEnum MoveDirection { get; private set; }

    public void SetMoveDirection(DirectionsEnum newDir)
    {
        RotatePart(newDir);
        MoveDirection = newDir;
    }

    private void Awake()
    {
        MoveDirection = DirectionsEnum.Forward;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TransformPart()
    {
        Vector3 moveTransformDir = Vector3.zero;
        switch (MoveDirection)
        {
            case DirectionsEnum.Forward:
                moveTransformDir.x = 1; break;
            case DirectionsEnum.Backward:
                moveTransformDir.x = -1; break;
            case DirectionsEnum.Right:
                moveTransformDir.z = -1; break;
            case DirectionsEnum.Left:
                moveTransformDir.z = 1; break;
            case DirectionsEnum.Top:
                moveTransformDir.y = 1; break;
            case DirectionsEnum.Down:
                moveTransformDir.y = -1; break;
        }
        transform.Translate(moveTransformDir);
    }

    private void RotatePart(DirectionsEnum newDir)
    {
        if (isHead || MoveDirection == newDir)
        {
            if (!isHead)
            {
                modelAngle.gameObject.SetActive(false);
                modelTransform.gameObject.SetActive(true);
            }
            if (newDir == DirectionsEnum.Forward)
            {
                modelTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            if (newDir == DirectionsEnum.Backward)
            {
                modelTransform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            if (newDir == DirectionsEnum.Top)
            {
                modelTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
            if (newDir == DirectionsEnum.Right)
            {
                modelTransform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            if (newDir == DirectionsEnum.Down)
            {
                modelTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
            }
            if (newDir == DirectionsEnum.Left)
            {
                modelTransform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
            }
        }
        else
        {
            modelAngle.gameObject.SetActive(true);
            modelTransform.gameObject.SetActive(false);

            var newRotation = new Vector3(0,0,0); 

            if (MoveDirection == DirectionsEnum.Forward)
            {
                if (newDir == DirectionsEnum.Top)
                {
                    newRotation = new Vector3(0, 0, 0);
                }
                if (newDir == DirectionsEnum.Down)
                {
                    newRotation = new Vector3(180, 0, 0);
                }
                if (newDir == DirectionsEnum.Right)
                {
                    newRotation = new Vector3(270, 0, 0);
                }
                if (newDir == DirectionsEnum.Left)
                {
                    newRotation = new Vector3(90, 0, 0);
                }
            }
            if (MoveDirection == DirectionsEnum.Backward)
            {
                if (newDir == DirectionsEnum.Top)
                {
                    newRotation = new Vector3(0, 180, 0);
                }
                if (newDir == DirectionsEnum.Down)
                {
                    newRotation = new Vector3(180, 180, 0);
                }
                if (newDir == DirectionsEnum.Right)
                {
                    newRotation = new Vector3(90, 180, 0);
                }
                if (newDir == DirectionsEnum.Left)
                {
                    newRotation = new Vector3(270, 180, 0);
                }
            }
            if (MoveDirection == DirectionsEnum.Left)
            {
                if (newDir == DirectionsEnum.Top)
                {
                    newRotation = new Vector3(0, 270, 0);
                }
                if (newDir == DirectionsEnum.Down)
                {
                    newRotation = new Vector3(180, 270, 0);
                }
                if (newDir == DirectionsEnum.Forward)
                {
                    newRotation = new Vector3(270, 270, 0);
                }
                if (newDir == DirectionsEnum.Backward)
                {
                    newRotation = new Vector3(90, 270, 0);
                }
            }
            if (MoveDirection == DirectionsEnum.Right)
            {
                if (newDir == DirectionsEnum.Top)
                {
                    newRotation = new Vector3(0, 90, 0);
                }
                if (newDir == DirectionsEnum.Down)
                {
                    newRotation = new Vector3(180, 90, 0);
                }
                if (newDir == DirectionsEnum.Forward)
                {
                    newRotation = new Vector3(90, 90, 0);
                }
                if (newDir == DirectionsEnum.Backward)
                {
                    newRotation = new Vector3(270, 90, 0);
                }
            }
            if (MoveDirection == DirectionsEnum.Top)
            {
                if (newDir == DirectionsEnum.Forward)
                {
                    newRotation = new Vector3(0, 180, 90);
                }
                if (newDir == DirectionsEnum.Backward)
                {
                    newRotation = new Vector3(0, 0, 90);
                }
                if (newDir == DirectionsEnum.Right)
                {
                    newRotation = new Vector3(0, 270, 90);
                }
                if (newDir == DirectionsEnum.Left)
                {
                    newRotation = new Vector3(0, 90, 90);
                }
            }
            if (MoveDirection == DirectionsEnum.Down)
            {
                if (newDir == DirectionsEnum.Forward)
                {
                    newRotation = new Vector3(0, 0, 270);
                }
                if (newDir == DirectionsEnum.Backward)
                {
                    newRotation = new Vector3(0, 180, 270);
                }
                if (newDir == DirectionsEnum.Right)
                {
                    newRotation = new Vector3(0, 90, 270);
                }
                if (newDir == DirectionsEnum.Left)
                {
                    newRotation = new Vector3(0, 270, 270);
                }
            }

            modelAngle.rotation = Quaternion.Euler(newRotation);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (isHead)
        {
            var sc = GetComponentInParent<SnakeController>();
            if (sc!=null)
                sc.TriggerEnter(coll);

            var scai = GetComponentInParent<SnakeController_AI>();
            if (scai != null)
                scai.TriggerEnter(coll);

            //GetComponentInParent<SnakeController_AI>().TriggerEnter(coll);
        }
    }
}
