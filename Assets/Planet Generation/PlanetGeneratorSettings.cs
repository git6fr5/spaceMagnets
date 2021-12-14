using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlanetGeneratorSettings {

    public static string path = "Planets/";
    public static string filetype = ".planet";

    public string planetName; 
    public int textureType;
    public int refreshType;
    public float radius;
    public float seaLevel;
    public float heightRange;
    public int subdivisions;
    // public Gradient waterGradient;
    // public Gradient landGradient;
    public float[] vectorSeed;
    public bool autoRebuild;
    public int animationFrameRate;
    public float incrementPerFrame;
    public float implosionSpeed;
    public float explosionSpeed;
    public float rotationSpeed;
    public float rotationIncrements;
    public float[] rotationAxis;

    public PlanetGeneratorSettings(PlanetGenerator gen) {

        this.planetName = gen.planetName;
        this.textureType = (int)gen.textureType;
        this.refreshType = (int)gen.refreshType;
        this.radius = gen.radius;
        this.seaLevel = gen.seaLevel;
        this.heightRange = gen.heightRange;
        this.subdivisions = gen.subdivisions;
        // this.waterGradient = gen.waterGradient;
        // this.landGradient = gen.landGradient;
        this.vectorSeed = new float[2] { gen.vectorSeed.x, gen.vectorSeed.y };
        this.autoRebuild = gen.autoRebuild;
        this.animationFrameRate = gen.animationFrameRate;
        this.incrementPerFrame = gen.incrementPerFrame;
        this.implosionSpeed = gen.implosionSpeed;
        this.explosionSpeed = gen.explosionSpeed;
        this.rotationSpeed = gen.rotationSpeed;
        this.rotationIncrements = gen.rotationIncrements;
        this.rotationAxis = new float[3] { gen.rotationAxis.x, gen.rotationAxis.y, gen.rotationAxis.z };

    }


    public void Save() {

        // Concatenate the path.
        string fullPath = GameRules.Path + path + planetName + filetype;

        // Format the data.
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(fullPath, FileMode.Create);
        formatter.Serialize(fileStream, this);

        // Close the file.
        fileStream.Close();

    }

    public static void Load(PlanetGenerator gen) {

        // Concatenate the path.
        string fullPath = GameRules.Path + path + gen.planetName + filetype;

        if (File.Exists(fullPath)) {

            // Read the data.
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(fullPath, FileMode.Open);
            PlanetGeneratorSettings settings = (PlanetGeneratorSettings)formatter.Deserialize(fileStream);

            gen.planetName = settings.planetName;
            gen.textureType = (PlanetGenerator.TextureType)settings.textureType;
            gen.refreshType = (PlanetGenerator.RefreshType)settings.refreshType;
            gen.radius = settings.radius;
            gen.seaLevel = settings.seaLevel;
            gen.heightRange = settings.heightRange;
            gen.subdivisions = settings.subdivisions;
            // gen.waterGradient = settings.waterGradient;
            // gen.landGradient = settings.landGradient;
            gen.vectorSeed = new Vector2( settings.vectorSeed[0], settings.vectorSeed[1] );
            gen.autoRebuild = settings.autoRebuild;
            gen.animationFrameRate = settings.animationFrameRate;
            gen.incrementPerFrame = settings.incrementPerFrame;
            gen.implosionSpeed = settings.implosionSpeed;
            gen.explosionSpeed = settings.explosionSpeed;
            gen.rotationSpeed = settings.rotationSpeed;
            gen.rotationIncrements = settings.rotationIncrements;
            gen.rotationAxis = new Vector3( settings.rotationAxis[0], settings.rotationAxis[1], settings.rotationAxis[2] );


            // Close the file.
            fileStream.Close();

        }

        return;

    }

}
