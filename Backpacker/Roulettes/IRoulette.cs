using System;
using System.Collections.Generic;
using Backpacker.Interfaces;

namespace Backpacker.Roulettes
{
    public interface IRoulette<T> where T : IRouletteOption
    {
        IRoulette<T> SetOptions(List<T> options);

        T GetNext();

        IRoulette<T> AddWeight(T option, double amount);

        IRoulette<T> ReduseAllWeights();
    }
}