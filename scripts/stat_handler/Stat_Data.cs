using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace StepToStep.Scripts.StatHandler;

[GlobalClass]
public partial class Stat_Data : Resource
{
    [Serializable]
    public class TypeStatData
    {
        public string From;
        public string Do;
        public float Amount;
        public TypeAmount Type;
        
        [Serializable]
        public enum TypeAmount
        {
            Value,
            Percent
        }
    }

    [Export] public TypeStatData stats;
    // [Export] public Array<TypeStatData> stats = new Array<TypeStatData>();
}



