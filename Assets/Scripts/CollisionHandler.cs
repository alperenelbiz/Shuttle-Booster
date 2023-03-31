using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip successAudio;
    [SerializeField] AudioClip failureAudio;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem failureParticle;

    AudioSource audiosource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;

            case "Finish":
                StartSuccesSequence();
                break;

            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccesSequence()
    {
        isTransitioning = true;
        audiosource.Stop();
        GetComponent<PlayerMovement>().enabled = false;
        audiosource.PlayOneShot(successAudio);
        successParticle.Play();

        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audiosource.Stop();
        GetComponent<PlayerMovement>().enabled = false;
        audiosource.PlayOneShot(failureAudio);
        failureParticle.Play();

        Invoke("RelaodLevel", levelLoadDelay);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    void RelaodLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }
}
