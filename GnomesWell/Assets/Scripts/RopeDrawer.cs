using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeDrawer : MonoBehaviour
{
    public Transform pointToTeleport;

    public GameObject currentGnome;

    public Transform connectedObject;

    public GameObject ropeSegmentPrefab;

    public Transform ropeTop;
    
    public Transform ropeSegments;

    public float ropeSegmentLength = 0.4f;
    
    public LineRenderer ropeLineRender;

	public List<GameObject> ropeSegmentsList = new List<GameObject>();

    public float ropeSpeed;

    bool isRopeCreated = false;

    public bool isIncreasing { get; set; }
    public bool isDecreasing { get; set; }




    // Start is called before the first frame update

    private void Awake()
    {
        //     for (int i = 0; i < ropeSegments.childCount; i++)
        //     {
        //ropeSegmentsList.Add(ropeSegments.GetChild(i).gameObject);
        //     }
        isIncreasing = false;

        isDecreasing = false;
    }

    void Start()
    {
        //StartNewRopeCreation();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRopeCreated)
        {
            DrawRope();
        }

        GameObject topSegment = ropeSegmentsList[0];
        DistanceJoint2D topSegmentJoint =
            topSegment.GetComponent<DistanceJoint2D>();


        //if (isIncreasing)
        //{
        //    if (topSegmentJoint.distance >= ropeSegmentLength)
        //    {
        //        //RemoveRopeSegment(1);
        //        CreateRopeSegment();
        //    }

        //    else
        //    {
        //        topSegmentJoint.distance += ropeSpeed *
        //            Time.deltaTime;
        //    }

        //}


        //if (isDecreasing)
        //{
        //    if (topSegmentJoint.distance <= -0.005f)
        //    {
        //        //CreateRopeSegment();
        //        RemoveRopeSegment(1);
        //    }
        //    else
        //    {
        //        topSegmentJoint.distance -=  ropeSpeed *
        //            Time.deltaTime;
        //    }
        //}

        if (isIncreasing)
        {
            if (topSegmentJoint.distance > ropeSegmentLength)
            {
                //RemoveRopeSegment(1);
                CreateRopeSegment();
            }
            else
            {
                topSegmentJoint.distance += ropeSpeed *
                    Time.deltaTime;
            }
            
        }

        if (isDecreasing && ropeSegmentsList.Count > 1)
        {
            if (topSegmentJoint.distance <= 0.005f)
            {
                RemoveRopeSegment(0);
            }
            else
            {
                topSegmentJoint.distance -= ropeSpeed *
                    Time.deltaTime;
            }
        }



    }

    private void DrawRope()
    {
        ropeLineRender.positionCount = ropeSegmentsList.Count + 1; 

        for (int i = 0; i < ropeSegmentsList.Count; i++)
        {
            Transform ropeSegment = ropeSegmentsList[i].transform;
            ropeLineRender.SetPosition(i, ropeSegment.position);
        }

        //треба додати дно до отстанього сегемента, аби відмалювати
        ropeLineRender.SetPosition(ropeSegmentsList.Count, ropeSegmentsList[ropeSegmentsList.Count - 1].transform.Find("LastChainBottom").position);
    }


	public void ResetLength()
	{

		foreach (GameObject segment in ropeSegmentsList)
		{
			Destroy(segment);
		}

        CreateRopeSegment();
	}

    public void StartNewRopeCreation()
    {
        StartCoroutine(CreateNewRope(pointToTeleport));
    }


    IEnumerator CreateNewRope(Transform newGnomePosition)
    {

        Vector3 pointToLeg = new Vector3(newGnomePosition.position.x + 0.55f, newGnomePosition.position.y + 2.2f, newGnomePosition.position.z);
        Vector3 distance = ropeTop.transform.position - pointToLeg;
        Vector3 toDistance = ropeTop.transform.position - newGnomePosition.transform.position;
        float distanceY = distance.y;

        currentGnome.transform.position = pointToTeleport.position;

        Vector2 segmentAnchorPosition = toDistance.normalized * ropeSegmentLength;


        float distancex = distance.x;

        ResetLength();

        int howManySegmentsToCreate = (int)(distanceY / ropeSegmentLength);


        if ((distanceY % ropeSegmentLength) > 0f)
        {
            howManySegmentsToCreate++;
        }

        for (int i = 0; i < howManySegmentsToCreate; i++)
        {
            CreateRopeSegment();
        }

        foreach (GameObject segment in ropeSegmentsList)
        {
            segment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        currentGnome.transform.Find("Body").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //yield return new WaitForSeconds(Time.deltaTime);

        //yield return new WaitForSeconds(Time.deltaTime);

      

        for (int i = 0; i < howManySegmentsToCreate; i++)
        {
            DistanceJoint2D ropeSegmentHingeJoint = ropeSegmentsList[i].GetComponent<DistanceJoint2D>();
            ropeSegmentHingeJoint.distance = ropeSegmentLength;
            //ropeSegmentsList[i].transform.position = new Vector3(ropeTop.transform.position.x, ropeTop.transform.position.y - i * ropeSegmentLength, ropeTop.transform.position.z);
        }
        if ((distanceY - (int)distanceY) > 0f)
        {
            DistanceJoint2D ropeSegmentHingeJoint = ropeSegmentsList[howManySegmentsToCreate].GetComponent<DistanceJoint2D>();
            ropeSegmentHingeJoint.distance = ropeSegmentLength;
            //ropeSegmentsList[howManySegmentsToCreate].transform.position = new Vector3(ropeTop.transform.position.x,
            //ropeTop.transform.position.y - (howManySegmentsToCreate * ropeSegmentLength + (distanceY % ropeSegmentLength)), ropeTop.transform.position.z);
        }

        //yield return new WaitForSeconds(Time.deltaTime);

        for (int i = 1; i < howManySegmentsToCreate; i++)
        {
            ropeSegmentsList[i].transform.position = new Vector3(ropeTop.transform.position.x, ropeTop.transform.position.y - i * ropeSegmentLength, ropeSegmentsList[i].transform.position.z);
        }
        if ((distanceY - (int)distanceY) > 0f)
        {
            ropeSegmentsList[howManySegmentsToCreate].transform.position = new Vector3(ropeTop.transform.position.x,
              ropeTop.transform.position.y - (howManySegmentsToCreate * ropeSegmentLength + (distanceY % ropeSegmentLength)), ropeTop.transform.position.z);
        }


        //legJoint.connectedBody = rope.ropeSegments[howManySegmentsToCreate].GetComponent<Rigidbody2D>();

        //yield return new WaitForSeconds(2 * Time.deltaTime);

        //currentGnome.transform.position = newGnomePosition.position;


        yield return new WaitForSeconds(Time.deltaTime);

        foreach (GameObject segment in ropeSegmentsList)
        {
            segment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }





        isRopeCreated = true;


        //legJoint.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        DistanceJoint2D gnomeHingeJoint = connectedObject.GetComponent<DistanceJoint2D>();


        


        gnomeHingeJoint.connectedBody = ropeSegmentsList[ropeSegmentsList.Count-1].GetComponent<Rigidbody2D>();

        gnomeHingeJoint.enabled = true;

        currentGnome.transform.Find("Body").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

    }

    public void CreateRopeSegment()
	{

		// Create the new rope segment.
		GameObject segment = (GameObject)Instantiate(ropeSegmentPrefab,

			this.transform.position, Quaternion.identity);

		// Make the rope segment be a child of this object, and make 
		// it keep its world position
		segment.transform.SetParent(ropeSegments.transform, true);

		// Get the rigidbody from the segment
		Rigidbody2D segmentBody = segment.GetComponent<Rigidbody2D>();

		// Get the distance joint from the segment
		DistanceJoint2D segmentJoint =
			segment.GetComponent<DistanceJoint2D>();

		// Error if the segment prefab doesn't have a rigidbody or 
		// spring joint - we need both
		if (segmentBody == null || segmentJoint == null)
		{
			Debug.LogError("Rope segment body prefab has no " +
				"Rigidbody2D and/or DistanceJoint2D!");
			return;
		}

		// Now that it's checked, add it to the start of the list 
		// of rope segments
		ropeSegmentsList.Insert(0, segment);
        segment.transform.parent = ropeSegments;

        //segmentJoint.connectedAnchor = new Vector2(0f, -ropeSegmentLength);

        // If this is the FIRST segment, it needs to be connected to 
        // the gnome

        if (ropeSegmentsList.Count == 1)
		{
            
            // Connect the joint on the connected object to the 
            // segment
            //DistanceJoint2D connectedObjectJoint =
            //connectedObject.GetComponent<DistanceJoint2D>();

            //connectedObjectJoint.connectedBody = segmentBody;
            //connectedObjectJoint.distance = 0.1f;
            segmentJoint.connectedBody = ropeTop.GetComponent<Rigidbody2D>();

            GameObject lastChainBottom = Instantiate(new GameObject(), segment.transform);
            lastChainBottom.name = "LastChainBottom";
            Rigidbody2D lastchainBottomRigidbody = lastChainBottom.AddComponent<Rigidbody2D>();
            lastchainBottomRigidbody.bodyType = RigidbodyType2D.Kinematic;
            lastChainBottom.transform.localPosition = new Vector3(lastChainBottom.transform.localPosition.x, lastChainBottom.transform.localPosition.y - ropeSegmentLength,
                lastChainBottom.transform.localPosition.z);
            

            ropeSegmentsList[0].GetComponent<DistanceJoint2D>().connectedBody = ropeTop.GetComponent<Rigidbody2D>();
            // Set this joint to already be at the max length

        }
		else
		{
			// This is an additional rope segment. We now need to 
			// connect the previous top segment to this one

			// Get the second segment
			GameObject nextSegment = ropeSegmentsList[1];

			// Get the joint that we need to attach to
			DistanceJoint2D nextSegmentJoint =
				nextSegment.GetComponent<DistanceJoint2D>();

			// Make this joint connect to us
			nextSegmentJoint.connectedBody = segmentBody;

			// Make this segment start at a distance of 0 units
			// away from the previous one - it will be extended.
			
		}

		// Connect the new segment to the rope anchor (i.e. this object)
		segmentJoint.connectedBody = ropeTop.GetComponent<Rigidbody2D>();
	}


    void RemoveRopeSegment(int id)
    {

        // If we don't have two or more segments, stop.
        if (ropeSegmentsList.Count < 2)
        {
            return;
        }

        // Get the top segment, and the segment under it.
        GameObject topSegment = ropeSegmentsList[id];
        GameObject nextSegment = ropeSegmentsList[id+1];

        // Connect the second segment to the rope's anchor.
        DistanceJoint2D nextSegmentJoint =
            nextSegment.GetComponent<DistanceJoint2D>();

        DistanceJoint2D topSegmentJoint =
            topSegment.GetComponent<DistanceJoint2D>();

        nextSegmentJoint.connectedBody =
            ropeTop.GetComponent<Rigidbody2D>();

        topSegmentJoint.connectedBody =
            ropeTop.GetComponent<Rigidbody2D>();
        // Remove the top segment and destroy it.
        ropeSegmentsList.RemoveAt(id);
        Destroy(topSegment);

    }
}
