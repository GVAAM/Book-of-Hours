using UnityEngine;

public class ListComponents : MonoBehaviour
{
    void Start()
    {
        // Get all components attached to the GameObject
        Component[] components = GetComponents<Component>();

        // Iterate through each component and print its type name
        foreach (Component component in components)
        {
            Debug.Log("Component: " + component.GetType().Name);
        }
    }
}
