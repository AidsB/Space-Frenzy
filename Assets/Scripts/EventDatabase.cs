using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventDatabase {
    static System.Type[] eventTypes;
    static Dictionary<System.Type, int> probabilities;
    static int probabilitySum;

    static EventDatabase() {
        eventTypes = (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                     from assemblyType in domainAssembly.GetTypes()
                     where typeof(Event).IsAssignableFrom(assemblyType)
                     select assemblyType).ToArray();
        probabilities = new Dictionary<System.Type, int>();
        probabilitySum = 0;
        foreach (System.Type t in eventTypes) {
            probabilities.Add(t, ((Event)System.Activator.CreateInstance(t)).probability);
            probabilitySum += probabilities[t];
        }
    }
    
    public static Event GetEvent() {
        while (true) {
            int p = Random.Range(0, probabilitySum);

            int index = -1;
            int total = 0;
            for (int i = 0; i < eventTypes.Length; i++) {
                if (p < total + probabilities[eventTypes[i]]) {
                    index = i;
                    break;
                }
                total += probabilities[eventTypes[i]];
            }

            if (index != -1) {
                Event e = (Event)System.Activator.CreateInstance(eventTypes[index]);
                if (!e.Generate()) continue;
                return e;
            }
        }
    }
}
