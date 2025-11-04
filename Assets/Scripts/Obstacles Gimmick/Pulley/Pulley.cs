using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OriginTransform
{
    public Vector3 position;
    public Quaternion rotation;

    public OriginTransform(Vector3 pos, Quaternion rot)
    {
        this.position = pos;
        this.rotation = rot;
    }
}

public class Pulley : MonoBehaviour, InteractWithController, IObstacle
{
    [SerializeField] private GameObject chainPrefab;
    [SerializeField] private GameObject stoolPrefab;

    [SerializeField] private Transform anchor;
    [SerializeField] private int chainCount = 4;
    [SerializeField] private Vector2 stoolScale = new Vector2(1.6f, 1f);

    private List<HingeJoint2D> pulleyObjects = new List<HingeJoint2D>();
    private List<OriginTransform> originTransforms = new List<OriginTransform>();

    public void Init()
    {
        for (int i= 0; i<pulleyObjects.Count; i++)
        {
            Rigidbody2D rb = pulleyObjects[i].GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;

            pulleyObjects[i].transform.localPosition = originTransforms[i].position;
            pulleyObjects[i].transform.localRotation = originTransforms[i].rotation;
        }
    }

    public void Activate(BaseController bc)
    {
        
    }

    private void Start()
    {
        GenerateChainAndStool();
    }

    private void GenerateChainAndStool()
    {
        float posY = -0.65f;
        float extraHeight = -0.45f;
        float finalHeight = 0.0f;

        Vector3 originPos;
        Quaternion originRot;

        Rigidbody2D prevRb = anchor.GetComponent<Rigidbody2D>();

        for (int i=0; i< chainCount; i++)
        {
            HingeJoint2D chain = Instantiate(chainPrefab, anchor).GetComponent<HingeJoint2D>();
            Rigidbody2D currentRb = chain.GetComponent<Rigidbody2D>();

            finalHeight = posY + (extraHeight * i);
            chain.transform.localPosition = new Vector3(0f, finalHeight, 0f);

            chain.connectedBody = prevRb;

            prevRb = currentRb;

            pulleyObjects.Add(chain);

            originPos = chain.transform.localPosition;
            originRot = chain.transform.localRotation;
            originTransforms.Add(new OriginTransform(originPos, originRot));

            Debug.Log("초기 생성" + originPos);
        }

        HingeJoint2D stool = Instantiate(stoolPrefab, anchor).GetComponent<HingeJoint2D>();
        stool.transform.localPosition = new Vector3(0f, finalHeight + extraHeight, 0f);
        stool.transform.localScale = stoolScale;
        stool.connectedBody = prevRb;

        pulleyObjects.Add(stool);

        originPos = stool.transform.localPosition;
        originRot = stool.transform.localRotation;
        originTransforms.Add(new OriginTransform(originPos, originRot));
    }
}
