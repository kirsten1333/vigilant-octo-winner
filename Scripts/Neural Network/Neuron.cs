using System;
using System.Collections.Generic;

namespace NeuralNetworks
{
    public class Neuron
    {
        public List<double> Weights { get; } //Лист весов нейрона
        public List<double> Inputs { get; } //Лист, хранящий значения со входов нейрона
        public NeuronType NeuronType { get; } //Тип нейрона : входной, скрытый, выходной
        public double Output { get; private set; } //Выход с нейрона
        public double Delta { get; private set; }
        public double Sum { get; private set; }

        public Neuron(int inputCount, NeuronType type = NeuronType.Normal) //Конструктор нейрона
        {
            NeuronType = type; //Тип нейрона
            Weights = new List<double>(); //Лист  весов нейрона
            Inputs = new List<double>();

            InitWeightsRandomValue(inputCount);
        }

        private void InitWeightsRandomValue(int inputCount)
        {
            var rnd = new Random();

            for (int i = 0; i < inputCount; i++)
            {
                if (NeuronType == NeuronType.Input)
                {
                    Weights.Add(1);
                }
                else
                {
                    Weights.Add(rnd.NextDouble());
                }
                Inputs.Add(0);
            }
        }

        public double FeedForward(List<double> inputs)
        //FEED FORWARD сети идут слева направо, без возможности рекурентности
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                Inputs[i] = inputs[i];
            }
            //Реализовать проверку входов?
            Sum = 0.0;
            for (int i = 0; i < inputs.Count; i++)
            {
                Sum += inputs[i] * Weights[i];
            }

            if (NeuronType != NeuronType.Input)
            {
                Output = Sigmoid(Sum);
            }
            else
            {
                Output = Sum;
            }

            return Output;
        }

        private double Sigmoid(double x) //Функция сигмойды от Х (функция активации)
        {
            var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
            return result;
        }

        private double SigmoidDx(double x)
        {
            var sigmoid = Sigmoid(x);
            var result = sigmoid * (1 - sigmoid);
            return result;
        }

        public void Learn(double delta, double learningRate)
        {
            Delta = delta;
            if (NeuronType == NeuronType.Input)
            {
                return;
            }
            
            for (int i = 0; i < Weights.Count; i++)
            {
                var weight = Weights[i];
                var input = Inputs[i];

                var newWeigth = weight - learningRate * input * Delta;
                Weights[i] = newWeigth;
            }
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}
