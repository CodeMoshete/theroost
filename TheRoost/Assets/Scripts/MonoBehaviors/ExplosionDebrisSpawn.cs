using UnityEngine;
using System.Collections;

public class ExplosionDebrisSpawn : MonoBehaviour {

	private readonly string[] DEBRIS_SMALL = {"Debris/SmallDebris1","Debris/SmallDebris2","Debris/SmallDebris3"};
	private readonly string[] DEBRIS_MED = {"Debris/MedDebris1","Debris/MedDebris2"};
	private readonly string[] DEBRIS_LARGE = {"Debris/LargeDebris1","Debris/LargeDebris2"};

	public int MIN_SMALL = 60;
	public int MAX_SMALL = 120;
	public int MIN_MED = 10;
	public int MAX_MED = 20;
	public int MIN_LARGE = 8;
	public int MAX_LARGE = 15;

	// Use this for initialization
	void Awake ()
	{
		int numSmall = Random.Range(MIN_SMALL,MAX_SMALL);
		int numMed = Random.Range(MIN_MED,MAX_MED);
		int numLarge = Random.Range(MIN_LARGE,MAX_LARGE);
		ArrayList prefabsToSpawn = new ArrayList();

		int i=0;
		for(i=0; i<numSmall; i++)
		{
			prefabsToSpawn.Add(DEBRIS_SMALL[Random.Range(0,DEBRIS_SMALL.Length)]);
		}

		for(i=0; i<numMed; i++)
		{
			prefabsToSpawn.Add(DEBRIS_MED[Random.Range(0,DEBRIS_MED.Length)]);
		}

		for(i=0; i<numLarge; i++)
		{
			prefabsToSpawn.Add(DEBRIS_LARGE[Random.Range(0,DEBRIS_LARGE.Length)]);
		}

		foreach(string prefabName in prefabsToSpawn)
		{
			Instantiate(Resources.Load(prefabName), this.transform.position, Quaternion.identity);
		}

		this.transform.Find("ExplosionMesh").GetComponent<Animator>().SetTrigger("explode");
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
