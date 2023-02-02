using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandling : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        // todo add SFX upon success
        audioSource.PlayOneShot(success);
        // todo add particle effect upon crash
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        // todo add SFX upon crash
        audioSource.PlayOneShot(crash);
        // todo add particle effect upon crash
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
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

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }


}
