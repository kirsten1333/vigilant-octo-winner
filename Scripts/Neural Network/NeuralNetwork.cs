﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using UnityEngine;


namespace NeuralNetworks
{
    public class NeuralNetwork
    {
        public Topology Topology { get; }
        public List<Layer> Layers { get; }

        public NeuralNetwork(Topology topology)
        {
            Topology = topology;

            Layers = new List<Layer>();

            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }

        public List<Neuron> Predict(params double[] inputSignals)
        {
            //Реализовать проверку, что входящие данные равны количествву входящих нейронов
            SendSignalsToInputNeurons(inputSignals);
            FeedForwardAllLayersAfterInput();
            
            return Layers.Last().Neurons;

        }

        public double Learn(double[,] expected, double[,] inputs, int epoch)
        {
            var error = 0.0;
            for (int i = 0; i < epoch; i++)
            {
                Console.WriteLine("Эпоха: {0} ", i);
                for (int j = 0; j < expected.GetLength(0); j++)
                {
                    var output = GetRow(expected,j);
                    var input = GetRow(inputs, j);

                    //Console.Write("Прогон: {0}", j);
                    //Console.WriteLine();
                    error += Backpropagation(output, input);
                }
            }

            var result = error / epoch;
            return result;
        }

        public static double[] GetRow(double[,] matrix, int row)
        {
            var columns = matrix.GetLength(1);
            var array = new double[columns];
            for (int i = 0; i < columns; ++i)
                array[i] = matrix[row, i];
            return array;
        }

        private double Backpropagation(double[] expected, params double[] inputs)
        {
            var actual = Predict(inputs).ToArray();

            //Console.WriteLine("Ошибка до обратного: " + (AverageQuadError(expected, inputs)).ToString());
            
            for (int i = 0; i < Layers.Last().Neurons.Count; i++) //Проход по последнему слою
            {
                var delta = -(expected[i] - actual[i].Output) * (1 - actual[i].Output) * actual[i].Output;
                Layers.Last().Neurons[i].Learn(delta, Topology.LearningRate);
            }

            for (int j = Layers.Count - 2; j >= 0; j--) //Проход по всем последующим слоям
            {
                var layer = Layers[j];
                var previousLayer = Layers[j + 1];

                for (int i = 0; i < layer.NeuronCount; i++)
                {
                    var neuron = layer.Neurons[i];
                    double delta = 0; 
                    for (int k = 0; k < previousLayer.NeuronCount; k++)
                    {
                        var previousNeuron = previousLayer.Neurons[k];
                        delta += previousNeuron.Weights[i] * previousNeuron.Delta;

                    }
                    delta = delta * neuron.Output * (1 - neuron.Output);
                    neuron.Learn(delta, Topology.LearningRate);

                }
            }
            
            var result = AverageQuadError(expected, inputs);
            //Console.WriteLine("Ошибка после обратного: " + (result).ToString());
            return result;
        }
        private double AverageQuadError(double[] expected, params double[] inputs)
        {
            var actual = Predict(inputs).ToArray();
            double errorTotal = 0;
            for (int i = 0; i < Layers.Last().Neurons.Count; i++)
            {
                var err = 0.5 * Math.Pow((expected[i] - actual[i].Output), 2);
                errorTotal += err;
            }
            return errorTotal;
        }

        private void FeedForwardAllLayersAfterInput()//Для каждого скрытого слоя в каждый нейрон
                                                     //загружаем данные со всех выходов предыдущих нейронов
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var layer = Layers[i];//В слою I начиная со второго слоя (i = 1, а не нулю)
                var previousLayerSingals = Layers[i - 1].GetSignals();//Получаем лист выходов с нейроно

