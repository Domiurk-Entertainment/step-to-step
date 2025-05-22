using Godot;
using StepToStep.Entity;
using StepToStep.scripts.entity;
using StepToStep.Utils;
using System;
using System.Collections.Generic;

namespace StepToStep.Demo;

public partial class EnemyBattleDemo : Node
{
    private const float COOLDOWN_ATTACKS = 5;
    private const float CHANCE_TURN_PLAYER = 0.1f;

    [Signal] public delegate void NextTurnEnemyEventHandler();
    [Signal] public delegate void NextTurnPlayerEventHandler();

    [Export] public EnemyDemo Enemy;
    [Export] public Player Player;
    private readonly Queue<IAttack> _turns = new();
    private IAttack _currentTurn;

    public override void _Ready()
    {
        base._Ready();
        Player.AttackedStep += PlayerOnAttackedStep;

        Enemy.AttackBase.InitialTarget(Enemy, Player);
        Enemy.AttackBase.Connect(AttackBase.SignalName.ChangeAttackStep, Callable.From<string>(EnemyChangeAttackState));

        _turns.Enqueue(Player);
        _turns.Enqueue(Player);
        _turns.Enqueue(Player);
        _turns.Enqueue(Player);
        _turns.Enqueue(Player);

        EmitSignal(CheckNextTurn<Player>() ? SignalName.NextTurnPlayer : SignalName.NextTurnEnemy);
    }
    private void PlayerOnAttackedStep(AttackType attack)
    {
        if(Enemy == null)
            return;

        switch(attack){
            case AttackType.Start:
                AddRandomTurn();
                break;
            case AttackType.Attacked:
                break;
            case AttackType.End:
                _currentTurn = null;
                EndTurn();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attack), attack, null);
        }
    }

    private void EnemyChangeAttackState(string stateName)
    {
        switch(stateName.ParseToEnum<AttackType>()){
            case AttackType.End:
                _currentTurn = null;

                EndTurn();
                break;
            case AttackType.Start:
                AddRandomTurn();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void EndTurn()
    {

        if(CheckNextTurn<EnemyBase>(force: true)){
            EmitSignal(SignalName.NextTurnEnemy);
            StartEnemyAttack();
        }
        else{
            EmitSignal(SignalName.NextTurnPlayer);
        }
    }

    private void AddRandomTurn()
    {
        _turns.Enqueue(GD.Randf() > CHANCE_TURN_PLAYER ? Player : Enemy);
    }

    public void StartAttack()
    {
        if(_currentTurn == null)
            GD.Print($"{nameof(EnemyBattleDemo)}: Current turn is null");
        _currentTurn?.Attack();
    }

    public void StartEnemyAttack()
        => GetTree()
           .CreateTimer(COOLDOWN_ATTACKS)
           .Connect(SceneTreeTimer.SignalName.Timeout, Callable.From(StartAttack));

    private bool CheckNextTurn<TAttack>(bool force = false) where TAttack : IAttack
    {
        bool result = _turns.Peek() is TAttack;
        if(result || force)
            _currentTurn = _turns.Dequeue();
        return result;
    }
}