/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[ExecuteInEditMode]
public class Revenue : MonoBehaviour {

    public struct Settings {

        public float mass;
        public float horizon;
        public float radius;
        public int value;

        public Settings(int mass, int horizon, int radius, int value) {
            this.mass = ((float)mass) / 16f;
            this.horizon = ((float)horizon) / 16f;
            this.radius = ((float)radius) / 16f;
            this.value = value;
        }

    };

    public enum Type {

        Moon,

        Dwarf,
        Terrestial,
        GasGiant,

        WhiteDwarf,
        RedGiant,

        Binary, // Hmmm?
        Pulsar, // Maybe this can be a pulsating push thing?
        Supernova // Maybe this can be a constant push thing?

    };

    public Dictionary<Type, Settings> defaultSettingsDict = new Dictionary<Type, Settings>() {

        { Type.Moon, new Settings(1, 2, 16, 2) },
        { Type.Dwarf, new Settings(2, 4, 24, 3) },
        { Type.Terrestial, new Settings(4, 8, 32, 4) },
        { Type.GasGiant, new Settings(8, 12, 40, 5) },

        { Type.WhiteDwarf, new Settings(16, 14, 48, 7) },
        { Type.RedGiant, new Settings(16, 16, 48, 10) },

    };

    /* --- Properties --- */
    public string locationName = "Unnamed";
    public Type type;
    public int value = 0;
    public bool defaultSettings = false;

    public RevenueUI uiComponent; 

    /* --- Unity --- */
    private void Start() {

        GameObject uiObject = Instantiate(uiComponent.gameObject, transform.position, Quaternion.identity, transform);
        uiObject.SetActive(true);
        uiObject.transform.position += new Vector3(0, -1.15f, 0f);

    }

    private void Update() {
        if (defaultSettings) {
            value = defaultSettingsDict[type].value;
            Force force = GetComponent<Force>();
            if (force != null) {
                force.mass = defaultSettingsDict[type].mass;
                force.horizon = defaultSettingsDict[type].horizon;
                force.radius = defaultSettingsDict[type].radius;
            }
            defaultSettings = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.color *= 0.75f;
        Gizmos.DrawWireSphere(transform.position, ShuttlePath.MaxDistance);
        Gizmos.DrawWireSphere(transform.position, ShuttlePath.MinDistance);
    }


}
