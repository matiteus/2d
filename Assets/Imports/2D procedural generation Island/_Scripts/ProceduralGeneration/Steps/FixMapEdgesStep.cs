using System.Collections.Generic;
using UnityEngine;

public class FixMapEdgesStep : GenerationStep
{
    [SerializeField]
    private bool m_applyStep = true;

    public override void Execute(GenerationData generationData)
    {
        if (m_applyStep == false)
            return;

        for (int x = 0; x < generationData.MapWidth; x++)
        {
            foreach (int y in new List<int> { 0, generationData.MapHeight - 1 })
            {
                generationData.FixSeaEdgesPositions.Add(new(x, y));
            }
        }
        for (int y = 0; y < generationData.MapHeight; y++)
        {
            foreach (int x in new List<int> { 0, generationData.MapHeight - 1 })
            {
                generationData.FixSeaEdgesPositions.Add(new(x, y));
            }
        }
    }

}
