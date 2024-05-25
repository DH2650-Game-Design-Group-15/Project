using System.Collections.Generic;
using UnityEngine;

public class Owner : MonoBehaviour
{
    [SerializeField] private Fraction fraction;
    public List<GameObject> interact = new();

    public Fraction Fraction { get => fraction; set => fraction = value; }
    public List<GameObject> GetInteractions(){
        return interact;
    }

    public void AddInteraction(GameObject character){
        if (fraction != Fraction.None){
            interact.Add(character);
        }
    }

    public void RemoveInteraction(GameObject character){
        interact.Remove(character);
    }
}
