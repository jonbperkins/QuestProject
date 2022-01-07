using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationInstance : MonoBehaviour
{
    public bool startAnimationBit = true;
    private float elapsedTime = 0;
    public bool useObjAxis = true;  //When false, movements and rotations will use world space, instead of local space.  
    public AnimationSegment animationSegment;
    public ControlState controlState;
    public Transform targetObject;


    //Rotate Variables
    public Vector3 rotateDirection;
    public float rotationDegPerSec = 0;
    public float targetRotations = 1;  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN.  Stops after this number of rotations.
    public Vector3 rotateCenter;
    public bool rotateTowardsRightHand = false;
 
    public FunctionType rotateType = FunctionType.CONTINOUS;
    public float rotationsCompleted = 0;
    public bool rotateFinished = false;

    //Move Variables
    public Vector3 moveDirection;
    public float moveCyclesPerSec = 0;
    public float moveDistanceCM;
    public float targetMoveCycles = 1;  //Used for RAMP_RTN, or SIN_RAMP_RTN. Stops after this number of cycles.

    public FunctionType moveType = FunctionType.SIN_RAMP_RTN;
    public float moveCyclesCompleted;
    private float moveSinusoidAmount;
    private float prevMoveSinusoidAmount;
    public bool moveFinished = false;

    //Scale Variables
    public Vector3 scaleDirection;
    public float scaleCyclesPerSec = 0;
    public float scaleShrinkPercent; //-100% or above.  -100% will shrink to Scale of 0;
    public float targetScaleCycles = 1;  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN. 

    public FunctionType scaleType = FunctionType.SIN_RAMP_RTN;
    public float scaleCyclesCompleted;
    private float scaleSinusoidAmount;
    private float prevScaleSinusoidAmount;
    public bool scaleFinished = false;

    //Recolor Variables
    private Renderer objectRenderer;
    public Material initialMaterialInstance;
    public Material recolorMaterial;
    public float reColorIntensity = 1;
    public float recolorCyclesPerSec = 0;
    public float targetRecolorCycles = 0;
    public float recolorCyclesCompleted;
    private float recolorSinusoidAmount;
    private float prevEmissionSinusoidAmount;
    public bool recolorFinished;

    //Initial Values
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    public Color initialColor;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialScale = transform.localScale;

        if (animationSegment != null)  //Currently Some animations are pre-set using animation segment scriptable objects
        {
            LoadAnimationSegementParameters();
        }

        rotateDirection = Vector3.Normalize(rotateDirection);
        rotateCenter = GetObjectCenterPosition();
        moveDirection = Vector3.Normalize(moveDirection);
        scaleDirection = Vector3.Normalize(scaleDirection);

        controlState = FindObjectOfType<AnimateTool>().controlState;
        targetObject = GameObject.Find("RightControllerAnchor").transform;





        if (rotationDegPerSec == 0) rotateFinished = true;  //Don't go through calculations if the function isn't used.
        if (moveCyclesPerSec == 0) moveFinished = true;
        if (scaleCyclesPerSec == 0) scaleFinished = true;
        if (recolorCyclesPerSec == 0) recolorFinished = true;

    }

    private void LoadAnimationSegementParameters()  //Gets animation parameters from a scriptable object (Animation Segment) and set to this animation instance
    {
        //Rotate Variables
        rotateDirection = animationSegment.rotateDirection;
        rotationDegPerSec = animationSegment.rotationDegPerSec;
        targetRotations = animationSegment.targetRotations;  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN.  Stops after this number of rotations.
        rotateType = animationSegment.functTypeRotation;
        rotateTowardsRightHand = animationSegment.rotateTowardsRightHand; 


        //Move Variables
        moveDirection = animationSegment.moveDirection;
        moveCyclesPerSec = animationSegment.moveCyclesPerSec;
        moveDistanceCM = animationSegment.moveDistanceCM;
        targetMoveCycles = animationSegment.targetMoveCycles;  //Used for RAMP_RTN, or SIN_RAMP_RTN. Stops after this number of cycles.
        moveType = animationSegment.functTypeMove;

        //Scale Variables
        scaleDirection = animationSegment.scaleDirection;
        scaleCyclesPerSec = animationSegment.scaleCyclesPerSec;
        scaleShrinkPercent = animationSegment.scaleShrinkPercent; //-100% or above.  -100% will shrink to Scale of 0;
        targetScaleCycles = animationSegment.targetScaleCycles;  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN. 
        scaleType = animationSegment.functTypeScale;

        //Recolor Variables

        recolorCyclesPerSec = animationSegment.recolorCyclesPerSec;
        targetRecolorCycles = animationSegment.recolorTargetCycles;  //Used for RAMP_RTN, or SIN_RAMP_RTN. Stops after this number of cycles.
        reColorIntensity = animationSegment.intensity;
        if (recolorCyclesPerSec != 0)
        {
            if (this.GetComponent<Renderer>() == null)
            {
                objectRenderer = this.GetComponentInChildren<Renderer>();
                initialMaterialInstance = objectRenderer.material;
            }
            else  //If the menu object is a group of objects, find the first material. 
            {
                objectRenderer = this.GetComponent<Renderer>();
                initialMaterialInstance = objectRenderer.material;
                Debug.Log("Initial Material Name is: " + objectRenderer.material);
            }

            recolorMaterial = initialMaterialInstance;

            if (recolorMaterial.IsKeywordEnabled("_EMISSION") == true)
            {
                initialColor = recolorMaterial.GetColor("_EmissionColor");
            }
            else if (recolorMaterial.HasProperty("_Tint"))
            {
                initialColor = recolorMaterial.GetColor("_Tint");
            }
            else
            {
                initialColor = recolorMaterial.GetColor("_Color");
            }
            //if (recolorMaterial.IsKeywordEnabled("_EMISSION") == true)
            //{
            //    initialColor = recolorMaterial.GetColor("_EmissionColor");
            //}
            //else
            //{
            //    initialColor = recolorMaterial.GetColor("_Albedo");
            //}
        }
        //Other Variables
        useObjAxis = animationSegment.useObjectAxis;

        
    }

    /*  TODO:  Could update to use this section instead of the Add functions below.   

     private bool CompareAnimationSegementParameters(AnimationSegment newSegment)  //Gets animation parameters from a scriptable object (Animation Segment) and set to this animation instance
     {

         if (
         //Rotate Variables
         rotateDirection == newSegment.rotateDirection &&
         rotationDegPerSec == newSegment.rotationDegPerSec &&
         targetRotations == newSegment.targetRotations &&  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN.  Stops after this number of rotations.
         rotateType == newSegment.functTypeRotation &&


         //Move Variables
         moveDirection == newSegment.moveDirection &&
         moveCyclesPerSec == newSegment.moveCyclesPerSec &&
         moveDistanceCM == newSegment.moveDistanceCM &&
         targetMoveCycles == newSegment.targetMoveCycles &&  //Used for RAMP_RTN, or SIN_RAMP_RTN. Stops after this number of cycles.
         moveType == newSegment.functTypeMove &&

         //Scale Variables
         scaleDirection == newSegment.scaleDirection &&
         scaleCyclesPerSec == newSegment.scaleCyclesPerSec &&
         scaleShrinkPercent == newSegment.scaleShrinkPercent && // -100% will shrink to Scale of 0;
         targetScaleCycles == newSegment.targetScaleCycles &&  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN. 
         scaleType == newSegment.functTypeScale &&

         //Emission Variables
         emissionCyclesPerSec == newSegment.emissionCyclesPerSec &&
         targetEmissionCycles == newSegment.emissionTargetCycles &&  //Used for RAMP_RTN, or SIN_RAMP_RTN. Stops after this number of cycles.
         emissionIntensity == newSegment.intensity &&
         emissionMaterial == newSegment.emissionMaterial)
         {
             return true;
         }

         else
         {
             return false;
         }
     }
 */



    // Update is called once per frame
    void Update()
    {
        if (startAnimationBit == true)
        {
            if (rotateFinished == false)
            {
                RotateObject();
            }

            if (moveFinished == false)
            {
                moveObject();
            }

            if (scaleFinished == false)
            {
                scaleObject();
            }
            if (recolorFinished == false)
            {
                RecolorObject();
            }

            if (rotateFinished == true && moveFinished == true && scaleFinished == true && recolorFinished == true)  
            {
                Destroy(this);
            }

        }

        elapsedTime += Time.deltaTime;
    }

    private void OnDestroy()
    {
        Destroy(initialMaterialInstance);
    }

    private void RotateObject()
    {
        if (rotationsCompleted + Mathf.Abs((rotationDegPerSec / 360f) * Time.deltaTime) < targetRotations)
        {
            rotationsCompleted += Mathf.Abs((rotationDegPerSec / 360f) * Time.deltaTime);
            if (useObjAxis == true)
            {
                //transform.RotateAround(rotateCenter, rotateDirection, rotationDegPerSec * Time.deltaTime);
                transform.Rotate(rotationDegPerSec * rotateDirection * Time.deltaTime, Space.Self);
            }
            else
            {
                transform.Rotate(rotateDirection, rotationDegPerSec *  Time.deltaTime);  //Update to not use object axis
            }
        }
        else
        { 
            transform.Rotate((targetRotations - rotationsCompleted) * rotateDirection * 360, Space.Self);
            rotateFinished = true;
            rotationDegPerSec = 0;
        }
        if (rotateTowardsRightHand == true)
        {
            if (controlState.rightTriggerPress == true)
            {
                transform.LookAt(targetObject);
            }

            if (controlState.leftTriggerPress == true)  //Pressing left trigger will remove animation script and keep objects in current location 
            {
                Destroy(this);
            }
        }



        //rotationsCompleted += rotationDegPerSec / 360f * Time.deltaTime;
    }

   /* public void AddRotation(Vector3 dir, float rps, float targetRot, int typeNumber)
    {
    rotateDirection = Vector3.Normalize(dir);
    rotationDegPerSec += rps;
    targetRotations = targetRot;  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN.  Stops after this number of rotations.

    rotateType = (FunctionType)typeNumber;
    rotationsCompleted -= Mathf.Round(rotationsCompleted);  //start target counter over but keep partial rotations
    rotateFinished = false;
    }
    */

    private void moveObject()
    {
        if (moveCyclesCompleted < targetMoveCycles)
        {
            moveCyclesCompleted += (moveCyclesPerSec * Time.deltaTime);
            //moveSinusoidAmount = (1 - Mathf.Cos(2.0f * 3.14f * moveCyclesCompleted)) * moveDistanceCM / 100;
            moveSinusoidAmount = (1 - Mathf.Cos(2.0f * 3.14f * moveCyclesCompleted)) * 0.5f;
            Vector3 moveDirectionUsingLocalCoords = moveDirection.x * transform.right +  moveDirection.y * transform.up +  moveDirection.z * transform.forward;
            if (useObjAxis == true)
            {
                transform.localPosition += moveDirectionUsingLocalCoords * (moveSinusoidAmount - prevMoveSinusoidAmount) * moveDistanceCM / 100;
            }
            else
            {
                transform.localPosition += moveDirection * (moveSinusoidAmount - prevMoveSinusoidAmount) * moveDistanceCM / 100;
            }

            prevMoveSinusoidAmount = moveSinusoidAmount;
        }
        else
        {
            moveFinished = true;
        }
    }

    private void RecolorObject()
    {
        if (recolorCyclesCompleted < targetRecolorCycles)
        {
            recolorCyclesCompleted += (recolorCyclesPerSec * Time.deltaTime);
            recolorSinusoidAmount = (1 - Mathf.Cos(2.0f * 3.14f * recolorCyclesCompleted)) * reColorIntensity;

            //Update to "if emission is on".  Else modify albedo color. 
            //Update to make a copy of material for each action group, so that only objects of that group are recolored. 
            if (recolorMaterial.IsKeywordEnabled("_EMISSION") == true)
            {
                recolorMaterial.SetColor("_EmissionColor", initialColor + initialColor * recolorSinusoidAmount);
            }
            else if (recolorMaterial.HasProperty("_Tint"))
            {
                recolorMaterial.SetColor("_Tint", initialColor + initialColor * recolorSinusoidAmount);
            }
            else
            {
                recolorMaterial.SetColor("_Color", initialColor + initialColor * recolorSinusoidAmount);
            }
        }
        else
        {
          //  objectRenderer.material = initialMaterialInstance;
          //  recolorFinished = true;
        }
    }

    public void AddMove(Vector3 moveDir, float cps, float moveCM, float targetCyc, int typeNumber)
    {
        moveDirection = Vector3.Normalize(moveDir);
        moveCyclesPerSec = cps;
        moveDistanceCM += moveCM;
        targetMoveCycles = targetCyc;  //Used for RAMP_RTN, or SIN_RAMP_RTN. Stops after this number of cycles.

        moveType = (FunctionType)typeNumber;
        moveCyclesCompleted -= Mathf.Round(moveCyclesCompleted);  //start target counter over but keep partial move cycles

        moveFinished = false;
    }


    private void scaleObject()
    {
        if (scaleCyclesCompleted < targetScaleCycles)
        {
            scaleCyclesCompleted += scaleCyclesPerSec * Time.deltaTime;
            scaleSinusoidAmount = (1 - Mathf.Cos(2.0f * 3.14f * scaleCyclesCompleted)) * 0.5f; //Sin wave between 0 and 1
            transform.localScale -= (scaleSinusoidAmount - prevScaleSinusoidAmount) * scaleShrinkPercent * .01f * Vector3.Scale(initialScale, scaleDirection);
            prevScaleSinusoidAmount = scaleSinusoidAmount;
        }
        else
        {
            scaleFinished = true;
            scaleShrinkPercent = 0;
        }

    }

    public void AddScale (Vector3 scaleDir, float cps, float shrinkPct, float targetCyc, int typeNumber)
    {
        scaleDirection = Vector3.Normalize(scaleDir);
        scaleCyclesPerSec = cps;
        scaleShrinkPercent += shrinkPct; //-100% or above.  -100% will shrink to Scale of 0;
        targetScaleCycles = targetCyc;  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN. 

        scaleType = (FunctionType)typeNumber;
        scaleCyclesCompleted -= Mathf.Round(scaleCyclesCompleted);

        scaleFinished = false;
    }

    public void Revert()
    {
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
        transform.localScale = initialScale;
        Destroy(this);
    }

    public Vector3 GetObjectCenterPosition()
    {
        List <Mesh>objectMeshes = new List<Mesh>();
        if (gameObject.GetComponent<MeshFilter>() != null)
        {
            objectMeshes.Add(gameObject.GetComponent<MeshFilter>().mesh);

        }
        foreach (Transform myTransform in gameObject.transform)
        // if (obj.GetComponentsInChildren<Mesh>() != null)
        {
            Mesh myMesh;
            if (myTransform.GetComponent<MeshFilter>() != null)
            {
                myMesh = myTransform.GetComponent<MeshFilter>().mesh;
            }
            else if (myTransform.GetComponent<SkinnedMeshRenderer>() != null)
            {
                myMesh = myTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            }
            else
            {
                myMesh = null;
            }

            if (myMesh != null)
            {
                objectMeshes.Add(myMesh);
            }

        }

        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float maxY = 0;
        float minZ = 0;
        float maxZ = 0;
        // GetComponentsInChildren<Mesh>();
        foreach (Mesh mesh in objectMeshes)
        {
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].x < minX)
                {
                    minX = vertices[i].x;
                }
                if (vertices[i].x > maxX)
                {
                    maxX = vertices[i].x;
                }
                if (vertices[i].y < minY)
                {
                    minY = vertices[i].y;
                }
                if (vertices[i].y > maxY)
                {
                    maxY = vertices[i].y;
                }
                if (vertices[i].z < minZ)
                {
                    minZ = vertices[i].z;
                }
                if (vertices[i].z > maxZ)
                {
                    maxZ = vertices[i].z;
                }
            }
        }

        float width = maxX - minX;
        float height = maxY - minY;
        float depth = maxZ - minZ;

        Vector3 centerOffset = new Vector3 ((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);
        Vector3 centerPoint = gameObject.transform.position + centerOffset;

        return centerPoint;
    }



}

public enum FunctionType  //TODO - Update to use these. 
{
    CONTINOUS,
    SIN_RAMP_RTN,
    FOLLOW_LEFT_HAND
}

