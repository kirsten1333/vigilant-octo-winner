using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetworks;
using System.IO;

public class NNControler : MonoBehaviour
{
    public Topology topology = new Topology(20, 3, 0.15, 16, 16);
    public NeuralNetwork neuralNetwork;
    void Awake()
    {
        var fileName = Path.Combine(Application.persistentDataPath, "Weights.xml");
        Debug.Log(fileName + "-------------------------------");
        if (File.Exists(fileName))
        {
            Debug.Log("EXIST--------------------------");
        }
        else
        {
            Debug.Log("NOT EXIST-----------------------");
        }
        neuralNetwork = InitializedNN(topology, neuralNetwork, fileName);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public int PredictRune(double[] input, NeuralNetwork neuralNetwork)
    {
        var neurons = neuralNetwork.Predict(input);
        int outRune = 0;
        for (int i = 0; i < neurons.Count; i++)
        {
            if (neurons[i].Output > 0.8)
            {
                outRune = i+1;
            }
        }
        return outRune;
    }
    public NeuralNetwork InitializedNN(Topology topology, NeuralNetwork neuralNetwork, string path)
    {
        neuralNetwork = new NeuralNetwork(topology);
        var weights = neuralNetwork.WeightsFromXml(path);
        neuralNetwork.InputWeightsMass(neuralNetwork, weights);
        return neuralNetwork;
    }

}
