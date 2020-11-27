using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// BEGIN 2d_gamemanager
// Manages the game state.
public class GameManager : Singleton<GameManager> {

	public int healthPoints = 10;


	// The location where the gnome should appear.
	public GameObject startingPoint;

	// The rope object, which lowers and raises the gnome.
	public Rope rope;

	// The fade-out script (triggered when the game resets)
	public Fade fade;

	// The follow script, which will follow the gnome
	public CameraFollow cameraFollow;

	// The 'current' gnome (as opposed to all those dead ones)
	Gnome currentGnome;

	// The prefab to instantiate when we need a new gnome
	public GameObject gnomePrefab;

	// The UI component that contains the 'restart' and 'resume' buttons
	public RectTransform mainMenu;

	// The UI component that contains the 'up', 'down' and 'menu' buttons
	public RectTransform gameplayMenu;

	// The UI component that contains the 'you win!' screen
	public RectTransform gameOverMenu;

	public RectTransform healthBar;

	public Sprite fullHeart;

	public Sprite emptyHeart;

	public string objectThatHitName = "";

	public float impetusForce = 10f;

	// If true, ignore all damage (but still show damage effects)
	// The 'get; set;' make this a property, to make it show
	// up in the list of methods in the Inspector for Unity Events
	public bool gnomeInvincible { get; set; }

	public bool gnomeHasShield { get; set; }

	// How long to wait after dying before creating a new gnome
	public float delayAfterDeath = 1.0f;

	// The sound to play when the gnome dies
	public AudioClip gnomeDiedSound;

	// The sound to play when the game is won
	public AudioClip gameOverSound;

	//public Transform pointToTeleport;

	Transform LegRope;

	// BEGIN 2d_gamemanager_start_reset
	void Start() {
		// When the game starts, call Reset to set up the gnome.
		Reset ();

		LegRope = currentGnome.transform.Find("Leg Rope");


		currentGnome.transform.parent = GameObject.Find("GameObject").transform;
		//StartCoroutine(TeleportColldected(pointToTeleport));
	}

	//IEnumerator LateStart(float waitTime)
	//{

	//	Vector3 pointToLeg = new Vector3(pointToTeleport.position.x + 0.55f, pointToTeleport.position.y + 2.5f, pointToTeleport.position.z);
	//	Vector3 distance = rope.transform.position - pointToLeg;
	//	float distanceY = distance.y;

	//	Debug.Log(distanceY);

	//	float distancex = distance.x;
	//	Debug.Log("Length added");

	//	SpringJoint2D legJoint = LegRope.GetComponent<SpringJoint2D>();

	//	legJoint.connectedBody = null;

	//	legJoint.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;


	//	currentGnome.transform.position = pointToTeleport.position;



	//	currentGnome.transform.Find("Body").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

	//	int howManySegmentsToCreate = (int)(distanceY);

	//	if ((distanceY - (int)distanceY) > 0f)
	//	{
	//		howManySegmentsToCreate++;
	//	}

	//	Debug.Log(howManySegmentsToCreate + " ; " + distanceY);

	//	for (int i = 0; i < howManySegmentsToCreate; i++)
	//	{
	//		rope.CreateRopeSegment();
	//	}

	//	yield return new WaitForSeconds(Time.deltaTime);

	//	for (int i = 1; i < howManySegmentsToCreate; i++)
	//	{
	//		rope.ropeSegments[i].GetComponent<SpringJoint2D>().distance = 1f;
	//	}

	//	if ((distanceY - (int)distanceY) > 0f)
	//	{
	//		rope.ropeSegments[howManySegmentsToCreate].GetComponent<SpringJoint2D>().distance = distanceY - (int)distanceY;
	//	}

	//	yield return new WaitForSeconds(Time.deltaTime);

	//	for (int i = 1; i < howManySegmentsToCreate; i++)
	//	{
	//		rope.ropeSegments[i].transform.position = new Vector3(rope.ropeSegments[i].transform.position.x, rope.ropeSegments[i].transform.position.y - i * 1f, rope.ropeSegments[i].transform.position.z);
	//	}
	//	if ((distanceY - (int)distanceY) > 0f)
	//	{
	//		rope.ropeSegments[howManySegmentsToCreate].transform.position = new Vector3(rope.ropeSegments[howManySegmentsToCreate].transform.position.x,
	//			rope.ropeSegments[howManySegmentsToCreate].transform.position.y - (howManySegmentsToCreate + (distanceY - (int)distanceY) - 1), rope.ropeSegments[howManySegmentsToCreate].transform.position.z);
	//		Debug.Log(rope.ropeSegments[howManySegmentsToCreate].transform.position);
	//	}

