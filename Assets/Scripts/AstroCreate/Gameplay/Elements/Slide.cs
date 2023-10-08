#nullable enable

using System.Collections.Generic;
using System.Linq;
using AstroCreate.Utilities;
using AstroDX.Contexts.Gameplay.PlayerScope;
using SimaiSharp.Structures;
using UnityEngine;

namespace AstroCreate.Gameplay
{
    public class Slide
    {
        public static readonly GameObject SlideSegmentPrefab = Resources.Load<GameObject>("Prefabs/SlideSegment");

        public readonly float length;

        public readonly List<List<GameObject>> SlideObjectList = new();
        public readonly List<SlidePath> SlidePaths;

        public Slide(Note note, List<SlidePath> slidePaths, GameObject? parentObject = null)
        {
            SlidePaths = slidePaths;

            foreach (var generator in note.slidePaths
                         .Select(slidePath => SlideUtility.MakeSlideGenerator(note, slidePath))
                         .SelectMany(slideGenerators => slideGenerators))
            {
                List<GameObject> gameObjects = new();

                var slideLength = generator.GetLength();
                length += slideLength;

                var slideHolder = new GameObject();
                slideHolder.name = "Slide";
                slideHolder.transform.parent = parentObject?.transform;

                var slideId = 0;
                for (var i = RenderManager.SlideSpacing; i < slideLength; i += RenderManager.SlideSpacing)
                {
                    generator.GetPoint(Mathf.Clamp(i / slideLength, 0, 1), out var location, out var rotation);
                    
                    var slide = UnityEngine.Object.Instantiate(SlideSegmentPrefab, slideHolder.transform);
                    slide.name = $"SlideSegment ({slideId})";
                    slide.transform.position = location;
                    slide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -rotation));
                    slide.SetActive(false);

                    gameObjects.Add(slide);

                    slideId++;
                }

                SlideObjectList.Add(gameObjects);
            }
        }

        public void SetVisible(bool visible)
        {
            foreach (var obj in SlideObjectList.SelectMany(pathNodeList => pathNodeList)) obj.SetActive(visible);
        }

        /**
         * @param t 0-1
         */
        public void SetVisible(float t)
        {
            var currentLength = 0f;

            foreach (var obj in SlideObjectList.SelectMany(pathNodeList => pathNodeList))
            {
                obj.SetActive(currentLength / length >= Mathf.Clamp(t, 0, 1));
                currentLength += RenderManager.SlideSpacing;
            }
        }
    }
}