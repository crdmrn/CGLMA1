using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	[System.Serializable]
	public class LevelScore {
		public string levelName = "test name";
		public float score;
	}

	public List<LevelScore> numbers = new List<LevelScore>();

    // Start is called before the first frame update
    void Awake() {
		if (Instance != null)
			Destroy(this.gameObject);
		else
			Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
