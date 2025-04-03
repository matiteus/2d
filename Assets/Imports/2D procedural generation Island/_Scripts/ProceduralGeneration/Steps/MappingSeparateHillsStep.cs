using System.Collections.Generic;
using UnityEngine;

public class MappingSeparateHillsStep : GenerationStep
{
    List<HashSet<Vector2Int>> m_hillsLevel1 = new(), m_hillsLevel2 = new();
    public override void Execute(GenerationData generationData)
    {
        generationData.MappedHillsLevel1 = FindSeparateHills(generationData.HillLevel1Interior);
        generationData.MappedHillsLevel2 = FindSeparateHills(generationData.HillLevel2Interior);

        m_hillsLevel1 = generationData.MappedHillsLevel1;
        m_hillsLevel2 = generationData.MappedHillsLevel2;
    }

    private List<HashSet<Vector2Int>> FindSeparateHills(HashSet<Vector2Int> hillData)
    {
        List<HashSet<Vector2Int>> separateHills = new();
        HashSet<Vector2Int> hillPositionsToCheck = new(hillData);
        foreach (var hillPosition in hillData)
        {
            if (hillPositionsToCheck.Contains(hillPosition))
            {
                HashSet<Vector2Int> hillPositions = new();
                Queue<Vector2Int> queue = new();
                queue.Enqueue(hillPosition);
                hillPositions.Add(hillPosition);
                hillPositionsToCheck.Remove(hillPosition);

                while (queue.Count > 0)
                {
                    Vector2Int currentHillPosition = queue.Dequeue();
                    foreach (Vector2Int direction in DirectionsHelper.DirectionOffsets8)
                    {
                        Vector2Int adjacentHillPosition = currentHillPosition + direction;
                        if (hillData.Contains(adjacentHillPosition) && hillPositionsToCheck.Contains(adjacentHillPosition))
                        {
                            queue.Enqueue(adjacentHillPosition);
                            hillPositions.Add(adjacentHillPosition);
                            hillPositionsToCheck.Remove(adjacentHillPosition);
                        }
                    }
                }
                separateHills.Add(hillPositions);
            }
        }
        return separateHills;
    }

    List<Color> colors = new();
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            //Show separate hills in different colors
            int index = 0;
            foreach (var hill in m_hillsLevel1)
            {
                if (colors.Count > index)
                {
                    Gizmos.color = colors[index];
                }
                else
                {
                    colors.Add(UnityEngine.Random.ColorHSV());
                    Gizmos.color = colors[^1];
                }
                index++;
                foreach (var pos in hill)
                {
                    Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
                }
            }
            foreach (var hill in m_hillsLevel2)
            {
                if (colors.Count > index)
                {
                    Gizmos.color = colors[index];
                }
                else
                {
                    colors.Add(UnityEngine.Random.ColorHSV());
                    Gizmos.color = colors[^1];
                }
                index++;
                foreach (var pos in hill)
                {
                    Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
                }
            }
        }
    }
}
