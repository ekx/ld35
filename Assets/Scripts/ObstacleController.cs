using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ObstacleController : Singleton<ObstacleController>
{
    public GameObject[] Obstacles;
    public AudioReverbFilter ReverbFilter;
    public Light PointLight;

    public void ResetDifficulty()
    {
        difficultyTimeOffset = Time.time;
        lastSpawn = Time.time;
    }

    public void DestroyObstacles()
    {
        obstacles.ForEach(o => o.RemoveObstacle());
        obstacles.Clear();
    }

    public void FixedUpdate()
    {
        if (GameController.Instance.TimeScale <= 0f)
        {
            return;
        }

        ReverbFilter.roomHF = RoomHF;
        ReverbFilter.decayTime = DecayTime;
        ReverbFilter.decayHFRatio = DecayHF;

        PointLight.intensity = LightIntensity;

        // Spawn
        if (Time.time - lastSpawn > SpawnRate)
        {
            SpawnObstacle();
        }

        // Move
        obstacles.ForEach((o) => 
        {
            o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y, o.transform.position.z - (Speed * GameController.Instance.TimeScale));
        });

        // Remove
        obstacles.ForEach((o) =>
        {
            if (o.transform.position.z < -20 )
            {
                o.RemoveObstacle();
                obstacles.Remove(o);
            }
        });
    }

    private void SpawnObstacle()
    {
        var index = Random.Range(0, ObstacleIndex);
        var prefab = Obstacles[index];

        var instance = Instantiate(prefab);
        var obstacle = instance.GetComponentInChildren<Obstacle>();

        instance.transform.SetParent(transform, true);      
        instance.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 180f));

        obstacles.Add(obstacle);
        lastSpawn = Time.time;
    }

    #region DifficultySettings

    private Dictionary<float, int> obstacleIndexMap = new Dictionary<float, int>()
    {
        { 20f, 1 },
        { 50f, 2 },
        { float.PositiveInfinity, 3 }
    };
    public int ObstacleIndex { get { return obstacleIndexMap.First(i => i.Key > DifficultyTime).Value; } }

    private float spawnRateMin = 5f;
    private float spawnRateMax = 1f;
    private float spawnRateVariance = 0.2f;
    public float SpawnRate { get { return Mathf.Lerp(spawnRateMin, spawnRateMax, DifficultyTime / DifficultyCapTime) + Random.Range(-spawnRateVariance, spawnRateVariance); } }

    private float speedMin = 0.4f;
    private float speedMax = 1f;
    public float Speed { get { return Mathf.Lerp(speedMin, speedMax, DifficultyTime / DifficultyCapTime); } }

    private float RoomHFMin = -4000f;
    private float RoomHFMax = -150f;
    public float RoomHF { get { return Mathf.Lerp(RoomHFMin, RoomHFMax, DifficultyTime / DifficultyCapTime); } }

    private float DecayTimeMin = 1.5f;
    private float DecayTimeMax = 7.5f;
    public float DecayTime { get { return Mathf.Lerp(DecayTimeMin, DecayTimeMax, DifficultyTime / DifficultyCapTime); } }

    private float DecayHFMin = 0.1f;
    private float DecayHFMax = 0.9f;
    public float DecayHF { get { return Mathf.Lerp(DecayHFMin, DecayHFMax, DifficultyTime / DifficultyCapTime); } }

    private float LightIntensityMin = 1.5f;
    private float LightIntensityMax = 0.1f;
    public float LightIntensity { get { return Mathf.Lerp(LightIntensityMin, LightIntensityMax, DifficultyTime / DifficultyCapTime); } }

    private const float DifficultyCapTime = 100f;
    public float DifficultyTime { get { return Time.time - difficultyTimeOffset; } }

    #endregion

    private float lastSpawn = 0f;
    private float difficultyTimeOffset = 0f;
    private List<Obstacle> obstacles = new List<Obstacle>();
}