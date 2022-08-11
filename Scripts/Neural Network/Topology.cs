using System.Collections.Generic;

namespace NeuralNetworks
{
    public class Topology //Определяет топологию нейронной сети
    {
        public int InputCount { get; } //Количество входов на неёронную сеть
        public int OutputCount { get; } //Количество выходов нейронной сети
        public double LearningRate { get; } //Скорость обучения сети, её точность
        public List<int> HiddenLayers { get; } //количество скрытых слоёв с количеством их нейронов
                                               //(У каждого слоя может быть разное количество нейронов)

        public Topology(int inputCount, int outputCount, double learningRate, params int[] layers)
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            LearningRate = learningRate;
            HiddenLayers = new List<int>();
            HiddenLayers.AddRange(layers);
        }

    }
}
