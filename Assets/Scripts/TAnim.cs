using System;

public class TAnim
{

    private TTransition _transition;
    private int _duration;
    private bool _isForward;
    private bool _inProgress;
    private double _progress;
    private static readonly double InertiaCost = Math.Ceiling(Math.Pow(0.5, 1d / 3d)) / 0.5;
    
    public TAnim(TTransition trans, int dur, bool isForwards = true, bool startRunning = false)
    {
        _transition = trans;
        _duration = dur;
        _isForward = isForwards;
        _inProgress = startRunning;
        _progress = isForwards ? 0 : 1;
    }
    
    public TAnim Start()
    {
        _inProgress = true;
        return this;
    }
    public TAnim Stop()
    {
        _inProgress = false;
        return this;
    }
    public TAnim Cancel()
    {
        return Reset().Stop();
    }
    
    public TAnim Reset()
    {
        _progress = _isForward ? 0 : 1;
        return this;
    }
    
    public TAnim Reverse()
    {
        _isForward = false;
        return Start();
    }
    
    public TAnim Forward()
    {
        _isForward = true;
        return Start();
    }
    
    public TAnim SetTransition(TTransition trans)
    {
        _transition = trans;
        return this;
    }
    
    public TAnim SetDuration(int dur)
    {
        _duration = dur;
        return this;
    }
    
    public TAnim SetProgress(double progressIn)
    {
        _progress = progressIn;
        return this;
    }
    
    public bool IsRunning()
    {
        return _inProgress;
    }
    
    public double Get()
    {
        Increment(1);
        return Translate(_progress);
    }
    
    public double Get(double low, double high)
    {
        return ((high - low) * Get() + low);
    }

    private void Increment(int ticks)
    {
        if (!_inProgress) return;
        if (!_isForward) ticks *= -1;
        _progress += ticks / (double) _duration;

        if (_progress > 1)
        {
            _progress = 1;
            _inProgress = false;
        }
        else if (_progress < 0)
        {
            _progress = 0;
            _inProgress = false;
        }
    }

    private double Translate(double progress)
    {
        switch (_transition)
        {
            case TTransition.Curve:
                return progress * progress;
            case TTransition.SteepCurve:
                return Math.Pow(progress, 3);
            case TTransition.BezierCurve:
                return Math.Pow(-1 + Math.Sqrt(-progress + 1), 2);
            case TTransition.InverseCurve:
                return -Math.Pow(progress - 1, 2) + 1;
            case TTransition.InverseSteepCurve:
                return Math.Pow(progress - 1, 3) + 1;
            case TTransition.Rubber:
                double trans = -Math.Sin(10.0 * progress) / (10.0 * progress) + 1;
                return trans > 0 ? trans : 0;
            case TTransition.Inertia:
                return Math.Ceiling(Math.Pow(progress - 0.5, 1d / 3d)) / InertiaCost + 0.5;
            case TTransition.Instant:
                return Math.Round(progress);
            case TTransition.Linear:
                return progress;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
    
public enum TTransition {
    Linear,
    Curve,
    SteepCurve,
    BezierCurve,
    InverseCurve,
    InverseSteepCurve,
    Rubber,
    Inertia,
    Instant,
}