using System.Collections; // required for arrays and other collections
using System.Collections.Generic; // required to use Lists or dictionaries
using UnityEngine; // required for unity

public class PowerUp : MonoBehaviour  {
    [Header("Set in Inspector")]
    // This is an unusual but handy use of Vector2s.  x holds a min value
    // and y a max value for a Random.Range() that will be called later
    public                  Vector2 rotMinMax = new Vector2(15, 90);
    public                  Vector2 driftMinMax = new Vector2(.25f, 2);
    public float            lifeTime = 6f; // seconds the PowerUp exists
    public float            fadeTime = 4f; // seconds it will then fade

    [Header("Set Dynamically")]
    public WeaponType       type; // the type of the powerup
    public GameObject       cube; // reference to the cube child
    public TextMesh         letter; // reference to the textmesh
    public Vector3          rotPerSecond; // Euler rotation speed
    public float            birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    void Awake () {
        // find the cube reference 
        cube = transform.Find("Cube").gameObject;
        // find the TextMesh and other components
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        // set a random velocity
        Vector3 vel = Random.onUnitSphere; // get random xyz velocity 
        // random.onUnitySphere gives you a vector point that is somewhere on
        // the surface of teh sphere with a radius of 1m around the origin

        vel.z = 0; // flatten the vel to the XY plane
        vel.Normalize(); // Normalizing a Vector3 makes it length 1m

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        // set the rotation of this GameObject to R: [0, 0, 0]
        transform.rotation = Quaternion.identity;
        // quaternion.identity is equal to no rotation.

        // set up the rotPerSecond for the Cube child using rotMinMax x & y
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y) );

        birthTime = Time.time;
    }

    void Update () {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        // fade out the PowerUp over time
        // given the default values, a PowerUP will exist for 10 seconds
        // and then fade out over 4 seconds
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        // for the lifeTime seconds, u will be <= 0. Then it will transition to 
        // 1 over the course of fadeTime seconds.

        // if u >= 1, destroy this PowerUp
        if (u >= 1) {
            Destroy(this.gameObject);
            return;
        }

        // use u to determine the alpha value of the Cube & letter
        if (u>0) {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;
            // fade the letter too, just not as much
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen) {
            // if the PowerUp has drifted entirely ff screen, destroy it.
            Destroy(gameObject);
        }
    }
    
    public void SetType ( WeaponType wt) {
        // grab the WeaponDefinition from Main
       // //WeaponDefinition def = Main.GetWeaponDefinition(wt);
        // Set the color of the Cube child
        // //cubeRend.material.color = def.color;
        // letter.color = def.letter; // we could colorize the letter too

        // //letter.text = def.letter; // set the letter that is shown too
        type = wt; // finally actually set the type
    }   

    public void AbsorbedBy ( GameObject target) {
        // this function is called by the Hero class when a PowerUp is collected
        // we could tween into hte target and shrink in size, 
        // but for now, just destroy this.gameObject
        Destroy(this.gameObject);
    }
}