                foreach (var neuron in layer.Neurons)
                {
                    neuron.FeedForward(previousLayerSingals);
                    //забиваем в нейрон выходы с предыдущего слоя (полносвязная НС)
                }
            }
        }

        private void SendSignalsToInputNeurons(params double[] inputSignals)
        //Загрузка данных в входящие нейроны
        {
            for (int i = 0; i < inputSignals.Length; i++)
            {
                var signal = new List<double>() { inputSignals[i] };//Нейроны принимают лист, поэтому создаём лист
                var neuron = Layers[0].Neurons[i];                  //из одного элемента (входящего сигнала на конкретный нейрон)

                neuron.FeedForward(signal);//Прогоняем сигнал по нейрону 
            }
        }

        private void CreateOutputLayer()
        {
            var outputNeurons = new List<Neuron>();
            var lastLayer = Layers.Last();
            for (int i = 0; i < Topology.OutputCount; i++)
            {
                var neuron = new Neuron(lastLayer.NeuronCount, NeuronType.Output);
                outputNeurons.Add(neuron);
            }
            var outputLayer = new Layer(outputNeurons, NeuronType.Output);
            Layers.Add(outputLayer);
        }

        private void CreateHiddenLayers()
        {
            for (int j = 0; j < Topology.HiddenLayers.Count; j++)
            {
                var hiddenNeurons = new List<Neuron>();
                var lastLayer = Layers.Last();
                for (int i = 0; i < Topology.HiddenLayers[j]; i++)
                {
                    var neuron = new Neuron(lastLayer.NeuronCount);
                    hiddenNeurons.Add(neuron);
                }
                var hiddenLayer = new Layer(hiddenNeurons);
                Layers.Add(hiddenLayer);
            }
        }

        private void CreateInputLayer()
        {
            var inputNeurons = new List<Neuron>();
            for (int i = 0; i < Topology.InputCount; i++)
            {
                var neuron = new Neuron(1, NeuronType.Input);
                inputNeurons.Add(neuron);
            }
            var inputLayer = new Layer(inputNeurons, NeuronType.Input);
            Layers.Add(inputLayer);
        }
        public double[][][] CreateWeightsMass(NeuralNetwork NN)
        {
            double[][][] weightNN = new double[NN.Layers.Count][][];
            for (int LC = 0; LC < NN.Layers.Count; LC++)
            {
                var currentLayer = NN.Layers[LC];
                var weightLayer = new double[currentLayer.NeuronCount][];
                for (int NC = 0; NC < currentLayer.NeuronCount; NC++)
                {
                    var currentNeuron = currentLayer.Neurons[NC];
                    var weightNeuron = currentNeuron.Weights.ToArray();
                    weightLayer[NC] = weightNeuron;
                }
                weightNN[LC] = weightLayer;
            }
            return weightNN;
        }

        public class Weight
        {
            public double[][][] weight;
        }
        public void WeightsInXml(double[][][] weights)
        {
            Weight weight = new Weight();
            weight.weight = weights;
            XmlSerializer writer = new XmlSerializer(typeof(Weight));

            var path = "Weights.xml";
            FileStream file = File.Create(path);

            writer.Serialize(file, weight);
            file.Close();
        }
        public double[][][] WeightsFromXml(string path)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("ANDROID CHECKED ---------------------------------------");
                XmlSerializer reader = new XmlSerializer(typeof(Weight));

                //Debug.Log("XML ---------------------------------------");
                //XmlReader xml = XmlReader.Create(Application.streamingAssetsPath + "/Weights");

                //if (xml == null)
                //{
                //    Debug.Log("xml cannot load-------------------------------");
                //}
                //else
                //{
                //    Debug.Log("xml LOAD---------------------------------------");
                //    Debug.Log(xml);
                //}
                //xml.Close();
                Debug.Log("Streamreader ---------------------------------------");
                StreamReader file = new StreamReader(path);

                Weight weight = (Weight)reader.Deserialize(file);
                
                file.Close();
                return weight.weight;
            }
            else 
            {
                Debug.Log("PC CHECKED ---------------------------------------");
                var fileName =  "Weights.xml";
                XmlSerializer reader = new XmlSerializer(typeof(Weight));
                StreamReader file = new StreamReader(fileName);
                Weight weight = (Weight)reader.Deserialize(file);
                file.Close();
                return weight.weight;
            }
        }
        public void InputWeightsMass(NeuralNetwork NN, double[][][] weights)
        {
            for (int LC = 0; LC < weights.GetLength(0); LC++)
            {
                for (int NC = 0; NC < weights[LC].GetLength(0); NC++)
                {
                    for (int WC = 0; WC < weights[LC][NC].GetLength(0); WC++)
                    {
                        double weight = weights[LC][NC][WC];
                        NN.Layers[LC].Neurons[NC].Weights[WC] = weight;
                    }
                }
            }
        }
        public void WriteWeights(double[][][] weights)
        {
            for (int LC = 0; LC < weights.GetLength(0); LC++)
            {
                for (int NC = 0; NC < weights[LC].GetLength(0); NC++)
                {
                    for (int WC = 0; WC < weights[LC][NC].GetLength(0); WC++)
                    {
                        double weight = weights[LC][NC][WC];
                        Console.WriteLine("Слой {0} Нейрон {1} Вес{2} = {3}", LC, NC, WC, weight);
                    }
                }
            }
        }
    }
}
