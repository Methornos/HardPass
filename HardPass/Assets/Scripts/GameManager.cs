using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text[] _objectsText = new Text[4];
    [SerializeField]
    private Transform _iconsContainer;
    [SerializeField]
    private GameObject _iconPrefab;
    [SerializeField]
    private Transform _spawnPosition;
    [SerializeField]
    private Material _standartMaterial;

    private int[] _objectsCounts = new int[4];

    private bool _isStarted = false;

    private ObjectsPool objectsPool;
    private GameObject _poolItem;

    public List<ContainerObjectDetails> ObjectsExamples;
    public List<GameObject> ObjectsQueue;
    public List<int> ObjectsId;

    private void Start()
    {
        objectsPool = GameObject.FindGameObjectWithTag("ObjectsPool").GetComponent<ObjectsPool>();
    }

    public void AddToQueue(int id)
    {
        if (_iconsContainer.childCount < 10)
        {
            _poolItem = objectsPool.GetPoolItem();

            _objectsCounts[id]++;
            _objectsText[id].text = "x" + _objectsCounts[id].ToString();

            ObjectsQueue.Add(_poolItem);
            ObjectsId.Add(id);

            if (!_isStarted) StartCoroutine(Spawner(id));

            GameObject newIcon = Instantiate(_iconPrefab, _iconsContainer);
            newIcon.GetComponent<Image>().sprite = ObjectsExamples[id].Preview;
        }
    }

    private IEnumerator Spawner(int id)
    {
        _isStarted = true;

        ObjectsQueue[0].transform.position = _spawnPosition.position;
        ObjectsQueue[0].GetComponent<MeshRenderer>().material = _standartMaterial;
        ObjectsQueue[0].GetComponent<MeshFilter>().mesh = ObjectsExamples[ObjectsId[0]].Mesh;
        ObjectsQueue[0].GetComponent<Rigidbody>().useGravity = false;
        ObjectsQueue[0].GetComponent<Rigidbody>().velocity = new Vector3Int(3, 0, 0);

        StartCoroutine(DestroyObject(ObjectsQueue[0], ObjectsId[0]));
        ObjectsQueue.RemoveAt(0);
        ObjectsId.RemoveAt(0);
        yield return new WaitForSeconds(1f);
        _isStarted = false;
        if (ObjectsQueue.Count != 0) StartCoroutine(Spawner(id));
    }

    private IEnumerator DestroyObject(GameObject obj, int id)
    {
        yield return new WaitForSeconds(3f);

        _objectsCounts[id]--;
        _objectsText[id].text = "x" + _objectsCounts[id].ToString();

        objectsPool.ReturnToPool(obj);
        Destroy(_iconsContainer.GetChild(0).gameObject);
    }
}
