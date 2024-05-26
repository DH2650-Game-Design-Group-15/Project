using System;
using System.Collections.Generic;
using UnityEngine;

public class Observe : MonoBehaviour
{
    [SerializeField] private float updateTime = 0.01f;
    private float timeSinceUpdate = 0;
    private ObjectDetection objectDetection;
    private Fractions fractions;

    void Start(){
        objectDetection = GetComponent<ObjectDetection>();
        fractions = Parent.FindParent(gameObject, typeof(Fractions), 10)?.GetComponent<Fractions>();
        if (fractions == null){
            fractions = gameObject.AddComponent<Fractions>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceUpdate += Time.deltaTime;
        if (updateTime < timeSinceUpdate){
            timeSinceUpdate = 0;
            if (fractions.OwnFraction != Fraction.None){
                GameObject[] objects = FindObjects();
                FindThief(objects);
            }
        }
    }

    private GameObject[] FindObjects(){
        GameObject[] objects = objectDetection.DetectObjects();
        objects = ObjectDetection.ObjectsWithComponent(objects, typeof(Owner));
        List<GameObject> myObjects = new();
        foreach (GameObject obj in objects)
        {
            if (obj.GetComponent<Owner>().Fraction == fractions.OwnFraction){
                myObjects.Add(obj);
            }
        }
        objects = myObjects.ToArray();
        return objects;
    }

    private void FindThief(GameObject[] objects){
        Fraction myFraction = fractions.OwnFraction;
        foreach (GameObject obj in objects)
        {
            List<GameObject> interactions = obj.GetComponent<Owner>().GetInteractions();
            if (interactions.Count > 0){
                for (int i = interactions.Count - 1; i >= 0; i--)
                {
                    GameObject interaction = interactions[i];
                    Fraction otherFraction = interaction.GetComponent<Fractions>().OwnFraction;
                    if (myFraction != otherFraction){
                        Tuple<Fraction, Fraction> frac = new(myFraction, otherFraction);
                        float oldReputation = Fractions.GetReputation(frac);
                        Fractions.SetReputation(frac, -15);
                        Debug.Log(transform.parent.name + " detected illegal access at " + obj.name + "\nThe reputation changed from " + oldReputation + " to " + Fractions.GetReputation(frac));
                        obj.GetComponent<Owner>().RemoveInteraction(interaction);
                    }   
                }
            }
        }
    }
}
