using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Fraction {
    Player,
    Friends,
    Neutral,
    Enemy,
    WorstEnemy
}

public class Fractions : MonoBehaviour {
    private static Hashtable reputation;
    private static object reputationLock = new object();
    public Fraction ownFraction;

    private void Start() {
        if (ownFraction == null) {
            ownFraction = Fraction.Neutral;
        }
        if (reputation == null){
            InitReputation();
        }
    }

    public Fraction getFraction() {
        return ownFraction;
    }

    public int getReputation(Fraction otherFraction) {
        return getReputation(otherFraction, ownFraction);
    }

    public int getReputation(Fraction otherFraction, Fraction fraction){
        lock(reputationLock) {
            Hashtable reputationTable = (Hashtable) reputation[fraction];
            return (int) reputationTable[otherFraction];
        }
    }

    private void setReputation(Fraction otherFraction, int value) {
        setReputation(otherFraction, ownFraction, value);
    }

    private void setReputation(Fraction otherFraction, Fraction fraction, int value) {
        ((Hashtable)reputation[fraction])[otherFraction] = value;
    }
    
    private void changeReputation(Fraction otherFraction, int value) {
        changeReputation(otherFraction, ownFraction, value);
    }

    private void changeReputation(Fraction otherFraction, Fraction fraction, int value) {
        int oldValue = (int)((Hashtable) reputation[fraction])[otherFraction];
        setReputation(otherFraction, fraction, value + oldValue);
    }

    private void setReputationForBoth(Fraction otherFraction, int value) {
        setReputation(otherFraction, ownFraction, value);
        setReputation(ownFraction, otherFraction, value);
    }

    private void setReputationForBoth(Fraction otherFraction, Fraction fraction, int value) {
        setReputation(otherFraction, fraction, value);
        setReputation(fraction, otherFraction, value);
    }
    
    private void changeReputationForBoth(Fraction otherFraction, int value) {
        changeReputation(otherFraction, ownFraction, value);
        changeReputation(ownFraction, otherFraction, value);
    }

    private void changeReputationForBoth(Fraction otherFraction, Fraction fraction, int value) {
        changeReputation(otherFraction, fraction, value);
        changeReputation(fraction, otherFraction, value);
    }

    /// <summary>
    /// initialises the Hashtable reputation, if it isn't already initialised, it initialises it for each fraction to each other
    /// </summary>
    /// <returns></returns>
    private static void InitReputation(){
        lock(reputationLock){
            if (reputation == null) {
                reputation = new Hashtable();
                InitReputationForFraction(new int[]{ 100, 50, 0, -50, -100}, Fraction.Player);
                InitReputationForFraction(new int[]{ 50, 100, 0, -50, -100}, Fraction.Friends);
                InitReputationForFraction(new int[]{ 0, 0, 100, 0, -100}, Fraction.Neutral);
                InitReputationForFraction(new int[]{ -50, -50, 0, 100, -100}, Fraction.Enemy);
                InitReputationForFraction(new int[]{ -100, -100, -100, -100, 100}, Fraction.WorstEnemy);
            }
        }
    }

    
    /// <summary>
    /// Creates a Hashtable with reputations to all fractions, for the given fraction.
    /// The table is always from perspective of this fraction to all other fractions.
    /// </summary>
    /// <param name="reputation">Values, that are mapped to all fractions</param>
    /// <param name="fraction">Fraction, that gets this Table</param>
    /// <returns></returns>
    private static void InitReputationForFraction(int[] reputations, Fraction fraction){
        Hashtable mapping = new Hashtable();
        Fraction[] fractions = (Fraction[])Enum.GetValues(typeof(Fraction));

        if (fractions.Length != reputations.Length)
        {
            throw new ArgumentException("Array has a wrong length.");
        }

        for (int i = 0; i < fractions.Length; i++)
        {
            mapping.Add(fractions[i], reputations[i]);
        }

        reputation.Add(fraction, mapping);
    }
    
}