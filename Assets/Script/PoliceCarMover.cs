using UnityEngine;


public class PoliceCarMover : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float stopDistance = 0.1f;
    public Transform[] wheels;
    public float wheelRotationSpeed = 360f;
    public GameObject policePrefab;           // Polis karakter prefabı
    public Transform policeSpawnPoint;        // Polis nereden spawn olacak
    public Transform policeWalkTarget;        // Polis nereye yürüsün

    private bool isMoving = false;
    private bool hasStarted = false;
    private bool hasSpawnedPolice = false;

    void Start()
    {
        StartMoving();
    }

    void Update()
    {
        if (isMoving && Vector3.Distance(transform.position, target.position) > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.LookAt(target);

            foreach (Transform wheel in wheels)
            {
                wheel.Rotate(Vector3.right * wheelRotationSpeed * Time.deltaTime);
            }
        }
        else if (isMoving && !hasSpawnedPolice)
        {
            isMoving = false;
            SpawnPolice();
        }
    }

    public void StartMoving()
    {
        if (hasStarted) return;

        hasStarted = true;
        isMoving = true;

        var audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
        {
            audio.Play();
        }
    }

    void SpawnPolice()
    {
        hasSpawnedPolice = true;

        GameObject police = Instantiate(policePrefab, policeSpawnPoint.position, policeSpawnPoint.rotation);

        // PoliceWalker scriptini çalıştır
        PoliceWalker walker = police.GetComponent<PoliceWalker>();
        if (walker != null)
        { 
            walker.SetTarget(policeWalkTarget);
        }
    }

}