	//	legJoint.connectedBody = rope.ropeSegments[howManySegmentsToCreate].GetComponent<Rigidbody2D>();

	//	yield return new WaitForSeconds(2 * Time.deltaTime);

	//	foreach (GameObject segment in rope.ropeSegments)
	//	{
	//		segment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
	//	}

	//	yield return new WaitForSeconds(4 * Time.deltaTime);

	//	foreach (GameObject segment in rope.ropeSegments)
	//	{
	//		segment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
	//	}


	//	//yield return new WaitForSeconds(Time.deltaTime);


	//	legJoint.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;


	//	currentGnome.transform.Find("Body").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

	//}


	// Reset the entire game.
	public void Reset() {

        // Turn off the menus, turn on the gameplay UI
        if (gameOverMenu)
            gameOverMenu.gameObject.SetActive(false);

        if (mainMenu)
            mainMenu.gameObject.SetActive(false);

        if (gameplayMenu)
            gameplayMenu.gameObject.SetActive(true);

        // Find all Resettable components and tell them to reset
        var resetObjects = FindObjectsOfType<Resettable>();

        foreach (Resettable r in resetObjects) {
            r.Reset();
        }

        // Make a new gnome
        CreateNewGnome();

        // Un-pause the game
        Time.timeScale = 1.0f;
    }
    // END 2d_gamemanager_start_reset


    // BEGIN 2d_gamemanager_createnewgnome
	void CreateNewGnome() {

        // Remove the current gnome, if there is one
        RemoveGnome();

        // Create a new gnome object, and make it be our currentGnome
        GameObject newGnome = (GameObject)Instantiate(gnomePrefab, 

            startingPoint.transform.position, 

            Quaternion.identity);                                                     
        currentGnome = newGnome.GetComponent<Gnome>();

        // Make the rope visible
        rope.gameObject.SetActive(true);

        // Connect the rope's trailing end to whichever rigidbody the 
        // Gnome object wants (e.g. his foot)
        rope.connectedObject = currentGnome.ropeBody;

        // Reset the rope's length to the default
        rope.ResetLength();

        // Tell the cameraFollow to start tracking the new gnome object
        cameraFollow.target = currentGnome.cameraFollowTarget;

		healthPoints = 10;

		foreach(Transform heart in healthBar.transform)
        {
			if (heart.GetComponent<Image>() != null)
            {
				Image heartImage = heart.GetComponent<Image>();
				heartImage.sprite = fullHeart;
			}
		}


		

	}
    // END 2d_gamemanager_createnewgnome

    // BEGIN 2d_gamemanager_removegnome
	void RemoveGnome() {

		// Don't actually do anything if the gnome is invincible
		if (gnomeInvincible)
			return;

		// Hide the rope
		rope.gameObject.SetActive(false);

		// Stop tracking the gnome
		cameraFollow.target = null;

		// If we have a current gnome, make that no longer be the player
		if (currentGnome != null) {

			// This gnome is no longer holding the treasure
			currentGnome.holdingTreasure = false;

			// Mark this object as not the player (so that 
            // colliders won't report when the object 
            // hits them)
			currentGnome.gameObject.tag = "Untagged";

			
			// Find everything that's currently tagged "Player", 
            // and remove that tag
			foreach (Transform child in currentGnome.transform) {
				child.gameObject.tag = "Untagged";
				
			}

		

			currentGnome.isDead = false;

			// Mark ourselves as not currently having a gnome
			currentGnome = null;
		}
	}
    // END 2d_gamemanager_removegnome

