using System.Collections; // required for arrays and other collections
using System.Collections.Generic; // required to use Lists or dictionaries
using UnityEngine; // required for unity
using UnityEngine.SceneManagement;  // for loading and reloading of scenes

public class Main : MonoBehaviour {
    static public Main S; // singleton for main
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[]         prefabEnemies; // array of enemy prefabs
    public float                enemySpawnPerSecond = 0.5f; // # enemies/second
    public float                enemyDefaultPadding = 1.5f; // padding for position
    public WeaponDefinition[]   weaponDefinitions;
    public GameObject           prefabPowerUp;

    public WeaponType[] powerUpFrequency = new WeaponType[] {
        WeaponType.blaster,
        WeaponType.blaster,
        WeaponType.spread,
        WeaponType.shield }; // is this weaponType.shi?

    private BoundsCheck bndCheck;

    public void ShipDestroyed( Enemy e) { 
        // potentially generate a PowerUp
        if (Random.value <= e.powerUpDropChance) {
            // choose which PowerUp to pick
            // pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];

            // spawn a powerUp
            GameObject go = Instantiate( prefabPowerUp ) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // set it to the proper WeaponType
            pu.SetType(puType);

            // set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }

    }

    // Use this for initialization
    void Awake () {
        S = this;
        // set bndCheckto reference the BoundsCheck component on this GameObject
        bndCheck = GetComponent<BoundsCheck>();
        // invoke SpawnEnemy () once (in 2 seconds, based on default values)
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        // a generic dictionary with WeaponType as the key 
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach ( WeaponDefinition def in weaponDefinitions) {
            WEAP_DICT[def.type] = def;
        }
	}

    public void SpawnEnemy() {
        //Debug.Log("Spawningsomethiong at" + Time.time);
        // pick a random Enemy prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]); //Error here

        // position the enemy above the screen with a random x position
        float enemyPadding = enemyDefaultPadding; // book typo?
        //float enemyDefaultPadding; 
        if (go.GetComponent<BoundsCheck>() != null) {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);

            // set the initial position for the spawned enemy
            Vector3 pos = Vector3.zero;
            float xMin = -bndCheck.camWidth + enemyPadding;
            float xMax = bndCheck.camWidth - enemyPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = bndCheck.camHeight + enemyPadding;
            go.transform.position = pos;

            // invoke SpawnEnemy () again
            Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        }
    }

    public void DelayedRestart(float delay) {
        // invoke the restart () method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart() {
        // reload _Scene_0 to restart the game
        SceneManager.LoadScene("_Scene_0");
    }

    /* <summary>
        static function that gets a WeaponDefinition from the WEAP_Dict static 
        protected field of the main class
        </summary>
        <returns> the weaponDefinition or, if there is no WeaponDefinition with 
        the WeaponType passed in, returns a new WeaponDefinition with a 
        WeaponTYpe of none..</returns>
        <param name = "wt"> The WeaponType of the desired WeaponDefinition</param>
    */

    static public WeaponDefinition GetWeaponDefinition (WeaponType wt) {
        // check to make sure that the key exists in the dictionary 
        // attempting to retrieve a key that didn't exist, would throw an error,
        // so the following if statement is important
        if (WEAP_DICT.ContainsKey(wt)) {
            return (WEAP_DICT[wt]);
        }
        // this returns a new WeaponDefinition with a type of WeaponTYpe.none, 
        // which means it has failed to find the right WeaponDefinition
        return (new WeaponDefinition());
    }
} // first bracket

		

