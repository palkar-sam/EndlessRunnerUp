using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private List<GameObject> lifes;

    public int RemainingLife => _totalLife;

    private int _totalLife;

    private void Start()
    {
        _totalLife = lifes.Count - 1;
    }

    public void DamageLife(out int playerLife)
    {
        lifes[_totalLife].SetActive(false);
        _totalLife--;

        playerLife = _totalLife;
        if (_totalLife <= 0)
            _totalLife = 0;
    }

    public void ResetLife()
    {
        _totalLife = lifes.Count - 1;
        for (int i=0; i<lifes.Count; i++)
        {
            lifes[i].SetActive(true);
        }
    }
}
