using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    class Program
    {
        static Random random = new Random();

        // Генерация случайной особи (популяции)
        static List<int> GenerateRandomIndividual(int length)
        {
            return Enumerable.Range(0, length)
                .Select(_ => random.Next(2)) // 0 или 1
                .ToList();
        }

        // Оценка приспособленности особи (функция приспособленности)
        static double EvaluateFitness(List<int> individual)
        {
            // Ваша функция приспособленности (например, минимизация функции)
            // Здесь можно реализовать любую задачу оптимизации

            // Пример: сумма значений особи
            return individual.Sum();
        }

        // Скрещивание двух особей
        static List<int> Crossover(List<int> parent1, List<int> parent2)
        {
            int crossoverPoint = random.Next(parent1.Count);
            return parent1.Take(crossoverPoint)
                .Concat(parent2.Skip(crossoverPoint))
                .ToList();
        }

        // Мутация особи
        static void Mutate(List<int> individual, double mutationRate)
        {
            for (int i = 0; i < individual.Count; i++)
            {
                if (random.NextDouble() < mutationRate)
                    individual[i] = 1 - individual[i]; // Инвертирование бита
            }
        }

        static void Main(string[] args)
        {
            int populationSize = 100;
            int individualLength = 10;
            double mutationRate = 0.01;
            int generations = 100;


            List<List<int>> population = Enumerable.Range(0, populationSize)
                .Select(_ => GenerateRandomIndividual(individualLength))
                .ToList();

            for (int gen = 0; gen < generations; gen++)
            {
                var fitnessScores = population.Select(individual => EvaluateFitness(individual)).ToList();
                var bestIndividual = population[fitnessScores.IndexOf(fitnessScores.Max())];

                Console.WriteLine($"Поколение {gen + 1}: Лучшая приспособленность = {EvaluateFitness(bestIndividual)}");

                var newPopulation = new List<List<int>>();
                while (newPopulation.Count < populationSize)
                {
                    var parent1 = population[rouletteWheelSelection(fitnessScores)];
                    var parent2 = population[rouletteWheelSelection(fitnessScores)];
                    var child = Crossover(parent1, parent2);
                    Mutate(child, mutationRate);
                    newPopulation.Add(child);
                }

                population = newPopulation;
            }
        }

        // Выбор родителя с использованием рулеточного метода
        static int rouletteWheelSelection(List<double> fitnessScores)
        {
            double totalFitness = fitnessScores.Sum();
            double randomValue = random.NextDouble() * totalFitness;
            double sum = 0;

            for (int i = 0; i < fitnessScores.Count; i++)
            {
                sum += fitnessScores[i];
                if (sum >= randomValue)
                    return i;
            }

            return fitnessScores.Count - 1;
        }

    }
}
