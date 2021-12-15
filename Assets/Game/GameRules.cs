/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the parameters for the game.
/// </summary>
public class GameRules : MonoBehaviour {

    /* --- Physics --- */
    public static float MovementPrecision = 0.05f;

    /* --- Path --- */
    public static string Path = "Assets/Resources/";

    /* --- Game Tags --- */
    public static string PlayerTag = "Player";
    public static string SpawnerTag = "Spawner";
    public static string StationTag = "Station";

    /* --- Sorting Layers --- */
    public static string Background = "Background";
    public static string Midground = "Midground";
    public static string Foreground = "Foreground";

    /* --- Screen Dimensions --- */
    public static int PixelsPerUnit = 16;
    public static int PixelsVertical = 144;
    public static int PixelsHorizontal = 256;

    /* --- Colors --- */
    public static Color Red = Color.red;
    public static Color Blue = Color.blue;
    public static Color White = Color.white;
    public static Color Yellow = Color.yellow;

    // Collect objects in game
    // Set their outline colors
    // Set their layers

    public static bool IsEditing;
    // public bool isEditing;
    public Background background;

    void Update() {

        if (background == null) {
            return;
        }

        // IsEditing = isEditing;
        if (IsEditing) {
            background.spriteRenderer.color = Color.green;
            Time.timeScale = 0f;
        }
        else {
            background.spriteRenderer.color = Color.white;
            Time.timeScale = 1f;
        }

        //if (Input.GetKeyDown(KeyCode.E)) {
        //    isEditing = !isEditing;
        //    Reset();
        //}

        //ForceUI[] forceUIs = (ForceUI[])GameObject.FindObjectsOfType(typeof(ForceUI));
        //for (int i = 0; i < forceUIs.Length; i++) {
        //    forceUIs[i].enabled = isEditing;
        //}

        //Spawner spawner = (Spawner)GameObject.FindObjectOfType<Spawner>();
        //spawner.enabled = !isEditing;


    }

    public static void Reset() {
        ClearShuttles();
    }

    private static void ClearShuttles() {
        Shuttle[] shuttles = (Shuttle[])GameObject.FindObjectsOfType(typeof(Shuttle));
        for (int i = 0; i < shuttles.Length; i++) {
            Destroy(shuttles[i].gameObject);
        }

    }

    /* --- Editor --- */
    private void OnDrawGizmos() {
        Gizmos.color = Yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(PixelsHorizontal / PixelsPerUnit, PixelsVertical / PixelsPerUnit, 1f));
    }

}
