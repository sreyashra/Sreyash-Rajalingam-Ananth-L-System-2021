using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tree5Class : MonoBehaviour
{
    [SerializeField]
    private int iterations = 7;
    [SerializeField]
    private float branchLength = 1f;
    [SerializeField]
    private float branchAngle = 25.7f;

    [SerializeField] private GameObject branch;

    public TextMeshProUGUI iterationCounter;
    public TextMeshProUGUI lengthCounter;
    public TextMeshProUGUI angleCounter;
    
    private const string axiom = "X";
    
    private Stack<TransformInfo> transformStack;
    private Dictionary<char, string> rules;
    private string currentString = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        transformStack = new Stack<TransformInfo>();

        rules = new Dictionary<char, string>()
        {
            {'X', "F[+X][-X]FX"},
            {'F', "FF"}
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (iterations <= 0)
        {
            iterations = 0;
        }

        if (branchAngle <= 0f)
        {
            branchAngle = 0f;
        }

        if (branchAngle >= 360f)
        {
            branchAngle = 360f;
        }
        
        if (branchLength <= 0f)
        {
            branchLength = 0f;
        }

        iterationCounter.text = iterations.ToString();
        lengthCounter.text = branchLength.ToString();
        angleCounter.text = branchAngle.ToString();
    }

    public void TreeGenerateButton()
    {
        Generate();
    }

    public void AddValueIteration()
    {
        iterations +=1;
    }

    public void DeductValueIteration()
    {
        iterations -=1;
    }

    public void AddValueAngle()
    {
        branchAngle += 1;
    }

    public void DeductValueAngle()
    {
        branchAngle -= 1;
    }

    public void AddValueLength()
    {
        branchLength += 1;
    }

    public void DeductValueLength()
    {
        branchLength -= 1;
    }

    private void Generate()
    {
        currentString = axiom;

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < iterations; i++)
        {
            foreach (char c in currentString)
            {
                stringBuilder.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }

            currentString = stringBuilder.ToString();
            stringBuilder = new StringBuilder();
        }

        foreach (char c in currentString)
        {
            switch (c)
            {
                case 'F':
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * branchLength);

                    GameObject treeSegment = Instantiate(branch);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    break;
                case 'X':
                    break;
                case '+':
                    transform.Rotate(Vector3.back * branchAngle);
                    break;
                case '-':
                    transform.Rotate(Vector3.forward * branchAngle);
                    break;
                case '[':
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;
                case ']':
                    TransformInfo transformInfo = transformStack.Pop();
                    transform.position = transformInfo.position;
                    transform.rotation = transformInfo.rotation;
                    
                    break;
                default:
                    throw new InvalidOperationException("Invalid L-System Operation");
            }
        }
    }
}