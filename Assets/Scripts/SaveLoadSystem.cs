using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveLoadSystem : MonoBehaviour
{
    Vector3 player_position;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SaveGame()
    {
        player_position = GameObject.FindGameObjectWithTag("Player").transform.position;

        ProjectData projectData = new ProjectData
        {
            hearts = SwitchParametres.HealthCounter,
            peach = SwitchParametres.PeachCounter,
            canJump = SwitchParametres.CanJump,
            canDoubleJump = SwitchParametres.CanDoubleJump,
            canDash = SwitchParametres.CanDash,
            canAttack = SwitchParametres.CanAttack,
            gameObjectsNames = SwitchParametres.objectsNames,
            player_coordinates = new List<float> { player_position.x, player_position.y, player_position.z }
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
        GameObject.FindGameObjectWithTag("Player").transform.position = 
            new Vector3(projectData.player_coordinates[0], projectData.player_coordinates[1], projectData.player_coordinates[2]);
        SwitchParametres.objectsNames = projectData.gameObjectsNames;

        GameObject.Find("Text score").GetComponent<Text>().text = SwitchParametres.PeachCounter.ToString();
        SceneObjectsValidator();
    }

    public void SceneObjectsValidator()
    {
        foreach (string objectName in SwitchParametres.objectsNames)
        {
            Destroy(GameObject.Find(objectName));
        }
    }
}