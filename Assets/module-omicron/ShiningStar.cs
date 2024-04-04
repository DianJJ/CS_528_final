﻿using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.IO;
using System;

public class ShiningStar : MonoBehaviour
{
    public string csvFilePath; // CSV file path.

    private List<StarData> starsData = new List<StarData>();
    private List<Constellation> constellations = new List<Constellation>(); 
    private List<LineRenderer> LineRenderers = new List<LineRenderer>();
    private bool constellationLineRendererEnable = true;

    public Material starMaterial;


    void Start()
    {
        LoadDataFromCSV("Assets/module-omicron/athyg_31_reduced_m10.csv");
        CreateStars();
        LoadConstellations("Assets/module-omicron/constellationship.fab");
        DrawConstellations();

        foreach (Constellation constellation in constellations)
        {
            CreateConstellationCollider(constellation);
        }
        //OnDrawGizmos();

    }
    void Update()
    {
        UpdateConstellationLine();
        foreach (Constellation constellation in constellations)
        {
            UpdateConstellationCollider(constellation);
        }
    }

    void OnDrawGizmos()
    {
        foreach (Constellation constellation in constellations)
        {
            Bounds bounds = CalculateConstellationBounds(constellation);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }

    void UpdateConstellationLine()
    {
        for (int i = 0; i < constellations.Count; i++)
        {
            // Reset k for each constellation
            int k = 0;

            for (int j = 0; j < constellations[i].STAR_PAIRS.Count; j++)
            {
                GameObject starObject_1 = constellations[i].STAR_PAIRS[j].Item1;
                GameObject starObject_2 = constellations[i].STAR_PAIRS[j].Item2;

                if (starObject_1 != null && starObject_2 != null)
                {
                    if (k < constellations[i].LineRenderers.Count)
                    {
                        // Set positions for the LineRenderer
                        constellations[i].LineRenderers[k].SetPosition(0, starObject_1.transform.position);
                        constellations[i].LineRenderers[k].SetPosition(1, starObject_2.transform.position);


                        // Optionally, if you want to tag the GameObject to which the LineRenderer is attachedif
                        //Debug.Log(constellations[i].NAME);
                        //constellations[i].LineRenderers[k].gameObject.tag = constellations[i].NAME;
                        //if (constellations[i].NAME == "Lib")
                        //{
                        //    constellations[i].LineRenderers[k].gameObject.tag = "Lib";
                        //}

                        //if (constellations[i].NAME == "Ari")
                        //{
                        //    constellations[i].LineRenderers[k].gameObject.tag = "Ari";
                        //}

                        //if (constellations[i].NAME == "Vul")
                        //{
                        //    constellations[i].LineRenderers[k].gameObject.tag = "Vul";
                        //}
                        //if (constellations[i].NAME == "Tuc")
                        //{
                        //    constellations[i].LineRenderers[k].gameObject.tag = "Tuc";
                        //}

                        //if (constellations[i].NAME == "LMi")
                        //{
                            //Debug.Log(constellations[i].NAME);
                            //constellations[i].LineRenderers[k].gameObject.tag = "LMi";
                        //}
                         // Increment k for the next line within the same constellation
                        
                        //BoxCollider collider = constellations[i].LineRenderers[k].GetComponent<BoxCollider>();
                        //if (collider == null)
                        //{
                            //collider = constellations[i].LineRenderers[k].gameObject.AddComponent<BoxCollider>();
                            //collider.isTrigger = true; // Set as trigger if you don't want it to physically block objects
                        //}
                        //AdjustColliderToLine(collider, starObject_1.transform.position, starObject_2.transform.position);
                        k++;
                    }
                }
            }
        }
    }

    public Constellation GetConstellationByName(string name)
    {
        return constellations.Find(constellation => constellation.NAME.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public void ToggleConstellationVisibility()
    {
        // Toggle the state of constellationLineRendererEnable
        constellationLineRendererEnable = !constellationLineRendererEnable;

        // Apply the visibility state to all LineRenderers
        foreach (var lineRenderer in LineRenderers)
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = constellationLineRendererEnable;
            }
        }
    }

    public void ToggleConstellationLibra()
    {
        // This assumes you have a way to enable/disable a constellation.
        // For example, you could disable all line renderers associated with this constellation.
        Constellation constellation = GetConstellationByName("Lib");

        bool isVisible = !constellation.LineRenderers[0].enabled;
        foreach (var lineRenderer in constellation.LineRenderers)
        {
            lineRenderer.enabled = isVisible;
        }
    }

    public void ToggleConstellationAri()
    {
        // This assumes you have a way to enable/disable a constellation.
        // For example, you could disable all line renderers associated with this constellation.
        Constellation constellation = GetConstellationByName("Ari");

        bool isVisible = !constellation.LineRenderers[0].enabled;
        foreach (var lineRenderer in constellation.LineRenderers)
        {
            lineRenderer.enabled = isVisible;
        }
    }

    public void ToggleConstellationLMi()
    {
        // This assumes you have a way to enable/disable a constellation.
        // For example, you could disable all line renderers associated with this constellation.
        Constellation constellation = GetConstellationByName("LMi");

        bool isVisible = !constellation.LineRenderers[0].enabled;
        foreach (var lineRenderer in constellation.LineRenderers)
        {
            lineRenderer.enabled = isVisible;
        }
    }

    public void ToggleConstellationVul()
    {
        // This assumes you have a way to enable/disable a constellation.
        // For example, you could disable all line renderers associated with this constellation.
        Constellation constellation = GetConstellationByName("Vul");

        bool isVisible = !constellation.LineRenderers[0].enabled;
        foreach (var lineRenderer in constellation.LineRenderers)
        {
            lineRenderer.enabled = isVisible;
        }
    }

    public void ToggleConstellationTuc()
    {
        // This assumes you have a way to enable/disable a constellation.
        // For example, you could disable all line renderers associated with this constellation.
        Constellation constellation = GetConstellationByName("Tuc");

        bool isVisible = !constellation.LineRenderers[0].enabled;
        foreach (var lineRenderer in constellation.LineRenderers)
        {
            lineRenderer.enabled = isVisible;
        }
    }
    //public void ToggleConstellationLine(bool showLine)
    //{
    //    constellationLineRenderer.enabled = showLine;
    //}

    public void IncreaseStarVelocity()
    {
        Debug.Log("IncreaseStarVelocity");
        foreach (var star in starsData)
        {
            float increaseTime = 10.0f; // Define how much you want to increase the velocity
            star.VX *= increaseTime;
            star.VY *= increaseTime;
            star.VZ *= increaseTime;
        }
    }

    public void DecreaseStarVelocity()
    {
        foreach (var star in starsData)
        {
            float increaseTime = 0.1f; // Define how much you want to increase the velocity
            star.VX *= increaseTime;
            star.VY *= increaseTime;
            star.VZ *= increaseTime;
        }
    }

    public void ResetStarVelocity()
    {
        foreach (var star in starsData)
        {
            float increaseTime = 10.0f; // Define how much you want to increase the velocity
            star.VX = star.VX_ori;
            star.VY *= star.VY_ori;
            star.VZ *= star.VZ_ori;
        }
    }

    public void ReverseStarVelocity()
    {
        foreach (var star in starsData)
        {
            float increaseTime = -1.0f; // Define how much you want to increase the velocity
            star.VX *= increaseTime;
            star.VY *= increaseTime;
            star.VZ *= increaseTime;
        }
    }



    void LoadDataFromCSV(string csvFilePath)
    {
        try
        {
            //Debug.Log("Start loading csv file");
            string[] csvLines = File.ReadAllLines(csvFilePath);

            for (int i = 1; i < csvLines.Length; i++) // Read from the second line; the first line is typically the header.
            {
                string[] values = csvLines[i].Split(',');

                //Debug.Log("Start spliting csv file");
                // Parse the CSV data and create StarData objects.
                StarData star = new StarData();
                //Debug.Log("Start loading ID");
                star.ID = int.Parse(values[0]);

                if (!string.IsNullOrEmpty(values[1]) && !string.IsNullOrEmpty(values[3]) && !string.IsNullOrEmpty(values[3]) && !string.IsNullOrEmpty(values[4]) && !string.IsNullOrEmpty(values[5])
                    && !string.IsNullOrEmpty(values[6]) && !string.IsNullOrEmpty(values[7])
                    && !string.IsNullOrEmpty(values[8]) && !string.IsNullOrEmpty(values[9]) && !string.IsNullOrEmpty(values[10]) && !string.IsNullOrEmpty(values[11]
                    ))
                {
                    star.HIP = int.Parse(values[1]);
                    star.DIST = float.Parse(values[2]);
                    star.X0 = float.Parse(values[3])*5;
                    star.Y0 = float.Parse(values[4])*5;
                    star.Z0 = float.Parse(values[5])*5;
                    star.ABSMAG = float.Parse(values[6]);
                    star.MAG = float.Parse(values[7]);
                    star.VX = float.Parse(values[8]) * 1.02269E-6f;
                    star.VY = float.Parse(values[9]) * 1.02269E-6f;
                    star.VZ = float.Parse(values[10]) * 1.02269E-6f;
                    star.VX_ori = float.Parse(values[8]) * 1.02269E-6f;
                    star.VY_ori = float.Parse(values[9]) * 1.02269E-6f;
                    star.VZ_ori = float.Parse(values[10]) * 1.02269E-6f;
                    star.SPECT = values[11];
                }

                // Check and add valid star data.
                if ((star.DIST < 25* 3.262) && !float.IsNaN(star.HIP) && !string.IsNullOrEmpty(star.SPECT) && !float.IsNaN(star.X0) && !float.IsNaN(star.Y0) && !float.IsNaN(star.Z0))
                {

                    starsData.Add(star);
                    //Debug.Log(star);
                }
            }


        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data from CSV: " + e.Message);
        }
    }

    //void CreateStars()
    //{
     //   foreach (var starData in starsData)
     //   {
            // Create star objects based on StarData.
     //       GameObject starObject = Instantiate(starPrefab, new Vector3(starData.X0, starData.Y0, starData.Z0), Quaternion.identity);
     //       starObject.name = $"Star_{starData.HIP}";

            // Set star size based on brightness.
     //       float starSize = Mathf.Clamp(5.0f / starData.ABSMAG, 0.1f, 50.0f);
     //       starObject.transform.localScale = new Vector3(starSize, starSize, starSize);

            // Set star color based on spectral type.
     //       Color starColor = GetColorBySpectralType(starData.SPECT);
     //       starObject.GetComponent<Renderer>().material.color = starColor;

            //Place the star objects in the scene.
    //        starObject.transform.SetParent(transform);

    //        starObject.AddComponent<StarOrbit>().InitializeOrbit(starData, transform);
    //    }
    //}

    void CreateStars()
    {
        foreach (var starData in starsData)
        {
            // Create a Quad for the star.
            GameObject starObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(starObject.GetComponent<Collider>()); // Quads come with a collider component that you likely don't need.
            starObject.transform.position = new Vector3(starData.X0, starData.Y0, starData.Z0);
            starObject.name = $"Star_{starData.HIP}";

            // Set star size based on brightness.
            float starSize = Mathf.Clamp(10.0f / starData.ABSMAG, 0.1f, 50.0f);
            starObject.transform.localScale = new Vector3(starSize, starSize, starSize);

            // Apply the custom shader Material.
            starObject.GetComponent<Renderer>().material = starMaterial;

            // Optionally adjust material properties per star, e.g., color based on spectral type.
            // This assumes your shader/material supports a "_Color" property.
            Color starColor = GetColorBySpectralType(starData.SPECT);
            starObject.GetComponent<Renderer>().material.SetColor("_Color", starColor);

            // Place the star objects in the scene.
            starObject.transform.SetParent(transform);

            // Ensure the Quad faces the camera. Assuming you have a Billboard script as discussed.
            //starObject.AddComponent<Billboard>().cam = Camera.main;
            if (starObject.GetComponent<Billboard>() == null)
            {
                Billboard billboardComponent = starObject.AddComponent<Billboard>();
                billboardComponent.cam = Camera.main; // Assuming your billboard script uses a 'cam' variable to reference the camera
            }
            // Initialize star orbit if necessary.
            starObject.AddComponent<StarOrbit>().InitializeOrbit(starData, transform);
        }
    }

    void LoadConstellations(string fabFilePath)
    {
        string[] constellationLines = File.ReadAllLines(fabFilePath);

        //Debug.Log("successful loading the constellation file");



        foreach (var constellationLine in constellationLines)
        {
            string[] constellationData = System.Text.RegularExpressions.Regex.Split(constellationLine, @"\s+");
            //string[] constellationData = constellationLine.Split(' ');
            Constellation constellation = new Constellation();

            // Parse constellation data.
            //Debug.Log("constellationData.Length: " + constellationData.Length);
            //Debug.Log(constellationData[0]);
            //Debug.Log(constellationData[1]);
            string constellationName = constellationData[0];
            int starCount = int.Parse(constellationData[1]);

            constellation.NAME = constellationName;

            constellation.PAIR_NUMBER = starCount;
            //Debug.Log("successful parsing the constellation file");

            //The HIP (Hipparcos) number of stars begins from the third element.
            for (int i = 2; i < constellationData.Length - 1; i += 2)
            {
                //Debug.Log("i: " + i + ", constellationData.Length: " + constellationData.Length);

                //Debug.Log(constellationData.Length);
                int hipNumber_1 = int.Parse(constellationData[i]);
                //Debug.LogError("Index out of bounds while parsing constellation data.");
                //Debug.Log(i + 1);
                int hipNumber_2 = int.Parse(constellationData[i + 1]);

                //Debug.Log("successful load the hip");
                //Find the corresponding star objects.
                //Debug.Log("hipNumber_1" + hipNumber_1);
                //Debug.Log("hipNumber_2" + hipNumber_2);
                GameObject starObject_1 = FindStarByHIP(hipNumber_1);

                GameObject starObject_2 = FindStarByHIP(hipNumber_2);

                //Debug.Log("successful find the star by hip");
                if (starObject_1 != null && starObject_1 != null)
                {
                    //Debug.Log("successful add the star pair");
                    Tuple<GameObject, GameObject> starpair = Tuple.Create(starObject_1, starObject_2);

                    constellation.STAR_PAIRS.Add(starpair);
                }



                //if (starObject_1 != null && starObject_1 != null)
                //{
                //Create connecting lines or other geometric shapes representing constellations between stars.
                //DrawConstellationLine(starObject_1.transform.position, starObject_2.transform.position);
                //}
                //if (i >= constellationData.Length - 2)
                //{
                //Debug.Log("Index out of bounds while parsing constellation data.");
                //  break;  // 
                //}
            }
            //Debug.Log("pair length:"+ constellation.STAR_PAIRS.Count);
            //Debug.Log("successful add the constellation");
            if (constellation.STAR_PAIRS.Count == starCount)
            {
                constellations.Add(constellation);
            }
            
        }
    }

    Color GetColorBySpectralType(string spectralType)
    {
        // According to the spectral type, return the color.
        switch (spectralType[0])
        {
            case 'O': return Color.blue;
            case 'B': return Color.cyan;
            case 'A': return Color.white;
            case 'F': return Color.yellow;
            case 'G': return Color.yellow;
            case 'K': return new Color(1.0f, 0.5f, 0.0f); 
            case 'M': return Color.red;
            default: return Color.gray;
        }
    }

    void DrawConstellations()
    {
        // Read the constellation file.

        // The HIP number of the stars starts from the third element.
        for (int i = 0; i < constellations.Count; i++)
            {
            // You're finding the corresponding star objects based on the HIP numbers in the constellation data.
            for (int j = 0; j < constellations[i].STAR_PAIRS.Count; j++)
            {
                GameObject starObject_1 = constellations[i].STAR_PAIRS[j].Item1;

                GameObject starObject_2 = constellations[i].STAR_PAIRS[j].Item2;

                if (starObject_1 != null && starObject_2 != null)
                {
                    // You are attempting to draw lines or other geometric shapes between stars to represent constellations.
                    //Debug.Log("successfull draw lines");
                    //Debug.Log("star1 position: "+starObject_1.transform.position+ "star2 position: "+starObject_2.transform.position);
                    //LineRenderers.Add(lineRenderer);
                    DrawConstellationLine(starObject_1.transform.position, starObject_2.transform.position, constellations[i]);
                }
            }
            }
        
    }

    GameObject FindStarByHIP(int hipNumber)
    {
        //  find the corresponding GameObject in the scene using the HIP (Henry Draper Catalog) number.
        foreach (var starData in starsData)
        {
            if (starData.HIP == hipNumber)
            {
                return GameObject.Find($"Star_{hipNumber}"); 
            }
        }

        return null;
    }

    void DrawConstellationLine(Vector3 starPosition_1, Vector3 starPosition_2, Constellation constellation)
    {
        // draw lines or geometric shapes between stars to represent constellations. 
        //GameObject constellationLine = new GameObject("ConstellationLine");
        //constellationLine.transform.position = starPosition_1;
        //constellationLine.AddComponent<LineRenderer>();
        //Debug.Log("successfull draw lines");
        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        //Debug.Log(lineRenderer);
        //LineRenderer lineRenderer = GetComponent<LineRenderer>();
        GameObject constellationLine = new GameObject("ConstellationLine");
        LineRenderer lineRenderer = constellationLine.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.material.color = Color.white;


        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.material.color = Color.white;

        if (lineRenderer != null)
        {
            //Debug.Log("line renderer is not null");
            
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, starPosition_1);
            lineRenderer.SetPosition(1, starPosition_2);

            //Add collider
            //BoxCollider collider = constellationLine.AddComponent<BoxCollider>();
            //collider.isTrigger = true; // Set as trigger if you don't want it to physically block objects
            //AdjustColliderToLine(collider, starPosition_1, starPosition_2);


            LineRenderers.Add(lineRenderer);
            constellation.LineRenderers.Add(lineRenderer);
        }

    }

