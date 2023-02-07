using UnityEngine;

namespace Assets.Scripts
{
    internal class CreateTrainingAgents : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabTrainer;
        [SerializeField] private int _maxTrainingAgents;
        [SerializeField] private Vector3 _spacing;

        private void Awake()
        {
            Vector3 position = Vector3.zero;
            for(int i=0; i < _maxTrainingAgents; i++)
            {
                Instantiate(_prefabTrainer, position, Quaternion.identity);
                position += _spacing;
            }
        }
    }
}