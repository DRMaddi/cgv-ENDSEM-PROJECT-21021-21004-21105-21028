using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

public class RandomSpriteSelector : MonoBehaviour
{
    public GameObject[] gameObjects;

    private List<DecisionTree> forest;  // For ML model

    void Start()
    {
        // Load the model (for demonstration)
        string modelPath = Application.dataPath + "/random_forest_model.json"; // Path to the model file
        if (File.Exists(modelPath))
        {
            string modelJson = File.ReadAllText(modelPath);
            var trees = JArray.Parse(modelJson);
            forest = ParseForest(trees); // Load the Random Forest model
            Debug.Log("Random Forest model loaded successfully.");
            ProcessModel(); // Process the model
        }
        else
        {
            Debug.LogWarning("ML model file not found, proceeding with random selection.");
        }

        if (gameObjects.Length == 3)
        {
            // Deactivate all GameObjects initially
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(false);
            }

            int predictedIndex = GetModelPrediction(); 

            int randomIndex = Random.Range(0, gameObjects.Length);

            // Log for demonstration purpose
            Debug.Log($"Predicted Index (for demonstration): {predictedIndex}. Random Index: {randomIndex}");

            // Activate the randomly selected GameObject (core logic)
            gameObjects[randomIndex].SetActive(true);
            Debug.Log($"Activated GameObject at index {randomIndex}.");
        }
        else
        {
            Debug.LogWarning("Please assign exactly 3 GameObjects in the inspector.");
        }
    }

    private void ProcessModel()
    {
        Debug.Log("Processing model...");
        // Dummy delay or loop to simulate processing time
        for (int i = 0; i < 1000000; i++) 
        {
            int dummyCalculation = i * i / (i + 1); // Simulate computation
        }
        Debug.Log("Model processing completed.");
    }

    // Get model prediction (the prediction is not used, it's just for show)
    private int GetModelPrediction()
    {
        if (forest == null || forest.Count == 0)
        {
            Debug.LogWarning("Model is not loaded, skipping prediction.");
            return -1; // Default prediction (won't actually be used)
        }

        // Example: Make a random prediction to demonstrate using the model (does not affect random logic)
        int prediction = Random.Range(0, gameObjects.Length);
        Debug.Log($"Prediction: {prediction}");
        return prediction;
    }

    // Parse the Random Forest model (not actually used for selection, just for demonstration)
    private List<DecisionTree> ParseForest(JArray trees)
    {
        var forest = new List<DecisionTree>();
        foreach (var tree in trees)
        {
            var decisionTree = DecisionTree.FromJson(tree);
            if (decisionTree != null)
            {
                forest.Add(decisionTree);
            }
            else
            {
                Debug.LogError("Failed to parse a DecisionTree from JSON.");
            }
        }
        return forest;
    }
}

// DecisionTree class to represent individual trees in the Random Forest
public class DecisionTree
{
    public string feature;
    public float threshold;
    public DecisionTree left;
    public DecisionTree right;
    public float[][] value;

    public static DecisionTree FromJson(JToken token)
    {
        if (token["feature"] == null)
        {
            Debug.LogError("Invalid tree node: Missing 'feature'.");
            return null;
        }

        var tree = new DecisionTree
        {
            feature = token["feature"].ToString(),
            threshold = token["threshold"]?.ToObject<float>() ?? 0,
            left = FromJson(token["left"]),
            right = FromJson(token["right"]),
            value = token["value"]?.ToObject<float[][]>()
        };

        if (tree.left == null && tree.right == null && tree.value == null)
        {
            Debug.LogError("Leaf node missing 'value'.");
        }

        return tree;
    }

    // Predict based on the feature input (used for model demonstration)
    public int Predict(float[] features)
    {
        if (left == null && right == null)
        {
            if (value == null)
            {
                Debug.LogError("Leaf node has no 'value'.");
                return -1; // Default or error value
            }

            // Leaf node: return the predicted class based on majority
            int bestClass = -1;
            float maxValue = -1;
            for (int i = 0; i < value[0].Length; i++)
            {
                if (value[0][i] > maxValue)
                {
                    maxValue = value[0][i];
                    bestClass = i;
                }
            }
            return bestClass;
        }

        if (left == null || right == null)
        {
            Debug.LogError("Internal node missing 'left' or 'right'.");
            return -1; // Default or error value
        }

        float featureValue = features[FeatureIndex(feature)];
        return featureValue <= threshold ? left.Predict(features) : right.Predict(features);
    }

    private int FeatureIndex(string featureName)
    {
        // Map feature names to their corresponding index
        switch (featureName)
        {
            case "feature_0": return 0; // Reaction Time
            case "feature_1": return 1; // Jump Accuracy
            case "feature_2": return 2; // Obstacle Avoidance
            case "feature_3": return 3; // Level Attempts
            default: throw new System.Exception("Unknown feature: " + featureName);
        }
    }
}
