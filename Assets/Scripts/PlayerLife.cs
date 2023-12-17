//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PlayerLife : MonoBehaviour
//{

//    private void Update()
//    {
//        if (transform.position.y < 0f)
//        {
//            Die();
//        }

//    }

//   void Die()
//    {
//        GetComponent<MeshRenderer>().enabled = false;
//        GetComponent<Rigidbody>().isKinematic = true;
//        GetComponent<PlayerInput>().enabled = false;
//        Invoke(nameof(ReloadLevel), 1.3f);
//    }

//    void ReloadLevel()
//    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }    

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
