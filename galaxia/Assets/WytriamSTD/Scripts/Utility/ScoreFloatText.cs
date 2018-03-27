using UnityEngine;

public class ScoreFloatText : MonoBehaviour
{
    public TextMesh mesh;
    private void Start()
    {
        mesh = GetComponent<TextMesh>();
    }

    private void Update()
    {
        gameObject.transform.position += new Vector3(0, 0, 2) * Time.deltaTime;
    }

    public ScoreFloatText InitializeText(string text, Vector3 position, Color textColor)
    {
        transform.position = position;
        mesh.text = text;
        mesh.color = textColor;
        gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
        return this;
    }
}