    // Kills the gnome.
    // BEGIN 2d_gamemanager_killgnome
    void KillGnome(Gnome.DamageType damageType, Transform objectThatInteracted) {



        // Show the damage effect
		if (gnomeHasShield == false)
        {
			//currentGnome.ShowDamageEffect(damageType);
		}
        

		// If we have an audio source, play "gnome died" sound
        var audio = GetComponent<AudioSource>();
        if (audio) {
            audio.PlayOneShot(this.gnomeDiedSound);
        }

        // If we're not invincible, reset the game and make
        // the gnome not be the current player.
        if (gnomeInvincible == false) {

			if (healthPoints > 0)
            {

				if ((objectThatHitName == "Arm Loose" || objectThatHitName == "Leg Dangle") && gnomeHasShield != true)
				{
						healthPoints--;
						GameObject heart = healthBar.GetChild(healthPoints).gameObject;
						Image heartImage = heart.GetComponent<Image>();
						heartImage.sprite = emptyHeart;
						currentGnome.ShowDamageEffect(damageType);
				}
				else if (objectThatHitName == "Arm Holding" || objectThatHitName == "Leg Rope")
                {
					currentGnome.ShowDamageEffect(damageType);
					healthPoints--;
					GameObject heart = healthBar.GetChild(healthPoints).gameObject;
					Image heartImage = heart.GetComponent<Image>();
					heartImage.sprite = emptyHeart;

					if (healthPoints > 0 && gnomeHasShield != true)
                    {
						healthPoints--;
						heart = healthBar.GetChild(healthPoints).gameObject;
						heartImage = heart.GetComponent<Image>();
						heartImage.sprite = emptyHeart;
					}
				}
				else if (gnomeHasShield == false)
                {
					currentGnome.ShowDamageEffect(damageType);
					healthPoints--;
					GameObject heart = healthBar.GetChild(healthPoints).gameObject;
					Image heartImage = heart.GetComponent<Image>();
					heartImage.sprite = emptyHeart;
				}

				if(gnomeHasShield)
                {
					gnomeHasShield = false;
					currentGnome.deactivateOutline();
				}
				
				Vector2 impetusVector = (currentGnome.transform.position - objectThatInteracted.transform.position).normalized;

				Transform gnomeBody = currentGnome.transform.Find("Body");

				gnomeBody.GetComponent<Rigidbody2D>().AddForce(impetusVector * impetusForce);
            }
			
			if (healthPoints == 0)
            {
				// Tell the gnome that it died
				currentGnome.DestroyGnome(damageType);

				// Remove the Gnome
				RemoveGnome();

				// Reset the game
				StartCoroutine(ResetAfterDelay());
			}
        }
    }
    // END 2d_gamemanager_killgnome

    // BEGIN 2d_gamemanager_reset
    // Called when gnome dies.
    IEnumerator ResetAfterDelay() {

        // Wait for delayAfterDeath seconds, then call Reset
        yield return new WaitForSeconds(delayAfterDeath);
        Reset();
    }
    // END 2d_gamemanager_reset

    // BEGIN 2d_gamemanager_ontouch
	// Called when the player touches a trap
	public void TrapTouched(Transform trap) {
		KillGnome(Gnome.DamageType.Slicing, trap);
	}

	// Called when the player touches a fire trap
	public void FireTrapTouched(Transform trap) {
		KillGnome(Gnome.DamageType.Burning, trap);
	}

    // Called when the gnome picks up the treasure.
    public void TreasureCollected() {
        // Tell the currentGnome that it should have the treasure.
        currentGnome.holdingTreasure = true;
    }

	public void HealCollected(Transform heal)
    {
		if (healthPoints < 10)
		{
			healthBar.GetChild(healthPoints).GetComponent<Image>().sprite = fullHeart;
			healthPoints++;

			StartCoroutine(plusOneAnimation(healthBar.Find("+1 Text")));

			heal.GetComponent<SpriteRenderer>().enabled = false;
			heal.GetComponent<Collider2D>().enabled = false;
		}
	}

	IEnumerator plusOneAnimation(Transform onePlusSign)
	{
		onePlusSign.gameObject.SetActive(true);
		yield return new WaitForSeconds(1);
		onePlusSign.gameObject.SetActive(false);
	}


	public void ShieldCollected(Transform shield)
    {
		gnomeHasShield = true;

		currentGnome.activateOutline();

		Destroy(shield.gameObject);
		Debug.Log("Shield has been collected successfully");
    }

	public void TeleportGnome(Transform pointToTeleport)
    {
		//currentGnome.transform.position = Vector3.Lerp(currentGnome.transform.position,pointToTeleport.transform.position, 1f);
		
    }

	public void StartTeleportCoroutine(Transform pointToTeleport)
    {
		StartCoroutine(TeleportColldected(pointToTeleport));
    }

