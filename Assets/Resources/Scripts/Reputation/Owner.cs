using System.Collections.Generic;
using UnityEngine;

public class Owner : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private Fraction fraction;
    public List<GameObject> interact = new();

    public GameObject Character { get => character; set => character = value; }
    public Fraction Fraction { get => fraction; set => fraction = value; }
    public List<GameObject> GetInteractions(){
        return interact;
    }

    public void AddInteraction(GameObject character){
        interact.Add(character);
    }

    public void RemoveInteraction(GameObject character){
        interact.Remove(character);
    }
}
