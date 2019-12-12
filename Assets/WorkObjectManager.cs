using UnityEngine;

public class WorkObjectManager : MonoBehaviour
{
    public WorkObjectWithoutUpdate[] workObjects;

    public void Update()
    {
        for (int i = 0; i < workObjects.Length; i++)
        {
            workObjects[i].ManualUpdate();
        }
    }
}