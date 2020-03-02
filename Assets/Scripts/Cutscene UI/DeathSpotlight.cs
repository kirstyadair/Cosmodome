using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSpotlight : MonoBehaviour
{
    bool _isShowing = false;

    GameObject _player;
    Animator _animator;

    public void ShowSpotlight(GameObject player)
    {
        if (_isShowing) return;

        _player = player;
        _isShowing = true;
        _animator.Play("Show");
        UpdatePosition();
    }

    void UpdatePosition()
    {
        this.transform.up = this.transform.position - _player.transform.position;
    }

    public void HideSpotlight()
    {
        if (!_isShowing) return;
        _isShowing = false;
        _animator.Play("Hide");
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isShowing)
        {

            UpdatePosition();
        }
    }
}
