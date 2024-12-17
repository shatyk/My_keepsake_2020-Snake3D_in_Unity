using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGap
{
    public int Index { get; private set; }
    public Vector3 Coords { get; private set; }

    public SnakeGap(Vector3 _coords)
    {
        Index = 1;
        Coords = _coords;
    }

    public void IncIndex()
    {
        Index++;
    }
}

public class SnakeController : MonoBehaviour
{
    [SerializeField] private ManagerGame gameManager;
    [SerializeField] private GameObject partSnakePrefab;
    private PartSnakeLogic headSnake;
    private PartSnakeLogic[] initPartsSnake;
    private CameraRotateContr cameraRotateContr;

    [SerializeField] private List<GameObject> partSnakePrefabList;
    [SerializeField] private List<GameObject> headSnakePrefabList;

    [SerializeField] private int skinId = 1;

    private float timeCounterMove;
    [SerializeField] private float delayBetwTransform = 0.3f;
    private GameObject headSnakeGameObj;
    private bool snakeEatFood = false;

    private DirectionsEnum headDir;
    private DirectionsEnum oldHeadDir;

    public List<PartSnakeLogic> partsSnakeList { get; private set; }
    private Queue<SnakeGap> snakeGaps = new Queue<SnakeGap>();

    private int rotateCameraOffsetDir = 0;

    private void Awake()
    {
        partsSnakeList = new List<PartSnakeLogic>();
    }
    // Start is called before the first frame update
    void Start()
    {
        cameraRotateContr = gameManager.GetCurrentCamera().GetComponent<CameraRotateContr>();
        cameraRotateContr.ChangeCameraDirection += OnChangedCameraDirection;

        initPartsSnake = new PartSnakeLogic[3];

        headSnakeGameObj = Instantiate(headSnakePrefabList[skinId]);
        headSnakeGameObj.transform.parent = this.transform;
        headSnakeGameObj.transform.localPosition = Vector3.zero;
        headSnakeGameObj.transform.localRotation = Quaternion.identity;

        headSnake = headSnakeGameObj.GetComponent<PartSnakeLogic>();

        for (int i = 0; i < 3; i++)
        {
            var snakePartObj = Instantiate(partSnakePrefabList[skinId]);
            initPartsSnake[i] = snakePartObj.GetComponent<PartSnakeLogic>();
            snakePartObj.transform.parent = this.transform;
            snakePartObj.transform.localPosition = new Vector3(-1 - i, 0, 0);
            snakePartObj.transform.localRotation = Quaternion.identity;
        }

        partsSnakeList.Add(headSnake);
        foreach (PartSnakeLogic part in initPartsSnake)
        {
            partsSnakeList.Add(part);
        }
        headDir = DirectionsEnum.Forward;

        gameManager.GenerateNewFood(partsSnakeList);
    }

    // Update is called once per frame
    void Update()
    {
        timeCounterMove -= Time.deltaTime;

        headDir = ReadInput();

        if (timeCounterMove <= 0)
        {
            headSnake.SetMoveDirection(headDir);
            headSnake.TransformPart();

            int countSnakeParts = partsSnakeList.Count;

            for (int i = 1; i < countSnakeParts; i++)
            {
                if (snakeEatFood && i == (countSnakeParts - 1))
                {
                    AddSnakePart(partsSnakeList[i].transform.position);
                } 
                partsSnakeList[i].TransformPart();

            }
            bool isDequeue = false;
            foreach(var gap in snakeGaps)
            {
                partsSnakeList[gap.Index].transform.position = gap.Coords;
                gap.IncIndex();
                if (gap.Index == partsSnakeList.Count) isDequeue = true;
            }
            if (isDequeue) snakeGaps.Dequeue();

            for (int i = partsSnakeList.Count - 1; i > 0; i--)
            {
                partsSnakeList[i].SetMoveDirection(partsSnakeList[i - 1].MoveDirection);
            }

            timeCounterMove += delayBetwTransform;
            oldHeadDir = headDir;

            //gameManager.GetAStarRunner().MyStart();
        }
    }

    private DirectionsEnum ReadInput()
    {
        DirectionsEnum dir = headDir;
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            if (dir != (DirectionsEnum)((2 + rotateCameraOffsetDir) % 4))
                dir = (DirectionsEnum) ((0 + rotateCameraOffsetDir) % 4);
        }
        else
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (dir != (DirectionsEnum)((0 + rotateCameraOffsetDir) % 4))
                dir = (DirectionsEnum)((2 + rotateCameraOffsetDir) % 4);
        }
        else
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (dir != (DirectionsEnum)((1 + rotateCameraOffsetDir) % 4))
                dir = (DirectionsEnum)((3 + rotateCameraOffsetDir) % 4);
        }
        else
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (dir != (DirectionsEnum)((3 + rotateCameraOffsetDir) % 4))
                dir = (DirectionsEnum)((1 + rotateCameraOffsetDir) % 4); 
        }
        else
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dir != DirectionsEnum.Down)
                dir = DirectionsEnum.Top;
        }
        else
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (dir != DirectionsEnum.Top)
                dir = DirectionsEnum.Down;
        }
        return dir;
    }

    private void AddSnakePart(Vector3 pos)
    {
        snakeEatFood = false;
        GameObject newPart = Instantiate(partSnakePrefabList[skinId], transform, false);

        newPart.transform.position = pos;
        partsSnakeList.Add(newPart.GetComponent<PartSnakeLogic>());
    }

    private void CreateGap()
    {
        Vector3 newCoords = headSnake.transform.position;
        switch (headSnake.MoveDirection)
        {
            case DirectionsEnum.Top:
                newCoords.y -= gameManager.sizeMap.Y;
                break;
            case DirectionsEnum.Down:
                newCoords.y += gameManager.sizeMap.Y;
                break;
            case DirectionsEnum.Left:
                newCoords.z -= gameManager.sizeMap.Z;
                break;
            case DirectionsEnum.Right:
                newCoords.z += gameManager.sizeMap.Z;
                break;
            case DirectionsEnum.Forward:
                newCoords.x -= gameManager.sizeMap.X;
                break;
            case DirectionsEnum.Backward:
                newCoords.x += gameManager.sizeMap.X;
                break;
        }
        headSnake.transform.position = newCoords;
        snakeGaps.Enqueue(new SnakeGap(newCoords));
    }

    public void TriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<Food>() != null)
        {
            gameManager.IncScore();
            snakeEatFood = true;
            Destroy(coll.gameObject);
            gameManager.GenerateNewFood(partsSnakeList);
        } else 
        if (coll.gameObject.GetComponent<Wall>() != null)
        {
            CreateGap();
        } else
        if (coll.gameObject.GetComponent<PartSnakeLogic>() != null)
        {
            gameManager.GameOver();
        }
    }

    private void OnChangedCameraDirection(DirectionsEnum newDir)
    {
        rotateCameraOffsetDir = (int)newDir;
    }

    private void SetDefaultProp()
    {
        
    }
}
