using System;
using Tensorflow;

class Program
{
    static void Main()
    {
        // Hiperparâmetros
        int inputSize = 1;
        int outputSize = 2;
        int trainingSteps = 1000;
        float learningRate = 0.1f;

        // Inicialize o ambiente TensorFlow
        TFGraph graph = new TFGraph();
        TFSession session = new TFSession(graph);

        // Defina os placeholders para entrada e labels
        var input = graph.Placeholder(TFDataType.Float);
        var labels = graph.Placeholder(TFDataType.Float);

        // Parâmetros da rede neural
        var weights = graph.Variable(graph.Const(new float[,] { { 0.1f }, { -0.1f } }));
        var biases = graph.Variable(graph.Const(new float[] { 0.1f, 0.1f }));

        // Construa a rede neural
        var output = graph.Add(graph.MatMul(input, weights), biases);

        // Função de perda (erro quadrático médio)
        var loss = graph.Mean(graph.Square(graph.Sub(labels, output)));

        // Otimizador (gradiente descendente)
        var optimizer = new TFGradientDescentOptimizer(learningRate).Minimize(loss);

        // Dados de treinamento
        float[,] inputValues = { { 0 }, { 1 } };
        float[,] labelValues = { { 1, 0 }, { 0, 1 } };

        // Treinamento
        using (var sess = new TFSession(graph))
        {
            for (int step = 0; step < trainingSteps; step++)
            {
                var lossValue = sess.Run(
                    new[] { loss, optimizer },
                    new[] { input, labels },
                    new TFTensor[] { inputValues, labelValues });

                Console.WriteLine($"Step {step}, Loss: {lossValue[0]}");
            }

            Console.WriteLine("Treinamento concluído.");
        }

        // Executar previsões
        float[,] testInput = { { 0 } };
        var prediction = session.Run(output, new[] { input }, new TFTensor[] { testInput });
        Console.WriteLine("Previsão para input 0: " + prediction.GetValue<float>(0));

        testInput = new float[,] { { 1 } };
        prediction = session.Run(output, new[] { input }, new TFTensor[] { testInput });
        Console.WriteLine("Previsão para input 1: " + prediction.GetValue<float>(0));
    }
}