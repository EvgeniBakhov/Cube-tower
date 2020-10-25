
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float cubeChangePlaceSpeed = 0.5f;

    public Transform cubeToPlace;
    public GameObject CubeToCreate;
    public GameObject AllCubes;

    private bool gameOver;

    private Rigidbody allCubesRigidBody;
    private CubePos nowCube = new CubePos(0, 1, 0);

    private List<Vector3> allCubesPositions = new List<Vector3> {
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1),
        new Vector3(-1, 0, -1),
        new Vector3(1, 0, 1)
    };

    // Start is called before the first frame update
    void Start()
    {
        allCubesRigidBody = AllCubes.GetComponent<Rigidbody>();
        StartCoroutine(ShowCubePlace());
    }

    // Update is called once per frame
    void Update()
    {
        /* If User presses LEFT MOUSE KEY or touches touchscreen*/
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) {
#if !UNITY_EDITOR
        if(Input.GetTouch(0).phase != TouchPhase.Began)
        return;
#endif
            GameObject newCube = Instantiate(CubeToCreate, cubeToPlace.position, Quaternion.identity) as GameObject;        //create new cube with random coordinates on user's touch or click
            newCube.transform.SetParent(AllCubes.transform);
            nowCube.SetVector(cubeToPlace.position);
            allCubesPositions.Add(nowCube.GetVector());

            allCubesRigidBody.isKinematic = true;
            allCubesRigidBody.isKinematic = false;

            if(!gameOver && (allCubesRigidBody.velocity.magnitude > 0.1f))
            {
                Destroy(cubeToPlace.gameObject);
                gameOver = true;
            }
        }
    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        List<Vector3> spawnedPositions = new List<Vector3>();
        List<Vector3> possiblePositions = FindAllPossiblePositions(new Vector3(nowCube.x, nowCube.y, nowCube.z));

        foreach(Vector3 position in possiblePositions)
        {
            if (IsPositionEmpty(position))
            {
                spawnedPositions.Add(position);
            }
        }
        Vector3 randomPosition;
        do
        {
            randomPosition = spawnedPositions[UnityEngine.Random.Range(0, spawnedPositions.Count)];
        } while (randomPosition.Equals(cubeToPlace.position));

        cubeToPlace.position = randomPosition;
    }

    private bool IsPositionEmpty(Vector3 cubePosition)
    {
        if(cubePosition.y == 0)
        {
            return false;
        }
        foreach(Vector3 lockedPosition in allCubesPositions)
        {
            if (cubePosition.Equals(lockedPosition))
            {
                return false;
            }
        }
        return true;
    }

    private List<Vector3> FindAllPossiblePositions(Vector3 currentPosition)
    {
        List<Vector3> possiblePositions = new List<Vector3>
        {
            new Vector3(currentPosition.x + 1, currentPosition.y, currentPosition.z),
            new Vector3(currentPosition.x - 1, currentPosition.y, currentPosition.z),
            new Vector3(currentPosition.x, currentPosition.y + 1, currentPosition.z),
            new Vector3(currentPosition.x, currentPosition.y - 1, currentPosition.z),
            new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + 1),
            new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - 1)
        };

        return possiblePositions;
    }
}

struct CubePos {
    public int x, y, z;

    public CubePos (int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }

    public void SetVector(Vector3 pos)
    {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}
