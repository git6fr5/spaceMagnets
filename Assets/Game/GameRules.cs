/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the parameters for the game.
/// </summary>
public class GameRules : MonoBehaviour {

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
    public static int PixelsPerUnit = 8;
    public static int PixelsVertical = 128;
    public static int PixelsHorizontal = 256;

    /* --- Colors --- */
    public static Color Red = Color.red;
    public static Color Blue = Color.blue;
    public static Color White = Color.white;


    // Collect objects in game
    // Set their outline colors
    // Set their layers

    public static bool IsEditing;
    // public bool isEditing;
    public BackgroundUI background;

    void Update() {

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
}
