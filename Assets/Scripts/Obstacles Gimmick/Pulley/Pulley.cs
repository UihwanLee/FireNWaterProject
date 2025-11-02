using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulley : MonoBehaviour, InteractWithController
{
    [SerializeField] private GameObject chainPrefab;
    [SerializeField] private GameObject stoolPrefab;

    [SerializeField] private Transform anchor;
    [SerializeField] private int chainCount = 4;
    [SerializeField] private Vector2 stoolScale = new Vector2(1.6f, 1f);

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

        Rigidbody2D prevRb = anchor.GetComponent<Rigidbody2D>();

        for (int i=0; i< chainCount; i++)
        {
            HingeJoint2D chain = Instantiate(chainPrefab, anchor).GetComponent<HingeJoint2D>();
            Rigidbody2D currentRb = chain.GetComponent<Rigidbody2D>();

            finalHeight = posY + (extraHeight * i);
            chain.transform.localPosition = new Vector3(0f, finalHeight, 0f);

            chain.connectedBody = prevRb;

            prevRb = currentRb;
        }

        HingeJoint2D stool = Instantiate(stoolPrefab, anchor).GetComponent<HingeJoint2D>();
        stool.transform.localPosition = new Vector3(0f, finalHeight + extraHeight, 0f);
        stool.transform.localScale = stoolScale;
        stool.connectedBody = prevRb;
    }
}
