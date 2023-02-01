using UnityEngine;

public class SaveGameOnStart : MonoBehaviour
{
    private SaveLoadSystem saveLoadSystem;

    private void Start()
    {
        saveLoadSystem = GameObject.FindGameObjectWithTag("Save System").GetComponent<SaveLoadSystem>();
        saveLoadSystem.LoadPlayerPosition();
        saveLoadSystem.SaveGame();
        saveLoadSystem.SceneObjectsValidator();

        var bg = GameObject.Find("Background");
        int childCounts = bg.transform.childCount;

        for (int i = 0; i < childCounts; i++)
        {
            if (SwitchParametres.BGName != null)
            {
                var bgChild = bg.transform.GetChild(i);

                if (bgChild.name == SwitchParametres.BGName)
                {
                    bgChild.gameObject.SetActive(true);
                    var childRenderer = bgChild.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var child in childRenderer)
                    {
                        child.color = new Color(child.color.r, child.color.g, child.color.b, 1.0f);
                    }
                }
                else
                    bgChild.gameObject.SetActive(false);
            }
        }
    }
}
