using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNotificationPooler
{
    public QuestNotificationPooler(QuestNotification _accepted, QuestNotification _completed, 
        int _initAmount, Transform _parent)
    {
        accepted = _accepted;
        completed = _completed;
        parent = _parent;
        CreatePool(_initAmount);
    }

    QuestNotification accepted;
    QuestNotification completed;

    protected List<QuestNotification> acceptedPool;
    protected List<QuestNotification> completedPool;
    protected Transform parent;

    public void CreatePool(int initAmount)
    {
        acceptedPool = new List<QuestNotification>();
        completedPool = new List<QuestNotification>();

        for (int i = 0; i < initAmount; i++)
        {
            acceptedPool.Add(CreateObj(accepted));
            completedPool.Add(CreateObj(completed));
        }
    }

    public QuestNotification CreateObj(QuestNotification prefab)
    {
        var obj = Object.Instantiate(prefab);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        obj.name = prefab.name;
        return obj;
    }
    public QuestNotification GetCompletedNotification()
    {
        for (int i = 0; i < completedPool.Count; i++)
        {
            if (!completedPool[i].IsOn())
            {
                return completedPool[i];
            }
        }
        var obj = CreateObj(completed);
        completedPool.Add(obj);
        return obj;
    }
    public QuestNotification GetAcceptedNotification()
    {
        for (int i = 0; i < acceptedPool.Count; i++)
        {
            if (!acceptedPool[i].IsOn())
            {
                return acceptedPool[i];
            }
        }

        var obj = CreateObj(accepted);
        acceptedPool.Add(obj);
        return obj;
    }

}
