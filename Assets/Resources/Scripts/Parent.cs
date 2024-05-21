using System;
using UnityEngine;

public static class Parent{
    /// <summary> Returns the first parent with this component. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next 5 parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="type"> The Type of the Component </param>
    public static Transform FindParent(GameObject child, Type type){
        return FindParent(child.transform, type, 5);
    }

    /// <summary> Returns the first parent with this component. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next 5 parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="type"> The Type of the Component </param>
    public static Transform FindParent(Component child, Type type){
        return FindParent(child, type, 5);
    }

    /// <summary> Returns the first parent with this component. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="type"> The Type of the Component </param>
    /// <param name="maxDepth"> How many generations it should look for the parent </param>
    public static Transform FindParent(GameObject child, Type type, int maxDepth){
        return FindParent(child.transform, type, maxDepth);
    }

    /// <summary> Returns the first parent with this component. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="type"> The Type of the Component </param>
    /// <param name="maxDepth"> How many generations it should look for the parent </param>
    public static Transform FindParent(Component child, Type type, int maxDepth){
        while(child.GetComponent(type) == null){
            child = child.transform.parent;
            maxDepth--;
            if (maxDepth == 0 || child == null){
                return null;
            }
        }
        return child.transform;
    }

    /// <summary> Returns the first found parent GameObject with this name. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next 5 parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="parentName"> The name of the GameObject </param>
    public static Transform FindParent(GameObject child, string parentName){
        return FindParent(child.transform, parentName, 5);
    }

    /// <summary> Returns the first found parent GameObject with this name. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next 5 parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="parentName"> The name of the GameObject </param>
    public static Transform FindParent(Component child, string parentName){
        return FindParent(child, parentName, 5);
    }

    /// <summary> Returns the first found parent GameObject with this name. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="parentName"> The name of the GameObject </param>
    /// <param name="maxDepth"> How many generations it should look for the parent </param>
    public static Transform FindParent(GameObject child, string parentName, int maxDepth){
        return FindParent(child.transform, parentName, maxDepth);
    }

    /// <summary> Returns the first found parent GameObject with this name. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="parentName"> The name of the GameObject </param>
    /// <param name="maxDepth"> How many generations it should look for the parent </param>
    public static Transform FindParent(Component child, string parentName, int maxDepth){
        while(child.name != parentName){
            child = child.transform.parent;
            maxDepth--;
            if (maxDepth == 0 || child == null){
                return null;
            }
        }
        return child.transform;
    }


    /// <summary> Returns the first found parent GameObject or one of the siblings with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next 5 parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    public static Transform FindParentSibling(GameObject child, string objectName){
        return FindParentSibling(child.transform, objectName, 5);
    }

    /// <summary> Returns the first found parent GameObject or one of the siblings with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next 5 parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    public static Transform FindParentSibling(Component child, string objectName){
        return FindParentSibling(child, objectName, 5);
    }

    /// <summary> Returns the first found parent GameObject or one of the siblings with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    /// <param name="maxDepth"> How many generations it should look for the parent </param>
    public static Transform FindParentSibling(GameObject child, string objectName, int maxDepth){
        return FindParentSibling(child.transform, objectName, maxDepth);
    }

    /// <summary> Returns the first found parent GameObject or one of the siblings with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this parent. Returns null, if no parent is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    /// <param name="maxDepth"> How many generations it should look for the parent </param>
    public static Transform FindParentSibling(Component child, string objectName, int maxDepth){
        while(child.transform.Find(objectName) == null){
            child = child.transform.parent;
            maxDepth--;
            if (maxDepth == 0 || child == null){
                return null;
            }
        }
        return child.transform.Find(objectName);
    }


    /// <summary> Returns the first found child GameObject with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this child. Returns null, if no child is found within the next 5 parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    public static Transform FindChild(GameObject gameObject, string childName){
        return FindChild(gameObject.transform, childName, 5);
    }

    /// <summary> Returns the first found child GameObject with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this child. Returns null, if no child is found within the next 5 parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    public static Transform FindChild(Component component, string childName){
        return FindChild(component, childName, 5);
    }

    /// <summary> Returns the first found child GameObject with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this child. Returns null, if no child is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    /// <param name="maxDepth"> How many generations it should look for the child </param>
    public static Transform FindChild(GameObject gameObject, string childName, int maxDepth){
        return FindChild(gameObject.transform, childName, maxDepth);
    }

    /// <summary> Returns the first found child GameObject with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this child. Returns null, if no child is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    /// <param name="maxDepth"> How many generations it should look for the child </param>
    public static Transform FindChild(Component component, string childName, int maxDepth){        
        Transform originalTransform = component.transform;
        Transform closestChild = null;
        int closestDistance = int.MaxValue;
        FindChild(originalTransform, childName, maxDepth, 0, ref closestChild, ref closestDistance);
        return closestChild;
    }

    private static void FindChild(Transform currentTransform, string childName, int maxDepth, int currentDepth, ref Transform closestChild, ref int closestDistance) {
        if (currentTransform.childCount == 0 || currentDepth == maxDepth)
            return;

        for (int i = 0; i < currentTransform.childCount; i++)
        {
            Transform child = currentTransform.GetChild(i);
            if (child.name == childName)
            {
                int distance = Mathf.Abs(currentDepth - maxDepth);
                if (distance < closestDistance)
                {
                    closestChild = child;
                    closestDistance = distance;
                }
            }

            FindChild(child, childName, maxDepth, currentDepth + 1, ref closestChild, ref closestDistance);
        }
    }


    /// <summary> Returns the first found child GameObject with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this child. Returns null, if no child is found within the next 5 parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    public static Transform FindChild(Component component, Type childComponent){
        return FindChild(component, childComponent, 5);
    }

    /// <summary> Returns the first found child GameObject with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this child. Returns null, if no child is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    /// <param name="maxDepth"> How many generations it should look for the child </param>
    public static Transform FindChild(GameObject gameObject, Type childComponent, int maxDepth){
        return FindChild(gameObject.transform, childComponent, maxDepth);
    }

    /// <summary> Returns the first found child GameObject with this name. If there're multiple GameObjects it takes the first hit in this generation. </summary>
    /// <returns> The Transform of this child. Returns null, if no child is found within the next maxDepth parents. </returns>
    /// <param name="child"> The GameObject where the search begins </param>
    /// <param name="objectName"> The name of the GameObject </param>
    /// <param name="maxDepth"> How many generations it should look for the child </param>
    public static Transform FindChild(Component component, Type childComponent, int maxDepth){        
        Transform originalTransform = component.transform;
        Transform closestChild = null;
        int closestDistance = int.MaxValue;
        FindChild(originalTransform, childComponent, maxDepth, 0, ref closestChild, ref closestDistance);
        return closestChild;
    }

    private static void FindChild(Transform currentTransform, Type childComponent, int maxDepth, int currentDepth, ref Transform closestChild, ref int closestDistance) {
        if (currentTransform.childCount == 0 || currentDepth == maxDepth)
            return;

        for (int i = 0; i < currentTransform.childCount; i++)
        {
            Transform child = currentTransform.GetChild(i);
            if (child.GetComponent(childComponent) != null)
            {
                int distance = Mathf.Abs(currentDepth - maxDepth);
                if (distance < closestDistance)
                {
                    closestChild = child;
                    closestDistance = distance;
                }
            }

            FindChild(child, childComponent, maxDepth, currentDepth + 1, ref closestChild, ref closestDistance);
        }
    }

}