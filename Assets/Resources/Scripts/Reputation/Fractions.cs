using System;
using System.Collections.Generic;
using UnityEngine;

public class Fractions : MonoBehaviour {
    [SerializeField] private Fraction ownFraction;

    private static readonly Dictionary<Tuple<Fraction, Fraction>, float> reputation = new();
    
    private static Tuple<Fraction, Fraction> SortFraction(Tuple<Fraction, Fraction> fractions){
        if ((int) fractions.Item1 < (int) fractions.Item2){
            fractions = new (fractions.Item2, fractions.Item1);
        }
        return fractions;
    }

    public static void SetRepuation(Tuple<Fraction, Fraction> fractions, float value){
        SetRepuation(fractions, value, false);
    }

    public static void SetRepuation(Tuple<Fraction, Fraction> fractions, float value, bool absolut){
        fractions = SortFraction(fractions);
        if (!absolut){
            value += GetReputation(fractions);
        }
        if (reputation.ContainsKey(fractions)){
            reputation[fractions] = value;
        } else {
            reputation.Add(fractions, value);
        }
    }

    public static float GetReputation(Tuple<Fraction, Fraction> fractions){
        fractions = SortFraction(fractions);
        bool found = reputation.TryGetValue(fractions, out float rep);
        if (found){
            return rep;
        } else {
            return 0;
        }
    }

    public Fraction OwnFraction { get => ownFraction; set => ownFraction = value; }
    
}