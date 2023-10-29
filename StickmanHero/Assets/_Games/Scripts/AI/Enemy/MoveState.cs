using SuperFight;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    SpiderHanging spider;
    float moveTime;
    Tween sequence;
    public MoveState(SpiderHanging controller, string stateName) : base(controller, stateName)
    {
        spider = controller;
        moveTime = spider.moveTime;
    }

    private Vector3 firstPos;
    private Vector3 endPos;


    void MoveParrtern1()
    {
        sequence = controller.transform.DOMove(firstPos, moveTime / 3).SetDelay(0.5f).OnComplete(() =>
        {
            MoveParrtern2();
        });
    }

    void MoveParrtern2()
    {
        sequence = controller.transform.DOMove(endPos, moveTime).SetDelay(1.5f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            MoveParrtern1();
        });
        controller.animatorHandle.PlayAnimation("Spider_Attack_Shot", 0.1f, 1, true);
    }

    public override void EnterState()
    {
        firstPos = controller.transform.position;
        endPos = controller.transform.position - new Vector3(0, spider.moveRange, 0);
        spider.silk.positionCount = 0;
        spider.silk.positionCount = 2;
        spider.silk.SetPosition(0, firstPos);
        sequence = controller.transform.DOMove(endPos, moveTime).SetDelay(spider.firstDelay).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            MoveParrtern1();
        });
    }

    public override void ExitState()
    {
        sequence.Kill(false);
    }

    public override void UpdateLogic()
    {
        spider.silk.SetPosition(1, controller.transform.position);
    }

    public override void UpdatePhysic()
    {

    }
}
