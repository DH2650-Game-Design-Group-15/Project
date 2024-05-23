using System;
using System.Collections.Generic;
using UnityEngine;

public class Fractions : MonoBehaviour {
    [SerializeField] private Fraction ownFraction;

    public static readonly float minEnemyReputation = -30;
    public static readonly float minFriendlyReputation = 30;
    public static readonly float maxReputation = 100;
    private static readonly Dictionary<Tuple<Fraction, Fraction>, float> reputation = new();
    
    private static Tuple<Fraction, Fraction> SortFraction(Tuple<Fraction, Fraction> fractions){
        if ((int) fractions.Item1 < (int) fractions.Item2){
            fractions = new (fractions.Item2, fractions.Item1);
        }
        return fractions;
    }

    public static void SetReputation(Tuple<Fraction, Fraction> fractions, float value){
        SetReputation(fractions, value, false);
    }

    public static void SetReputation(Tuple<Fraction, Fraction> fractions, float value, bool absolut){
        fractions = SortFraction(fractions);
        if (!absolut){
            value += GetReputation(fractions);
        }
        if (value > maxReputation){
            value = maxReputation;
        } else if (value < -maxReputation){
            value = -maxReputation;
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
    public void SetReputation(Fraction fraction, float value){
        SetReputation(new Tuple<Fraction, Fraction>(OwnFraction, fraction), value);
    }

    public void SetReputationToPlayer(float value){
        SetReputation(Fraction.Player, value);
    }

    public void SetReputation(Fraction fraction, float value, bool absolut){
        SetReputation(new Tuple<Fraction, Fraction>(OwnFraction, fraction), value, absolut);
    }

    public void SetReputationToPlayer(float value, bool absolut){
        SetReputation(Fraction.Player, value, absolut);
    }

    public float GetReputation(Fraction fraction){
        return GetReputation(new Tuple<Fraction, Fraction>(OwnFraction, fraction));
    }

    public float GetReputationToPlayer(){
        return GetReputation(Fraction.Player);
    }

    public bool IsFriendly(Fraction fraction){
        return GetReputation(fraction) > minFriendlyReputation;
    }

    public bool IsNeutral(Fraction fraction){
        return !IsFriendly(fraction) && !IsEnemy(fraction);
    }

    public bool IsEnemy(Fraction fraction){
        return GetReputation(fraction) < minEnemyReputation;
    }

    public bool IsFriendly(){
        return IsFriendly(Fraction.Player);
    }

    public bool IsEnemy(){
        return IsEnemy(Fraction.Player);
    }

    public bool IsNeutral(){
        return !IsFriendly() && !IsEnemy();
    }

    public Fraction OwnFraction { get => ownFraction; set => ownFraction = value; }



    // Only for testing

    public bool increase;
    public bool decrease;
    public bool printReputations;
    public Fraction toFraction;
    void Update(){
        if (increase){
            SetReputation(toFraction, 10);
            increase = false;
        } else if (decrease){
            SetReputation(toFraction, -10);
            decrease = false;
        } else if (printReputations){
            string msg = "";
            foreach (KeyValuePair<Tuple<Fraction, Fraction>, float> pair in reputation)
            {
                Fraction f1 = pair.Key.Item1;
                Fraction f2 = pair.Key.Item2;
                float rep = pair.Value;
                msg = msg + f1.ToString() + " and " + f2.ToString() + " have reputation: " + rep + "\n";
            }
            Debug.Log(msg);
            printReputations = false;
        }
    }
    
}