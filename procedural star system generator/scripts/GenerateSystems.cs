using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateSystem : MonoBehaviour
{
    private string[] terPlanetNames = new string[15];
    private string[] gasPlanetNames = new string[15];

    public Material[] planetMaterials;
    public Texture2D[] planetTextures;
    public Material[] starMaterials;
    public Mesh sphereMesh;
    public Material starMaterial;
    public GameObject planetShadow;
    public InputField input;
    public Flare Starflare;
    private GameObject newstar;

    public GameObject button;
    public GameObject canvas;

    public string system;

    private int starRadius;
    private int starMass;
    private float lastPlanetDistance;

    void Start()
    {
        terPlanetNames = NameLists.tPlanetNames;
        gasPlanetNames = NameLists.gPlanetNames;

        CreateSystem();
    }
    
    public void CreateSystem()
    {
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("Planet"); //removes last system
        foreach (GameObject planet in toDestroy)
            Destroy(planet);

        system = input.text;

        Random.InitState(system.GetHashCode());

        CreateStar();

        int planetNum = Random.Range(1, 6);

        for (int i = 0; i < planetNum; i++)
        {
            lastPlanetDistance += Random.Range(600, 5000);

            GenerateTexture(i, false);

            CreatePlanet(i, false);
        }

        int p = planetNum;

        planetNum += Random.Range(0, 5);

        lastPlanetDistance += 10000;

        for (int i = p; i < planetNum; i++)
        {
            lastPlanetDistance += Random.Range(5000, 15000);

            GenerateTexture(i, true);

            CreatePlanet(i, true);
        }

        GenerateAsteroidBelt();
    }

    void CreateStar()
    {
        newstar = new GameObject(system.ToString());
        newstar.tag = "Planet";
        starRadius = Random.Range(2500, 5000);
        lastPlanetDistance = starRadius + Random.Range(100, 500);
        starMass = starRadius * Random.Range(10, 100); //(4 / 3) * Mathf.PI * Mathf.Pow(starRadius, 3) * Random.Range();

        newstar.AddComponent<Light>().range = 150000;
        newstar.GetComponent<Light>().type = LightType.Point;
        newstar.GetComponent<Light>().flare = Starflare;

        newstar.AddComponent<MeshFilter>().mesh = sphereMesh;
        newstar.AddComponent<MeshRenderer>().material = starMaterials[Random.Range(0, starMaterials.Length)];

        newstar.AddComponent<Rigidbody>().useGravity = false;
        newstar.GetComponent<Rigidbody>().mass = starMass;

        newstar.transform.position = new Vector3(0, 0, 0);
        newstar.transform.localScale = new Vector3(starRadius * 2, starRadius * 2, starRadius * 2);
    }

    void CreatePlanet(int planet, bool isOuterPlanet)
    {
        string name = terPlanetNames[Random.Range(0, terPlanetNames.Length)];

        if (isOuterPlanet) name = gasPlanetNames[Random.Range(0, gasPlanetNames.Length)];

        GameObject newPlanet = new GameObject(system.ToString() + " - " + planet.ToString() + " " + terPlanetNames[Random.Range(0, terPlanetNames.Length)]); //1-2 Planet
        newPlanet.tag = "Planet";
        float radius = Random.Range(10, 150);
        if (isOuterPlanet) radius = Random.Range(300, 500);
        float mass = (4 / 3) * Mathf.PI * Mathf.Pow(radius, 3) * Random.Range(500, 3000);

        newPlanet.AddComponent<MeshFilter>().mesh = sphereMesh;
        newPlanet.AddComponent<MeshRenderer>().material = planetMaterials[planet];
        newPlanet.AddComponent<Rigidbody>().mass = mass;
        
        newPlanet.AddComponent<PlanetScript>().altitude = lastPlanetDistance;
        newPlanet.GetComponent<PlanetScript>().OrbitBody = newstar;
        newPlanet.GetComponent<PlanetScript>().rotation = new Vector3(Random.Range(-.15f, .15f), Random.Range(-1, 1), Random.Range(-.15f, .15f));
        newPlanet.GetComponent<PlanetScript>().SystemName = system;

        newPlanet.transform.position = new Vector3(0, 0, lastPlanetDistance);
        newPlanet.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);

        GameObject ps = Instantiate(planetShadow, newPlanet.transform);

        ps.transform.localScale = newPlanet.transform.lossyScale;

        int moonNum = Random.Range(0, 3);

        float lastMoonDistance = newPlanet.transform.lossyScale.x;

        for (int i = 0; i < moonNum; i++)
        {
            lastMoonDistance += Random.Range(newPlanet.transform.lossyScale.x * .5f, newPlanet.transform.lossyScale.x);

            //GenerateTexture(i, false);

            GenerateMoons(newPlanet, i, lastMoonDistance);
        }

        GameObject newButton = Instantiate(button, canvas.transform);
        newButton.GetComponentInChildren<Text>().text = newPlanet.name;
        newButton.transform.position = Vector3.zero;
        newButton.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        newButton.GetComponent<RectTransform>().position = new Vector3(2000 - (planet * 65), 0, 0);
    }

    void GenerateMoons(GameObject mainPlanet, int moonNum, float distance)
    {
        GameObject newMoon = new GameObject(mainPlanet.name.ToString() + " " + moonNum.ToString() + " " + terPlanetNames[Random.Range(0, terPlanetNames.Length)]);
        newMoon.tag = "Planet";
        float radius = Random.Range(mainPlanet.transform.lossyScale.x * .005f, mainPlanet.transform.lossyScale.x * .05f);
        float mass = (4 / 3) * Mathf.PI * Mathf.Pow(radius, 3) * Random.Range(500, 3000);

        newMoon.AddComponent<MeshFilter>().mesh = sphereMesh;
        newMoon.AddComponent<MeshRenderer>().material = starMaterial;
        newMoon.AddComponent<Rigidbody>().mass = mass;
        
        newMoon.AddComponent<PlanetScript>().altitude = distance;
        newMoon.GetComponent<PlanetScript>().OrbitBody = mainPlanet;
        newMoon.GetComponent<PlanetScript>().rotation = new Vector3(Random.Range(-.15f, .15f), Random.Range(-1, 1), Random.Range(-.15f, .15f));
        newMoon.GetComponent<PlanetScript>().SystemName = system;

        newMoon.transform.position = new Vector3(0, 0, lastPlanetDistance);
        newMoon.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);

        GameObject ps = Instantiate(planetShadow, newMoon.transform);

        ps.transform.localScale = newMoon.transform.lossyScale;
    }

    void GenerateTexture(int material, bool isOuterPlanet)
    {
        float scale = 3;

        if (isOuterPlanet) scale = 10;

        float xOrg = Random.Range(0, 100) * 1000;
        float yOrg = Random.Range(0, 100) * 1000;

        int pixWidth = 1000;
        int pixHeight = 1000;
        if (isOuterPlanet) pixHeight = 50;

            planetMaterials[material] = new Material(starMaterial);
        planetTextures[material] = new Texture2D(pixWidth, pixHeight);
        Color[] pixels = new Color[planetTextures[material].width * planetTextures[material].height];
        planetMaterials[material].mainTexture = planetTextures[material];
        
        float y = 0.0F;

        while (y < planetTextures[material].height)
        {
            float x = 0.0F;
            while (x < planetTextures[material].width)
            {
                float xCoord = xOrg + x / planetTextures[material].width * scale;
                float yCoord = yOrg + y / planetTextures[material].height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                sample += sample * (Mathf.PerlinNoise(xCoord + 1000, yCoord + 500) - .5f) * .5f;
                sample += sample * (Mathf.PerlinNoise(xCoord - 1000, yCoord - 500) - .5f) * .1f;

                Color pixelColor = Color.white;

                if (isOuterPlanet)
                {
                    if (sample < .75) pixelColor = new Color32(123, 123, 0, 255);
                    if (sample < .50) pixelColor = new Color32(123, 62, 0, 255);
                    if (sample < .25) pixelColor = new Color32(123, 0, 0, 255);
                    if (sample < .10) pixelColor = new Color32(0, 0, 255, 255);
                }
                else
                {
                    if (sample < .90) pixelColor = new Color32(114, 148, 105, 255);
                    if (sample < .80) pixelColor = new Color32(8, 128, 8, 255);
                    if (sample < .35) pixelColor = new Color32(224, 215, 132, 255);
                    if (sample < .30) pixelColor = new Color32(13, 61, 110, 255);
                    if (sample < .20) pixelColor = new Color32(13, 37, 117, 255);
                }

                //if (sample < .90) pixelColor *= new Color32((byte)(sample * 150), (byte)(sample * 150), (byte)(sample * 150), 255);
                //if (sample < .80) pixelColor *= new Color32(0, (byte)(sample * 255), 0, 255);
                //if (sample < .35) pixelColor *= new Color32((byte)(sample * 255), (byte)(sample * 255), 0, 255);
                //if (sample < .30) pixelColor = new Color32(0, 0, (byte)(sample * 200), 255);
                //if (sample < .20) pixelColor = new Color32(0, 0, (byte)(sample * 255), 255);

                pixels[(int)y * planetTextures[material].width + (int)x] = pixelColor;//new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        planetTextures[material].SetPixels(pixels);
        planetTextures[material].Apply();
    }

    void GenerateAsteroidBelt()
    {

    }
}
