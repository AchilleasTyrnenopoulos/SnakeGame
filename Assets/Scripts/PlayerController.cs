using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection direction;

    [HideInInspector]
    public float stepLength;
    //[HideInInspector]
    public float movementFreq = 0.1f;
    private float counter;
    private bool move;

    [SerializeField]
    private GameObject tailPrefab;

    private List<Vector3> deltaPos;
    private List<Rigidbody> nodes;
    private Rigidbody mainBody;
    private Rigidbody headBody;
    private Transform _transform;

    private bool createNodeAtTail;

    private void Awake()
    {
        _transform = transform;
        mainBody = GetComponent<Rigidbody>();

        stepLength = Metrics.node;

        //initialize snake nodes and snake game object with the right direction
        InitSnakeNodes();
        InitPlayer();

        deltaPos = new List<Vector3>()
        {
            new Vector3(-stepLength, 0f, 0f), //LEFT
            new Vector3(0f, 0f, stepLength), //UP
            new Vector3(stepLength, 0f, 0f), //RIGHT
            new Vector3(0f, 0f, -stepLength) //DOWN
        };
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementFreq();
    }

    private void FixedUpdate()
    {
        if(move)
        {
            move = false;
            Move();
        }
    }

    private void SetDirectionRandom()
    {
        int dirRnd = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRnd;
    }

    private void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        nodes.Add(_transform.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(_transform.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(_transform.GetChild(2).GetComponent<Rigidbody>());

        headBody = nodes[0];
    }

    private void InitPlayer()
    {
        SetDirectionRandom();

        switch (direction)
        {
            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.node, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.node * 2f, 0f, 0f);
                break;
            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f, 0f, Metrics.node);
                nodes[2].position = nodes[0].position - new Vector3(0f, 0f, Metrics.node * 2f);
                break;
            case PlayerDirection.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(Metrics.node, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.node * 2f, 0f, 0f);
                break;
            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, 0f, Metrics.node);
                nodes[2].position = nodes[0].position + new Vector3(0f, 0f, Metrics.node * 2f);
                break;
            default:
                Debug.LogError("something unexpected went wrong");
                break;
        }


    }

    private void Move()
    {
        Vector3 dPos = deltaPos[(int)direction];

        Vector3 parentPos = headBody.position;
        Vector3 previousPos;

        mainBody.position += dPos;
        headBody.position += dPos;

        for (int i = 1; i < nodes.Count; i++)
        {
            previousPos = nodes[i].position;

            nodes[i].position = parentPos;
            parentPos = previousPos;
        }

        //check if we need to create a new node
        if(createNodeAtTail)
        {
            createNodeAtTail = false;

            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);
            newNode.transform.SetParent(_transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    private void CheckMovementFreq()
    {
        counter += Time.deltaTime;

        if(counter >= movementFreq)
        {
            counter = 0f;
            move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        if (dir == PlayerDirection.UP && direction == PlayerDirection.DOWN
         || dir == PlayerDirection.DOWN && direction == PlayerDirection.UP
         || dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT
         || dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT)
            return;

        direction = dir;
        ForceMove();
    }

    private void ForceMove()
    {
        counter = 0f;
        move = false;
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.Fruit)
        {
            Destroy(other.gameObject);
            createNodeAtTail = true;

            //update score
            GameManager.instance.OnChangeScore(other.GetComponent<Fruit>().value);
        }

        if (other.tag == Tags.Wall || other.tag == Tags.Bomb || other.tag == Tags.Tail)
        {
            print("Lost");
            PlayerDeath();
        }       
    }

    private void PlayerDeath()
    {
        //pause
        Time.timeScale = 0f;

        //show death screen, play sound etc.
    }
    
}
