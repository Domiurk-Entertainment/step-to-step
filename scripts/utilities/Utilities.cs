using Godot;

namespace StepToStep.scripts;

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
}