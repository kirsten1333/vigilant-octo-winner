using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeuralNetworks
{
    class Program
    {
        static void Main(string[] args)
        {

            //var outputsRaw = new double[,] {
            //{ 0, 1, 0 }, { 0, 1, 0.06 }, { 1, 0, 0.12 }, { 0, 1, 0.18 },
            //{ 0, 1, 0.24 }, { 0, 1, 0.3 }, { 1, 0, 0.36 }, { 0, 1, 0.42 },
            //{ 1, 0, 0.48 }, { 1, 0, 0.54 }, { 1, 0, 0.6 }, { 1, 0, 0.72 },
            //{ 1, 0, 0.78 }, { 0, 0, 0.84 }, { 1, 0, 0.9 }, { 1, 0, 0.94 } };
            //var inputsRaw = new double[,] {
            //{ 0, 0, 0, 0 }, { 0, 0, 0, 1 }, { 0, 0, 1, 0 }, { 0, 0, 1, 1 },
            //{ 0, 1, 0, 0 }, { 0, 1, 0, 1 }, { 0, 1, 1, 0 }, { 0, 1, 1, 1 },
            //{ 1, 0, 0, 0 }, { 1, 0, 0, 1 }, { 1, 0, 1, 0 }, { 1, 0, 1, 1 },
            //{ 1, 1, 0, 0 }, { 1, 1, 0, 1 }, { 1, 1, 1, 0 }, { 1, 1, 1, 1 }};

            //Program program = new Program();

            //var testinputsRaw = program.GetFromTxt("TestInputs.txt", 20);
            //var testoutputsRaw = program.GetFromTxt("TestOutputs.txt", 3);

            //var inputsRaw = program.GetFromTxt("Inputs.txt", 20);
            //var outputsRaw = program.GetFromTxt("Outputs.txt", 3);

            //var (inputs, outputs) = program.Shuffle(inputsRaw, outputsRaw);

            //var (testinputs, testoutputs) = program.Shuffle(testinputsRaw, testoutputsRaw);

            //var topology = new Topology(20, 3, 0.15, 30);
            //var neuralNetwork = new NeuralNetwork(topology);

            //Console.WriteLine("Нейронная сеть создана\n");
            //RES(outputs, inputs, neuralNetwork, results);

            //var weights = neuralNetwork.CreateWeightsMass(neuralNetwork);
            //Console.WriteLine("\nМассив первичных весов создан\n");

            //var difference = neuralNetwork.Learn(outputs, inputs, 10000);
            //Console.WriteLine("\n____________________Нейронная сеть обучена_______________________________________\n");

            //RES(outputs, inputs, neuralNetwork);
            //Console.WriteLine("\n____________________ТЕСТОВЫЙ ПРОГОН___________________________\n");
            //RES(testoutputs, testinputs, neuralNetwork);

            //var weights = neuralNetwork.CreateWeightsMass(neuralNetwork);
            //neuralNetwork.WeightsInXml(weights);
            //Console.WriteLine("\nВыгружены сторонние веса\n");

            //Console.WriteLine("\nЗагружены сторонние веса\n");
            //var weights = neuralNetwork.WeightsFromXml();
            //neuralNetwork.InputWeightsMass(neuralNetwork, weights);
            //RES(outputs, inputs, neuralNetwork);
            //Console.WriteLine("\nЗагружены старые веса\n");

            //RES(outputs, inputs, neuralNetwork, results);
        }

        public static void RES(double[,] outputs, double[,] inputs, NeuralNetwork neuralNetwork)
        {
            var results = new List<double[]>();
            for (int i = 0; i < inputs.GetLength(0); i++)
            {
                var row = NeuralNetwork.GetRow(inputs, i);
                var neurons = neuralNetwork.Predict(row).ToArray();
                double[] res = new double[neurons.Length];
                for(int neuronInd = 0; neuronInd < neurons.Length; neuronInd++)
                {
                    res[neuronInd] = neurons[neuronInd].Output;
                }

                results.Add(res);
            }

            Console.WriteLine("\nИтоговый прогон результаты:\n");

            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine("Номер теста: {0}: ", i+1);
                //\n Ожидаемое значение: {1} Выданное значение: {2}
                for (int r = 0; r < results[i].Length; r++)
                {
                    var expected = Math.Round(outputs[i, r], 2);
                    var actual = Math.Round(results[i][r], 2);
                    Console.WriteLine("Значение {0}", r+1);
                    Console.WriteLine(" Ожидаемое значение: {0} Выданное значение: {1}", expected, actual);
                }
            }
        }

        public double[,] GetFromTxt(string path, int lenght) //Рефакторинг, вынесение методов
        {
            StreamReader stringReader = new StreamReader(path);
            string s;
            int strings = 0;
            int count = File.ReadAllLines(path).Length;

            double[,] inputsM = new double[count, lenght];

            while ((s = stringReader.ReadLine()) != null)
            {
                string[] numbers = s.Split(' ');
                for (int n = 0; n < numbers.Length; n++)
                {
                    inputsM[strings, n] = Convert.ToDouble(numbers[n]);
                }
                strings++;
            }
            stringReader.Close();
            return inputsM;
        }
        
        public (double[,], double[,]) Shuffle(double[,] inputs, double[,] outputs)
        {
            var s = ConcatenationMass(inputs, outputs);

            var rand = new Random();
            for (int i = s.GetLength(0) - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);
                double tmp = 0;
                for (int k = 0; k < s.GetLength(1); k++)
                {
                    tmp = s[j,k];
                    s[j,k] = s[i,k];
                    s[i,k] = tmp;
                }

            }

            return UnConcatenationMass(s, inputs.GetLength(1));
        }

        public double[,] ConcatenationMass(double[,] arr1, double[,] arr2)
        {
           double[,] sum = new double[arr1.GetLength(0), arr1.GetLength(1) + arr2.GetLength(1)];

            for (int i = 0; i < arr1.GetLength(0); i++)
            {
                for (int k = 0; k < arr1.GetLength(1); k++)
                {
                    sum[i, k] = arr1[i, k];
                }
            }
            for (int i = 0; i < arr2.GetLength(0); i++)
            {
                for (int k = 0; k < arr2.GetLength(1); k++)
                {
                    sum[i, k + arr1.GetLength(1)] = arr2[i, k];
                }
            }
            return sum;
        }
        
        public (double[,], double[,]) UnConcatenationMass(double[,] arr, int fLength)
        {
            double[,] arr1 = new double[arr.GetLength(0), fLength];
            double[,] arr2 = new double[arr.GetLength(0), arr.GetLength(1) - fLength];
            for (int i = 0; i < arr1.GetLength(0); i++)
            {
                for (int k = 0; k < arr1.GetLength(1); k++)
                {
                    arr1[i, k] = arr[i, k];
                }
            }
            for (int i = 0; i < arr1.GetLength(0); i++)
            {
                for (int k = 0; k < arr2.GetLength(1); k++)
                {
                    arr2[i, k] = arr[i, k + fLength];
                }
            }
            return (arr1,arr2);
        }
    }
}
