using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public List<PointOfInterest> NextPointsOfInterest { get; set; } = new();

    // Add this property to store points connected by paths
    // public List<PointOfInterest> NextPointsOfInterestWithPath { get; set; } = new();
}
