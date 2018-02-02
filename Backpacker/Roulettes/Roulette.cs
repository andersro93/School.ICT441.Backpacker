﻿using System;
using System.Collections.Generic;
using System.Linq;
using Backpacker.Interfaces;

namespace Backpacker.Roulettes
{
    public class Roulette<T> : IRoulette<T> where T : IRouletteOption
    {
        protected double _defaultWeight = 100.0;
        
        protected double _globalReductionFactor = 0.99;
        
        protected Dictionary<T, double> Options = new Dictionary<T, double>();
        
        protected Random Random = new Random();

        public IRoulette<T> SetOptions(List<T> options)
        {
            Options = new Dictionary<T, double>();
            
            options.ForEach(x =>
            {
                Options.Add(x, _defaultWeight);
            });

            return this;
        }

        public T GetNext()
        {
            double sumOfWieghts = Options.Sum(entry => entry.Value);

            int rouletteNumber = Random.Next(0, (int) sumOfWieghts);

            T selected = Options.First().Key;

            foreach (KeyValuePair<T, double> keyValuePair in Options)
            {
                rouletteNumber -= (int) keyValuePair.Value;

                if (rouletteNumber <= 0)
                {
                    selected = keyValuePair.Key;
                    break;
                }
                
            }

            if (selected == null)
            {
                Console.WriteLine("test");
            }

            return selected;
        }

        public IRoulette<T> AddWeight(T option, double amount)
        {
            Options[option] += amount;

            return this;
        }

        public IRoulette<T> ReduseAllWeights()
        {
            foreach (KeyValuePair<T,double> keyValuePair in Options)
            {
                Options[keyValuePair.Key] *= _globalReductionFactor;
            }

            return this;
        }
    }
}