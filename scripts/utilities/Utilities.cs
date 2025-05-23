﻿using Godot;
using System;
using System.Text.RegularExpressions;

namespace StepToStep.Utils;

public static class Utilities
{
    public static Tween CreateBounceRepeatTween(this Node obj,
                                                Variant startValue,
                                                Variant endValue,
                                                string property,
                                                float duration = 1,
                                                int repeated = 1,
                                                Tween.TransitionType transitionType = Tween.TransitionType.Linear)
    {
        repeated--;
        Tween self = CreateToTween(obj, startValue, endValue, property, duration, transitionType);
        if(repeated == 0)
            return self;

        self.Finished += OnTweenFinished;
        return self;

        void OnTweenFinished()
        {
            self.Finished -= OnTweenFinished;
            obj.CreateBounceRepeatTween(endValue, startValue, property, duration, repeated,
                                        transitionType);
        }
    }

    public static Tween CreateToTween(this Node obj,
                                      Variant startValue,
                                      Variant endValue,
                                      string property,
                                      float duration = 1,
                                      Tween.TransitionType transitionType = Tween.TransitionType.Linear)
    {
        Tween self = obj.CreateTween();

        self.TweenProperty(obj, property, endValue, duration).From(startValue).SetTrans(transitionType);

        return self;
    }

    public static TEnum ParseToEnum<TEnum>(this string valueName) where TEnum : struct, Enum
    {
        if(!Enum.TryParse(valueName, true, out TEnum state))
            throw (RegexParseException)new Exception("Invalid state name");
        return state;
    }

    public static T FindNode<T>(this Node self, bool includeInternal = false) where T : Node
    {
        T result = null;

        foreach(Node child in self.GetChildren(includeInternal)){
            if(typeof(T) != child.GetType())
                continue;
            result = child as T;
        }

        return result;
    }

    public static string GetSaveKey(this Node self, string additional = "")
    {
        string result = $"{self.GetTreeString()}";
        if(string.IsNullOrEmpty(additional))
            result = $"{result}/{additional}";
        return result;
    }
}