	IEnumerator TeleportColldected(Transform pointToTeleport)
	{


		Gnome newGome = Instantiate(gnomePrefab, pointToTeleport.position, Quaternion.identity).GetComponent<Gnome>();

		Destroy(currentGnome.gameObject);

		currentGnome = newGome;

		rope.connectedObject = currentGnome.ropeBody;

		cameraFollow.target = currentGnome.transform.Find("Body").transform;

		if (gnomeHasShield)
        {
			currentGnome.activateOutline();
        }
		

		Vector3 pointToLeg = new Vector3(pointToTeleport.position.x + 0.55f, pointToTeleport.position.y + 2.5f, pointToTeleport.position.z);
		Vector3 distance = rope.transform.position - pointToLeg;
		Vector3 toDistance = rope.transform.position - pointToTeleport.transform.position;
		float distanceY = distance.y;


		float distancex = distance.x;

		SpringJoint2D legJoint = currentGnome.transform.Find("Leg Rope").GetComponent<SpringJoint2D>();


		

		legJoint.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

		legJoint.connectedBody = null;

		rope.ResetLength();



		

		int howManySegmentsToCreate = (int)(distanceY) / 4;

		

		Debug.Log(currentGnome.transform.position);

		if ((distanceY % 4) > 0f)
		{
			howManySegmentsToCreate++;
		}

		for (int i = 0; i < howManySegmentsToCreate; i++)
		{
			rope.CreateRopeSegment();
		}

		foreach (GameObject segment in rope.ropeSegments)
		{
			segment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		}

		yield return new WaitForSeconds(Time.deltaTime);

		//currentGnome.transform.position = pointToTeleport.position;


		currentGnome.transform.Find("Body").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

		//currentGnome.transform.position = new Vector3(rope.gameObject.transform.position.x, rope.gameObject.transform.position.y - toDistance.y, rope.gameObject.transform.position.z);

		for (int i = 1; i < howManySegmentsToCreate; i++)
		{
			rope.ropeSegments[i].GetComponent<SpringJoint2D>().distance = 4f;
		}

		if ((distanceY % 4) > 0f)
		{
			rope.ropeSegments[howManySegmentsToCreate].GetComponent<SpringJoint2D>().distance = distanceY % 4;
		}

		yield return new WaitForSeconds(Time.deltaTime);

		for (int i = 1; i < howManySegmentsToCreate; i++)
		{
			rope.ropeSegments[i].transform.position = new Vector3(rope.transform.position.x, rope.transform.position.y - i * 4f, rope.transform.position.z);
		}
		if ((distanceY - (int)distanceY) > 0f)
		{
			rope.ropeSegments[howManySegmentsToCreate].transform.position = new Vector3(rope.transform.position.x,
				rope.transform.position.y - (howManySegmentsToCreate * 4 + (distanceY % 4)), rope.transform.position.z);
		}


		legJoint.connectedBody = rope.ropeSegments[howManySegmentsToCreate].GetComponent<Rigidbody2D>();

		yield return new WaitForSeconds(2 * Time.deltaTime);

		currentGnome.transform.position = pointToTeleport.position;


		yield return new WaitForSeconds(4 * Time.deltaTime);

		foreach (GameObject segment in rope.ropeSegments)
		{
			segment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		}


		//yield return new WaitForSeconds(Time.deltaTime);


		legJoint.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;


		currentGnome.transform.Find("Body").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

	}


	// END 2d_gamemanager_ontouch

	// BEGIN 2d_gamemanager_exitreached
	// Called when the player touches the exit.
	public void ExitReached() {
		// If we have a player, and that player is holding treasure, 
        // game over!
		if (currentGnome != null && currentGnome.holdingTreasure == 
true) {

			// If we have an audio source, play the game over sound
			var audio = GetComponent<AudioSource>();
			if (audio) {
				audio.PlayOneShot(this.gameOverSound);
			}

			// Pause the game
			Time.timeScale = 0.0f;

			// Turn off the game over menu, and turn on the game 
            // over screen!
			if (gameOverMenu)
				gameOverMenu.gameObject.SetActive(true);

			if (gameplayMenu)
				gameplayMenu.gameObject.SetActive(false);
		}
	}
    // END 2d_gamemanager_exitreached

    // BEGIN 2d_gamemanager_setpaused
	// Called when the Menu button is tapped, and when the Resume game is 
    // tapped.
	public void SetPaused(bool paused) {

		// If we're paused, stop time and enable the menu (and disable 
        // the game overlay)
		if (paused) {
			Time.timeScale = 0.0f;
			mainMenu.gameObject.SetActive(true);
			gameplayMenu.gameObject.SetActive(false);
		} else {
			// If we're not paused, resume time and disable the 
            // menu (and enable the game overlay)
			Time.timeScale = 1.0f;
			mainMenu.gameObject.SetActive(false);
			gameplayMenu.gameObject.SetActive(true);
		}
	}
    // END 2d_gamemanager_setpaused

    // BEGIN 2d_gamemanager_restartgame
	// Called when the Restart button is tapped. 
	public void RestartGame() {

		// Immediately remove the gnome (instead of killing it)
		Destroy(currentGnome.gameObject);
		currentGnome = null;

		// Now reset the game to create a new gnome.
		Reset();
	}
    // END 2d_gamemanager_restartgame

}
// END 2d_gamemanager
