using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveLoadSystem : MonoBehaviour
{
    Vector3 playerPosition;

    private void Start()
    {
        if (gameObject.name != "Save point" && gameObject.name != "MainUI" && gameObject.name != "PauseMenu") DontDestroyOnLoad(this);  
    }

    public void SaveGame()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        ProjectData projectData = new ProjectData
        {
            hearts = SwitchParametres.HealthCounter,
            peach = SwitchParametres.PeachCounter,
            canJump = SwitchParametres.CanJump,
            canDoubleJump = SwitchParametres.CanDoubleJump,
            canDash = SwitchParametres.CanDash,
            canAttack = SwitchParametres.CanAttack,
            gameObjectsNames = SwitchParametres.objectsNames,
            playerCoordinates = new List<float> { playerPosition.x, playerPosition.y, playerPosition.z },
            scene = SceneManager.GetActiveScene().name,
            bgname = SwitchParametres.BGName,
            bgTriggersNamesData = SwitchParametres.bgTriggersNames
        };

        File.WriteAllText("savedgame.json", JsonConvert.SerializeObject(projectData));
    }

    public void LoadGame()
    {
        string savedData = File.ReadAllText("savedgame.json");
        ProjectData projectData = JsonConvert.DeserializeObject<ProjectData>(savedData);

        SwitchParametres.HealthCounter = projectData.hearts;
        SwitchParametres.PeachCounter = projectData.peach;
        SwitchParametres.CanJump = projectData.canJump;
        SwitchParametres.CanDoubleJump = projectData.canDoubleJump;
        SwitchParametres.CanDash = projectData.canDash;
        SwitchParametres.CanAttack = projectData.canAttack;
        SwitchParametres.playerPosition = projectData.playerCoordinates;
        
        SwitchParametres.objectsNames = projectData.gameObjectsNames;
        SwitchParametres.SceneName = projectData.scene;
        SwitchParametres.BGName = projectData.bgname;
        SwitchParametres.bgTriggersNames = projectData.bgTriggersNamesData;

        SceneManager.LoadScene(SwitchParametres.SceneName);
    }
    
    public void SavesClearing()
    {
        Debug.Log("saves are cleared");

        SwitchParametres.HealthCounter = 3;
        SwitchParametres.PeachCounter = 0;
        SwitchParametres.CanJump = false;
        SwitchParametres.CanDoubleJump = false;
        SwitchParametres.CanDash = false;
        SwitchParametres.CanAttack = false;
        SwitchParametres.playerPosition = new List<float>();

        SwitchParametres.objectsNames = new List<string>();
        SwitchParametres.SceneName = null;
        SwitchParametres.BGName = null;
        SwitchParametres.bgTriggersNames = new Dictionary<string, bool>();
    }
    

    public void SceneObjectsValidator()
    {
        foreach (string objectName in SwitchParametres.objectsNames)
            Destroy(GameObject.Find(objectName));

        foreach (var triggerName in SwitchParametres.bgTriggersNames)
        {

            foreach (var item in TriggersInit.triggers)
            {
                if (item != null)
                {
                    if (item.name == triggerName.Key)
                    {
                        item.SetActive(triggerName.Value);
                    }
                }
            }
        }
    }

    public void LoadPlayerPosition()
    {
        try
        {
            GameObject.FindGameObjectWithTag("Player").transform.position =
            new Vector3(SwitchParametres.playerPosition[0], SwitchParametres.playerPosition[1], SwitchParametres.playerPosition[2]);
            GameObject.Find("Text score").GetComponent<Text>().text = SwitchParametres.PeachCounter.ToString();
        }
        catch
        {
            Debug.Log("Player coordinates error on load");
        }
    }
}