using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ListCreator : MonoBehaviour
{
    public Button ResetBtn;
    public Button BubbleSortBtn;
    public Button SelectionSortBtn;

    public GameObject cam;
    public GameObject cube;
    public int sizeOfList = 100;

    private List<GameObject> list = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        cam.transform.position = new Vector3(sizeOfList / 2, 0, sizeOfList * -1); // TODO: make the z just fit the whole list.

        SetUpUnorderedList();

        ResetBtn.onClick.AddListener(SetUpUnorderedList);
        BubbleSortBtn.onClick.AddListener(BubbleOnClick);
        SelectionSortBtn.onClick.AddListener(SelectionOnClick);
    }

    void BubbleOnClick()
    {
        StartCoroutine(BubbleSort());
    }

    void SelectionOnClick()
    {
        StartCoroutine(SelectionSort());
    }

    public IEnumerator SelectionSort()
    {
        int timesThrough = 0;
        var blah = sizeOfList;
        var currentMin = list[0];
        bool found = false;
        while (blah > 0)
        {
            currentMin.gameObject.GetComponent<Renderer>().material.color = new Color(
                currentMin.gameObject.transform.localScale.y / sizeOfList,
                (sizeOfList - currentMin.gameObject.transform.localScale.y) / sizeOfList,
                0);
            currentMin = list[timesThrough];

            for (int i = timesThrough; i <= sizeOfList+1; i++)
            {
                try
                {
                    if (list[i].gameObject.transform.localScale.y < currentMin.gameObject.transform.localScale.y)
                    {
                        found = true;
                        Debug.Log($"Cube {i} with scale {list[i].gameObject.transform.localScale.y} is smaller than cube {i + 1} with scale {list[i + 1].gameObject.transform.localScale.y}");
                        currentMin = list[i];
                    }
                }
                catch(System.ArgumentOutOfRangeException ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            Debug.Log($"The smallest cube in round {timesThrough} had scale {currentMin.gameObject.transform.localScale.y} and is at horizontal pos: {currentMin.gameObject.transform.position.x}");

            currentMin.gameObject.GetComponent<Renderer>().material.color = Color.yellow;


            if (found)
            {
                list.Remove(currentMin);
                list.Insert(timesThrough, currentMin);

                currentMin.transform.position = new Vector3(
                    timesThrough,
                    currentMin.gameObject.transform.position.y,
                    currentMin.gameObject.transform.position.z);

                for (int i = timesThrough + 1; i < sizeOfList; i++)
                {
                    list[i].gameObject.transform.position = new Vector3(
                        list[i].gameObject.transform.position.x+1,
                        list[i].gameObject.transform.position.y,
                        list[i].gameObject.transform.position.z);

                }
            }
            found = false;
            timesThrough++;
            blah--;
            yield return null;
        }

        Debug.Log($"The finished list: {list.ToString()}");
    }

    public IEnumerator BubbleSort()
    {
        var blah = sizeOfList;
        while (blah > 0)
        {
            for (int i = 0; i < blah; i++)
            {
                try
                {
                    if (list[i].transform.localScale.y <= list[i + 1].transform.localScale.y)
                    {
                        Debug.Log($"Cube {i} with scale {list[i].gameObject.transform.localScale.y} and cube {i + 1} with scale {list[i + 1].gameObject.transform.localScale.y} were already in order or were equal");
                    }
                    else
                    {
                        Debug.Log($"Cube {i} with scale {list[i].gameObject.transform.localScale.y} and cube {i + 1} with scale {list[i + 1].gameObject.transform.localScale.y} were not in order");

                        var bigger = list[i].transform.localScale.y;
                        var smaller = list[i + 1].transform.localScale.y;

                        list[i].gameObject.transform.localScale = new Vector3(1, smaller, 1);
                        list[i + 1].gameObject.transform.localScale = new Vector3(1, bigger, 1);

                        list[i].gameObject.GetComponent<Renderer>().material.color = new Color(
                            list[i].gameObject.transform.localScale.y / sizeOfList,
                            (sizeOfList - list[i].gameObject.transform.localScale.y) / sizeOfList,
                            0);
                        list[i+1].gameObject.GetComponent<Renderer>().material.color = new Color(
                            list[i+1].gameObject.transform.localScale.y / sizeOfList,
                            (sizeOfList - list[i+1].gameObject.transform.localScale.y) / sizeOfList,
                            0);

                        Debug.Log($"Cube {i} with scale {list[i].gameObject.transform.localScale.y} and cube {i + 1} with scale {list[i + 1].gameObject.transform.localScale.y} are now in order");


                    }
                }
                catch(System.ArgumentOutOfRangeException ex)
                {
                    Debug.Log("end of list: ignore");
                }
            }
            blah--;

            yield return null;
        }
    }

    private void SetUpUnorderedList()
    {
        StopAllCoroutines();
        RemovePreviousListAndCubes();
        int horizontalPosition = 0;
        for (int i = 0; i < sizeOfList; i++)
        {
            var c = Instantiate(cube, new Vector3(horizontalPosition, 0, 0), Quaternion.identity);

            c.transform.localScale = new Vector3(1, Random.Range(1, sizeOfList), 0.1f);
            list.Add(c);
            horizontalPosition++;
            Debug.Log($"Created cube and x={horizontalPosition}");
        }

        for (int i = 0; i < sizeOfList; i++)
        {
            list[i].gameObject.GetComponent<Renderer>().material.color = new Color(
                list[i].gameObject.transform.localScale.y / sizeOfList,
                (sizeOfList - list[i].gameObject.transform.localScale.y) / sizeOfList,
                0);
        }

        Debug.Log($"The size of the list is {list.Count}");
    }

    private void RemovePreviousListAndCubes()
    {
        foreach (var c in list)
        {
            Destroy(c.gameObject);
        }

        list.RemoveAll(x => x != null);


    }
}