    //void AdjustColliderToLine(BoxCollider collider, Vector3 startPoint, Vector3 endPoint)
    //{
        //Vector3 midPoint = (startPoint + endPoint) / 2;
        //collider.transform.position = midPoint; // Position at the midpoint

        //float lineLength = Vector3.Distance(startPoint, endPoint);
        // Assuming the line's thickness is minimal, adjust the X or Y scale for the collider's thickness
        //collider.size = new Vector3(10f, 10f, lineLength); // Z is length here

        // Correctly orient the collider
        //Vector3 vectorToTarget = endPoint - startPoint;
        //Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
        // Apply rotation. Because BoxCollider is along the z-axis, this directly applies.
        //collider.transform.rotation = targetRotation;
    //}

    Bounds CalculateConstellationBounds(Constellation constellation)
    {
        if (constellation.STAR_PAIRS.Count == 0) return new Bounds();

        Vector3 min = constellation.STAR_PAIRS[0].Item1.transform.position;
        Vector3 max = constellation.STAR_PAIRS[0].Item1.transform.position;

        foreach (var starPair in constellation.STAR_PAIRS)
        {
            if (starPair.Item1 != null && starPair.Item2 != null)
            {
                min = Vector3.Min(min, starPair.Item1.transform.position);
                min = Vector3.Min(min, starPair.Item2.transform.position);

                max = Vector3.Max(max, starPair.Item1.transform.position);
                max = Vector3.Max(max, starPair.Item2.transform.position);
            }
        }

        Vector3 center = (min + max) / 2;
        Vector3 size = max - min;

        return new Bounds(center, size);
    }

