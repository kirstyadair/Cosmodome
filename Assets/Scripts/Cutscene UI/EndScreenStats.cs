using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EndScreenStats : MonoBehaviour
{
    [SerializeField]
    GameObject _columnPrefab;

    [SerializeField]
    GameObject _columnContainer;

    Animator _animator;


    public void Setup(List<PlayerData> playerData) {
         _animator = GetComponent<Animator>();

        playerData = playerData.OrderBy((a) => a.placed).ToList();

        foreach (PlayerData data in playerData) {
            GameObject newColumn = Instantiate(_columnPrefab, _columnContainer.transform);
            newColumn.GetComponent<EndScreenColumn>().Setup(data, null);
        }

        _animator.Play("Drop in");
    }
}
