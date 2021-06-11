using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenCommonEnergySpawner : MonoBehaviour
{
    [SerializeField] private GameObject commonEnergyPf;
    [SerializeField] private Transform spawnCommonEnergyStartPos;
    [SerializeField] private GameObject commonEnergySpawnerPsObj;
    private Transform bbPos;
    private Sprint sprint;
    private float localStaminaAmount = 0f;
    private bool playerIsInArea = false;

    void Start()
    {
        commonEnergySpawnerPsObj.SetActive(false);

        if(BloquinhoBrancoController.instance != null)
        {
            bbPos = BloquinhoBrancoController.instance.GetComponent<Transform>();
            sprint = BloquinhoBrancoController.instance.GetComponent<Sprint>();
        }
    }
    private void Update()
    {
        //Debug.Log("local: " + localStaminaAmount);
        //Debug.Log("stamina: " + StaminaSystem.instance.GetStaminaAmount());
        
        if(playerIsInArea && !sprint.IsRunning())//&& !sprint.IsRunning()
            SpawnCommonEnergyHandler();

        if (sprint.IsRunning())
        {
            localStaminaAmount -= Time.deltaTime * 4f;

            if (localStaminaAmount < 0f)
                localStaminaAmount = 0f;
        }

        if(localStaminaAmount != StaminaSystem.instance.GetStaminaAmount() && !playerIsInArea)
            localStaminaAmount = StaminaSystem.instance.GetStaminaAmount();

        float playerDistance = Mathf.Abs(PlayerDistance(bbPos));
        float playerDistanceY = Mathf.Abs(PlayerDistanceY(bbPos));

        if (playerDistanceY <= 18f)
        {
            if (playerDistance <= 22f)
                commonEnergySpawnerPsObj.SetActive(true);
            else
                DisableParticles();
        }
        else
        {
            DisableParticles();
        }
    }

    void FixedUpdate()
    {
        if(localStaminaAmount >= StaminaSystem.instance.GetMaxStamina())
            localStaminaAmount = StaminaSystem.instance.GetMaxStamina();

        if(StaminaSystem.instance.GetStaminaAmount() >= StaminaSystem.instance.GetMaxStamina())
            StaminaSystem.instance.SetStaminaAmount(StaminaSystem.instance.GetMaxStamina());
    }

    private void SpawnCommonEnergyHandler()
    {
        if(localStaminaAmount < StaminaSystem.instance.GetMaxStamina())
        {
            localStaminaAmount++;

            float randomSpawnPointX = Random.Range(-0.5f, 0.5f);
            float randomSpawnPointY = Random.Range(-1f, 1.5f);

            Vector3 randomizedSpawnPosition = new Vector3(
                spawnCommonEnergyStartPos.position.x + randomSpawnPointX,
                spawnCommonEnergyStartPos.position.y + randomSpawnPointY,
                spawnCommonEnergyStartPos.position.z);

            GameObject cloneCommomEnergy = Instantiate(commonEnergyPf, randomizedSpawnPosition, transform.rotation);
            cloneCommomEnergy.GetComponent<CommonEnergy>().CanGoToBB = true;

            float randomSpeed = Random.Range(7, 9);
            cloneCommomEnergy.GetComponent<CommonEnergy>().SpeedToGo = randomSpeed;
        }
    }

    private void DisableParticles()
    {
        commonEnergySpawnerPsObj.SetActive(false);

        ParticleSystem ps = commonEnergySpawnerPsObj.GetComponent<ParticleSystem>();

        ps.Stop();
        ps.Clear();

        var main = ps.main;
        main.prewarm = true;

        ps.Play();
    }

    private float PlayerDistance(Transform playerPos)
    {
        return Vector2.Distance(playerPos.position, transform.position);
    }
    private float PlayerDistanceY(Transform playerPos)
    {
        return transform.position.y - playerPos.position.y;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
            playerIsInArea = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
            playerIsInArea = false;
    }
}
