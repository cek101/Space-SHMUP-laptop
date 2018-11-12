using System.Collections; // required for arrays and other collections
using System.Collections.Generic; // required to use Lists or dictionaries
using UnityEngine; // required for unity

public class Utils : MonoBehaviour {

    //=============================================== Materials Functions ===============================================

    // returns a list of aall materials on this GameObject and its children
    static public Material[] GetAllMaterials(GameObject go) {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();

        List<Material> mats = new List<Material>();
        foreach (Renderer rend in rends) { 
            mats.Add(rend.material);
        }

        return (mats.ToArray());
    }

} 