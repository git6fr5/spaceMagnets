/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(PlanetGenerator))]
[RequireComponent(typeof(Radial))]
[RequireComponent(typeof(Score))]
public class Planet : MonoBehaviour {

    /* --- Components --- */
    private PlanetGenerator planetGen;
    private Radial forceField;
    private Score score;

    /* --- Properties --- */
    public string planetName;
    public int scoreValue;
    public int mass;

    // Generation
    public bool generated = false;

    // Implosion
    public bool implode;
    private bool imploding = false;
    public float implosionDuration;
    public float implosionSpeed;
    public int implodeMassIncrements;

    /* --- Unity --- */
    private void Start() {
        // Cache these components
        planetGen = GetComponent<PlanetGenerator>();
        forceField = GetComponent<Radial>();
        score = GetComponent<Score>();

        // Set up the components
        SetForceField();
        score.value = scoreValue;

    }

    private void Update() {

        if (!generated) {
            planetGen.Load(planetName);
            planetGen.nextFrame = true;
            generated = true;
        }

        SetForceField();

        if (implode) {
            StartCoroutine(IEImplode(implosionDuration));
            implode = false;
            imploding = true;
        }

        if (imploding) {
            mass += implodeMassIncrements;
        }
    }

    private void OnTriggerStay2D(Collider2D collider) {
        CheckDeath(collider);
    }

    private IEnumerator IEImplode(float delay) {

        planetGen.refreshType = PlanetGenerator.RefreshType.Implode;
        planetGen.autoRebuild = true;
        planetGen.implosionSpeed = implosionSpeed * 2f;
        planetGen.rotationSpeed = 0f;
        planetGen.animationFrameRate = 60;

        yield return new WaitForSeconds(0.2f);

        planetGen.implosionSpeed = implosionSpeed;
        mass = 10;

        yield return new WaitForSeconds(0.2f);

        planetGen.implosionSpeed = implosionSpeed / 2f;
        mass = 0;

        yield return new WaitForSeconds(delay - 0.4f);

        Destroy(gameObject);
        // planetGen.Load(planetName);

        yield return null;

    }     

    /* --- Methods --- */
    private void SetForceField() {
        forceField.isActive = true;
        forceField.direction = Force.Direction.Pull;
        forceField.mass = mass;
    }

    /* --- Methods --- */
    private void CheckDeath(Collider2D collider) {
        Shuttle shuttle = collider.GetComponent<Shuttle>();
        if (shuttle != null) {
            print("Died");
            // Add the score
            Destroy(shuttle.gameObject);
        }
    }

}