    void CreateConstellationCollider(Constellation constellation)
    {
        Bounds bounds = CalculateConstellationBounds(constellation);
        //Debug.DrawLine(bounds.min, bounds.max, Color.red, 20f);
        GameObject constellationColliderGO = new GameObject($"{constellation.NAME} Collider");
        BoxCollider collider = constellationColliderGO.AddComponent<BoxCollider>();
        collider.center = bounds.center - constellationColliderGO.transform.position;
        collider.size = bounds.size;
        collider.isTrigger = true;

        constellationColliderGO.transform.parent = this.transform;

        constellationColliderGO.tag = constellation.NAME;

        constellation.ColliderGameObject = constellationColliderGO; // Assign the collider GameObject here
        
    }
    void UpdateConstellationCollider(Constellation constellation)
    {
        if (constellation.ColliderGameObject == null) return;

        Bounds bounds = CalculateConstellationBounds(constellation);
        BoxCollider collider = constellation.ColliderGameObject.GetComponent<BoxCollider>();

        // Adjust the collider's center and size
        collider.center = bounds.center - constellation.ColliderGameObject.transform.position; // Ensure this is relative to the collider's GameObject
        collider.size = bounds.size;
    }
}

// 星星数据类
public class StarData
{
    public int ID;
    public int HIP;
    public float DIST;
    public float X0, Y0, Z0;
    public float ABSMAG, MAG;
    public float VX, VY, VZ;
    public float VX_ori, VY_ori, VZ_ori;
    public string SPECT;
}


