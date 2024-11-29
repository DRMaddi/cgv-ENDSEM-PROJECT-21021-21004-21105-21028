using UnityEngine;

public class checkpointsystem : MonoBehaviour
{
    public static checkpointsystem Instance; // Singleton instance
    public Transform[] checkpoints; // Array of checkpoints
    private Vector2 lastCheckpointPosition; // Stores last checkpoint position
    public Transform player; // Reference to player

    public int restart_var = 0; 

    void Awake()
    {
        // Singleton pattern to ensure single instance
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        restart_var = PlayerPrefs.GetInt("restart_var", +restart_var);
        if (restart_var == 1)
        {
             ResetCheckpoints();
            restart_var = 0;
            PlayerPrefs.SetInt("restart_var", restart_var);
        }
        // ResetCheckpoints();
        if (PlayerPrefs.HasKey("LastCheckpointX") && PlayerPrefs.HasKey("LastCheckpointY"))
        {
            float x = PlayerPrefs.GetFloat("LastCheckpointX");
            float y = PlayerPrefs.GetFloat("LastCheckpointY");
            lastCheckpointPosition = new Vector2(x, y);
            player.position = lastCheckpointPosition;
        }
        else if (checkpoints.Length > 0)
        {
            lastCheckpointPosition = checkpoints[0].position;
            player.position = lastCheckpointPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (other.transform == checkpoints[i])
            {
                lastCheckpointPosition = checkpoints[i].position;
                PlayerPrefs.SetFloat("LastCheckpointX", lastCheckpointPosition.x);
                PlayerPrefs.SetFloat("LastCheckpointY", lastCheckpointPosition.y);
                PlayerPrefs.Save();
                print("hit_checkpoint");
                break;
            }
        }
    }

    public void RespawnPlayerAtCheckpoint()
    {
        player.position = lastCheckpointPosition;
    }

    public void ResetCheckpoints()
    {
        PlayerPrefs.DeleteKey("LastCheckpointX");
        PlayerPrefs.DeleteKey("LastCheckpointY");

        if (checkpoints.Length > 0)
        {
            lastCheckpointPosition = checkpoints[0].position;
            player.position = lastCheckpointPosition;
        }
    }

    // New button function to reset player to the first checkpoint
    public void SetPositionToFirstCheckpoint()
    {
        if (checkpoints.Length > 0)
        {
            lastCheckpointPosition = checkpoints[0].position;
            player.position = lastCheckpointPosition;

            // Update PlayerPrefs as well
            PlayerPrefs.SetFloat("LastCheckpointX", lastCheckpointPosition.x);
            PlayerPrefs.SetFloat("LastCheckpointY", lastCheckpointPosition.y);
            PlayerPrefs.Save();
        }
    }
}