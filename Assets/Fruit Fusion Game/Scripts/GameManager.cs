using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject FruitCatchPoint;
    public GameObject Cherry,Lemon, StrawBerry, Fig, Kiwi, Mango, Orange, DragonFruit, WaterMelon, Durian;
    public GameObject[] OrderToSort;
    public List<GameObject> StartingFruits;
    public List<(GameObject NewFruit, float FruitWieght)> DropingFruits = new List<(GameObject NewFruit, float FruitWieght)>();
    public int[] SimplePoint = { 0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 66 };
    public int[] BonusPoint = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
    public GameObject ComingFruit;
    public GameObject FruitToDisplayNext;
    public bool CanDrop = true;
    public bool IsLevelFailed = false, IsLevelPaused, IsLevelComplete;
    public float FruitZTransform = -1;
    public bool MurgedFruitKeepMovement = true;
    public float fruitDestroyScaleIncrement = 0.05f;
    [Header("Particles")]
    public float particlesZ = -2;
    public GameObject particlesContainer;
    public ParticleSystem particleFruitCollision;
    [Header("Audio")]
    public float volumeSFX = 1; // Between 0->1
    public GameObject audioContainer;
    public GameObject audioSource;
    public AudioClip SFXSimpletMerge;
    public AudioClip SFXCompleteTime;
    public GameObject PausePanel, FailedPanel, CompletePanel;
    public TextMeshProUGUI ScoreText;
    public int GameBalance;
    private void Awake()
    {
        instance = this;    
    }
    void Start()
    {
        GameBalance = PlayerPrefs.GetInt("Coins");
        OrderToSort = new GameObject[] { Cherry, Lemon, StrawBerry, Fig, Kiwi, Mango, Orange, DragonFruit, WaterMelon, Durian };
        StartingFruits = new List<GameObject>{Cherry, Lemon };
        DropingFruits.Add((Cherry, 0.25f));
        DropingFruits.Add((Lemon, 0.25f));
        DropingFruits.Add((Fig, 0.25f));
        DropingFruits.Add((Kiwi, 0.25f));
        Invoke(nameof(UpdateNextFruitDisplay), 0.15f);
    }
    public void UpdateNextFruitDisplay()
    {
        // Save 2nd fruit in queue in nextFruit var
        ComingFruit = StartingFruits[1];
        for (int i = FruitToDisplayNext.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(FruitToDisplayNext.transform.GetChild(i).gameObject);
        }
        GameObject nextFruitObject = Instantiate(ComingFruit, FruitToDisplayNext.transform.position, Quaternion.identity);
        nextFruitObject.transform.SetParent(FruitToDisplayNext.transform);
        nextFruitObject.GetComponent<Rigidbody2D>().simulated = false; // Turn off simulation on rb2d
    }
    public void AddRandomFruitToQueue()
    {
        // Calculate totalWeight
        float totalWeight = 0f;
        foreach (var fruit in DropingFruits) totalWeight += fruit.FruitWieght;

        // Generate a random number between 0 and totalWeight
        float randomValue = Random.Range(0f, totalWeight);

        // Determine which fruit to select based on the random value
        float cumulativeWeight = 0f;
        for (int i = 0; i < DropingFruits.Count; i++)
        {
            cumulativeWeight += DropingFruits[i].FruitWieght;
            if (randomValue <= cumulativeWeight)
            {
                StartingFruits.Add(DropingFruits[i].NewFruit);
                break;
            }
        }
    }
    public void SameFruitCollided(GameObject selfFruit, GameObject otherFruit, int fruitID)
    {
        // Play animation
        StartCoroutine(SameFruitCollidedAnimation(selfFruit, otherFruit, fruitID));

        // Add points to score
        CalculatePoint(fruitID, true);
    }
    IEnumerator SameFruitCollidedAnimation(GameObject selfFruit, GameObject otherFruit, int fruitID)
    {
        float fruitDestroyScaleIncrement = 0.05f;

        // Change fruit scale
        selfFruit.transform.localScale = selfFruit.transform.localScale + new Vector3(fruitDestroyScaleIncrement, fruitDestroyScaleIncrement, fruitDestroyScaleIncrement);
        otherFruit.transform.localScale = otherFruit.transform.localScale + new Vector3(fruitDestroyScaleIncrement, fruitDestroyScaleIncrement, fruitDestroyScaleIncrement);

        // Spwan particles
        Vector2 midpoint = (selfFruit.transform.position + otherFruit.transform.position) / 2f; // Position between fruits
        StartCoroutine(SpawnParticle(particleFruitCollision, midpoint));

        // Play SFX
        PlaySFX(SFXSimpletMerge, true, 0.8f, 1.2f);

        // Wait for a short duration
        yield return new WaitForSeconds(0.05f);

        // Destroy both fruits
        Destroy(selfFruit);
        Destroy(otherFruit);

        // Merge two fruits
        MergeFruits(selfFruit, otherFruit, fruitID);
    }
    public GameObject[] mergeParticlePrefab;
    public GameObject[] FruitLifeCycleUpdate;
    void MergeFruits(GameObject selfFruit, GameObject otherFruit, int fruitID)
    {
       // Debug.LogError("Current Fruit ID" + fruitID);
        // Get next fruit if fruitID <= 9
        if (fruitID <= 7)
        {
            FruitLifeCycleUpdate[fruitID + 1].SetActive(true);
            // Get next fruit ID
            int nextFruitID = fruitID + 1;

            // Check if fruitID is within valid range
            if (!(nextFruitID >= 0 && nextFruitID < OrderToSort.Length)) return;

            // Get the next fruit prefab from the array
            GameObject nextFruitPrefab = OrderToSort[nextFruitID];

            // Calculate midpoint position between selfFruit and otherFruit
            Vector2 midpoint = (selfFruit.transform.position + otherFruit.transform.position) / 2f;

            // Calculate average velocity
            Rigidbody2D selfRb = selfFruit.GetComponent<Rigidbody2D>();
            Rigidbody2D otherRb = otherFruit.GetComponent<Rigidbody2D>();
            Vector2 averageVelocity = (selfRb.linearVelocity + otherRb.linearVelocity) / 2f;
            float averageAngularVelocity = (selfRb.angularVelocity + otherRb.angularVelocity) / 2f;

            // Spawn the mergedFruit at the midpoint position
            GameObject mergedFruit = Instantiate(nextFruitPrefab, new Vector3(midpoint.x, midpoint.y, FruitZTransform), Quaternion.identity);
            mergedFruit.transform.SetParent(FruitCatchPoint.transform);
            if (mergeParticlePrefab != null)
            {
                GameObject effect = Instantiate(
                    mergeParticlePrefab[Random.Range(0, mergeParticlePrefab.Length)],
                    mergedFruit.transform.position,
                    Quaternion.identity
                );
                Destroy(effect, 3f);
            }
            // Apply the average velocity and angular velocity
            if (MurgedFruitKeepMovement)
            {
                Rigidbody2D mergedRb = mergedFruit.GetComponent<Rigidbody2D>();
                if (mergedRb != null)
                {
                    mergedRb.linearVelocity = averageVelocity;
                    mergedRb.angularVelocity = averageAngularVelocity;
                }
            }
        }
        // Clear board if fruitID = 10 (water melon)
        else if (fruitID == 8)
        {
            FruitLifeCycleUpdate[fruitID + 1].SetActive(true);
            // Play fruit10 merge sound
            PlaySFX(SFXCompleteTime);
            Invoke(nameof(GameCompleted), 3f);
            // Clear board
            StartCoroutine(ClearBoard());
        }
    }
   
    IEnumerator ClearBoard(float destroyDelay = 0.05f)
    {
        Transform containerTransform = FruitCatchPoint.transform;

        // Iterate through each child object backwards
        while (containerTransform.childCount > 0)
        {
            // Get the child object at the last index
            GameObject fruitObject = containerTransform.GetChild(containerTransform.childCount - 1).gameObject;

            // Get the fruit ID from the FruitScript attached to the fruitObject
            FruitManager fruitScript = fruitObject.GetComponent<FruitManager>();
            int fruitID = fruitScript.FruitIndex;

            // Animation
            fruitObject.transform.localScale = fruitObject.transform.localScale + new Vector3(fruitDestroyScaleIncrement, fruitDestroyScaleIncrement, fruitDestroyScaleIncrement);
           StartCoroutine(SpawnParticle(particleFruitCollision, fruitObject.transform.position));

            // Play SFX
            PlaySFX(SFXSimpletMerge, true, 0.8f, 1.2f);

            // Wait for a little before going to next fruit and destroying
            yield return new WaitForSeconds(destroyDelay);

            // Calculate point from fruit and destroy it
            CalculatePoint(fruitID);
            Destroy(fruitObject);
        }
    }
    public IEnumerator SpawnParticle(ParticleSystem particleSystem, Vector2 position)
    {
        Vector3 spawnPosition = new Vector3(position.x, position.y, particlesZ);
        ParticleSystem particleInstance = Instantiate(particleSystem, spawnPosition, Quaternion.identity);
        particleInstance.transform.SetParent(particlesContainer.transform); // Set particlesContainer as parent
        yield return new WaitForSeconds(particleInstance.main.duration); // Wait for the duration of the particle system        
        Destroy(particleInstance.gameObject); // Destroy the particle system
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void PlaySFX(AudioClip audioClip, bool pitchRange = false, float pitch1 = 0f, float pitch2 = 0f)
    {
        // Create audio source object as child of audioContainer
        GameObject localAudioSource = Instantiate(audioSource);
        localAudioSource.transform.SetParent(audioContainer.transform); // Set audioContainer as parent
        AudioSource localAudioSourceComponent = localAudioSource.GetComponent<AudioSource>(); // Get component

        // Change pitch if pitchRange true
        if (pitchRange) localAudioSourceComponent.pitch = Random.Range(pitch1, pitch2);

        // Play sound
        localAudioSourceComponent.volume = volumeSFX;
        localAudioSourceComponent.clip = audioClip;
        localAudioSourceComponent.Play();

        // Destroy object
        Destroy(localAudioSource, localAudioSourceComponent.clip.length);
    }
    void CalculatePoint(int fruitID, bool merge = false)
    {
        // Validate fruitID
        if (fruitID < 0 || fruitID >= SimplePoint.Length)
        {
            Debug.LogError($"Invalid fruitID: {fruitID}. It should be within the range 0 to {SimplePoint.Length - 1}.");
            return;
        }

        // Add points 
        if (merge && fruitID < BonusPoint.Length)
        {
            GameBalance = PlayerPrefs.GetInt("Coins");
            GameBalance += BonusPoint[fruitID];
        }
        GameBalance += SimplePoint[fruitID];
        ScoreText.text ="Score = " + GameBalance.ToString();
        PlayerPrefs.SetInt("Coins", GameBalance);
    }
    void GameCompleted()
    {
        Time.timeScale = 0f;
        CompletePanel.SetActive(true);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
    public void GamePause()
    {
        Time.timeScale = 0.01f;
        PausePanel.SetActive(true);
        IsLevelPaused = true;
    }
    public void GameResume()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
        IsLevelPaused =false;
    }
}
