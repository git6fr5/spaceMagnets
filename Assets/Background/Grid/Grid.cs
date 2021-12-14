using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

    // Components
    public Spring[] springs;
    public PointMass[][] points; // someone on the internet said jagged arrays are faster than rectangular arrays + im more used to them

    // Constructor
    public Grid(float mass, float massVelocityDamping,
        int verticalPrecision, int horizontalPrecision,
        float springDisplacementFactor = 2f, float springTaughtness = 1f,
        float borderAnchorStiffness = 0.1f, float borderAnchorDamping = 0.1f, 
        int incrementAnchor = 3, float incrementAnchorStiffness = 0.002f, float incrementAnchorDamping = 0.02f,
        float stiffness = 0.28f, float damping = 0.06f) {

        // The grid settings.
        Vector3 offset = new Vector3(GameRules.PixelsHorizontal, GameRules.PixelsVertical, 0f) / (2f * GameRules.PixelsPerUnit) - new Vector3(0.25f, 0.25f, 0f);
        float scaleX = ((float)(GameRules.PixelsHorizontal / GameRules.PixelsPerUnit)) / (float)horizontalPrecision; // Distance We Need To Cover / Amount
        float scaleY = ((float)(GameRules.PixelsVertical / GameRules.PixelsPerUnit)) / (float)verticalPrecision; // Distance We Need To Cover / Amount

        // The physical properties
        PointMass.InvMass = 1f / mass;
        PointMass.Damping = massVelocityDamping;

        // The point masses.
        points = new PointMass[verticalPrecision][];
        PointMass[][] fixedPoints = new PointMass[verticalPrecision][];  // these fixed points will be used to anchor the grid to fixed positions on the screen

        for (int i = 0; i < verticalPrecision; i++) {
            points[i] = new PointMass[horizontalPrecision];
            fixedPoints[i] = new PointMass[horizontalPrecision];
            for (int j = 0; j < horizontalPrecision; j++) {
                Vector3 position = new Vector3(j * scaleX, i * scaleY, 0) - offset;
                points[i][j] = new PointMass(position);
                fixedPoints[i][j] = new PointMass(position);

            }
        }

        List<Spring> springList = new List<Spring>();
        Spring.DisplacementFactor = springDisplacementFactor;
        Spring.Taughtness = springTaughtness;

        // link the point masses with springs
        for (int i = 0; i < verticalPrecision; i++) {
            for (int j = 0; j < horizontalPrecision; j++) {

                // Anchor the border of the grid  --- this tracks.
                if (i == 0 || j == 0 || i == verticalPrecision - 1 || j == horizontalPrecision - 1) { 
                    springList.Add(new Spring(fixedPoints[i][j], points[i][j], borderAnchorStiffness, borderAnchorDamping));
                }
                // Loosely anchor 1/9th of the point masses ??? --- why am i doing this?
                else if (i % incrementAnchor == 0 && j % incrementAnchor == 0) {
                    springList.Add(new Spring(fixedPoints[i][j], points[i][j], incrementAnchorStiffness, incrementAnchorDamping));
                }

                if (i > 0) {
                    springList.Add(new Spring(points[i - 1][j], points[i][j], stiffness, damping));
                }
                if (j > 0) {
                    springList.Add(new Spring(points[i][j - 1], points[i][j], stiffness, damping));
                }

            }
        }

        springs = springList.ToArray();
    }

    // Updating
    public void Update(float deltaTime) {

        for (int i = 0; i < springs.Length; i++) {
            springs[i].Update(deltaTime);
        }
        for (int i = 0; i < points.Length; i++) {
            for (int j = 0; j < points[i].Length; j++) {
                points[i][j].Update(deltaTime);
            }

        }
    }

    /* --- Rendering --- */


    /* --- Manipulations --- */
    public void ApplyDirectedForce(Vector3 force, Vector3 position, float radius, float factor = 10f) {
        for (int i = 0; i < points.Length; i++) {
            for (int j = 0; j < points[i].Length; j++) {
                if ((position - points[i][j].position).sqrMagnitude < radius * radius) {
                    points[i][j].ApplyForce(factor * force / (factor + Vector3.Distance(position, points[i][j].position)));
                }
            }
        }
    }

    public void ApplyImplosiveForce(float force, Vector3 position, float radius, float factor = 10f, float dampingFactor = 0.6f) {

        for (int i = 0; i < points.Length; i++) {
            for (int j = 0; j < points[i].Length; j++) {
                float sqrDistance = (position - points[i][j].position).sqrMagnitude;
                if (sqrDistance < radius * radius) {
                    points[i][j].ApplyForce(factor * force * (position - points[i][j].position) / (factor * factor + sqrDistance));
                    points[i][j].damping *= dampingFactor;
                }
            }
        }
    }

    public void ApplyExplosiveForce(float force, Vector3 position, float radius, float factor = 100f, float dampingFactor = 0.6f) {

        for (int i = 0; i < points.Length; i++) {
            for (int j = 0; j < points[i].Length; j++) {
                float sqrDistance = (position - points[i][j].position).sqrMagnitude;
                if (sqrDistance < radius * radius) {
                    points[i][j].ApplyForce(factor * force * (points[i][j].position - position) / (factor * factor + sqrDistance));
                    points[i][j].damping *= dampingFactor;
                }
            }
        }
    }

}