public class Constellation
{
    public string NAME;
    public int PAIR_NUMBER;
    public List<Tuple<GameObject, GameObject>> STAR_PAIRS = new List<Tuple<GameObject, GameObject>>();
    public List<LineRenderer> LineRenderers = new List<LineRenderer>();
    public GameObject ColliderGameObject;
}

public class StarOrbit : MonoBehaviour
{
    private StarData starData;
    private Transform center; // The object's Transform, which is the center around which the star rotates.
    private LineRenderer constellationLineRenderer; // LineRenderer used for drawing constellation connecting lines.

    public void InitializeOrbit(StarData starData, Transform center)
    {
        this.starData = starData;
        this.center = center;
    }

    void Update()
    {
        // Calculate the angle of rotation per frame.
        float rotationSpeed = Mathf.Sqrt(starData.VX * starData.VX + starData.VY * starData.VY + starData.VZ * starData.VZ);

        // Rotate the star based on velocity components.
        transform.RotateAround(center.position, new Vector3(starData.VX, starData.VY, starData.VZ), rotationSpeed * Time.deltaTime);

        //UpdateConstellationLine();
    }

    /*void UpdateConstellationLine()
    {
        if (constellationLineRenderer.enabled)
        {
            //
            constellationLineRenderer.SetPosition(0, center.position);
            constellationLineRenderer.SetPosition(1, transform.position);
        }
    }

    public void ToggleConstellationLine(bool showLine)
    {
        constellationLineRenderer.enabled = showLine;
    }*/
 }


