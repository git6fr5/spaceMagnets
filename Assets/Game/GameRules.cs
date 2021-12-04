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

    /* --- Modes --- */
    public static bool IsEditing;

    /* --- Static Methods --- */
    public static void Reset() {
        ClearShuttles();
    }

    private static void ClearShuttles() {
        Shuttle[] shuttles = (Shuttle[])GameObject.FindObjectsOfType(typeof(Shuttle));
        for (int i = 0; i < shuttles.Length; i++) {
            shuttles[i].Explode();
        }

    }

    public static bool IsWithinBounds(Transform transform) {

        if (transform.position.x > PixelsHorizontal / (2f * PixelsPerUnit)) {
            return false;
        }
        else if (transform.position.x < -PixelsHorizontal / (2f * PixelsPerUnit)) {
            return false;
        }

        if (transform.position.y > PixelsVertical / (2f * PixelsPerUnit)) {
            return false;
        }
        else if (transform.position.y < -PixelsVertical / (2f * PixelsPerUnit)) {
            return false;
        }

        return true;
    }

    public static void SnapWithinBounds(Transform transform) {

        float x = transform.position.x;
        float y = transform.position.y;

        if (transform.position.x > PixelsHorizontal / (2f * PixelsPerUnit)) {
            x = PixelsHorizontal / (2f * PixelsPerUnit);
        }
        else if (transform.position.x < -PixelsHorizontal / (2f * PixelsPerUnit)) {
            x = -PixelsHorizontal / (2f * PixelsPerUnit);
        }

        if (transform.position.y > PixelsVertical / (2f * PixelsPerUnit)) {
            y = PixelsVertical / (2f * PixelsPerUnit);
        }
        else if (transform.position.y < -PixelsVertical / (2f * PixelsPerUnit)) {
            y = -PixelsVertical / (2f * PixelsPerUnit);
        }

        transform.position = new Vector2(x, y);

    }

}
