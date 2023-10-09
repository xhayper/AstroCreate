#nullable enable

using System.Collections.Generic;
using System.Linq;
using AstroCreate.Utilities;
using AstroDX.Contexts.Gameplay.PlayerScope;
using AstroDX.Contexts.Gameplay.SlideGenerators;
using SimaiSharp.Structures;
using UnityEngine;

namespace AstroCreate.Gameplay
{
    public class Slide
    {
        public static readonly GameObject SlideSegmentPrefab = Resources.Load<GameObject>("Prefabs/SlideSegment");
        public static readonly GameObject SlideStarPrefab = Resources.Load<GameObject>("Prefabs/SlideStar");
        private readonly SlideGenerator _slideGenerator;

        private readonly List<float> _slideLengths = new();

        public readonly float length;
        public readonly List<GameObject> SlideContainerList = new();

        public readonly List<SlideUtility.SlideGeneratorData> SlideGeneratorDatas;

        public readonly List<List<GameObject>> SlideObjectList = new();

        public readonly GameObject slideStar;

        public Slide(Note note, GameObject? parentObject = null)
        {
            SlideGeneratorDatas = new List<SlideUtility.SlideGeneratorData>();

            foreach (var generatorData in note.slidePaths
                         .Select(slidePath => SlideUtility.MakeSlideGenerator(note, slidePath))
                         .SelectMany(slideGenerators => slideGenerators))
            {
                SlideGeneratorDatas.Add(generatorData);

                var generator = generatorData.generator;

                List<GameObject> gameObjects = new();

                var slideLength = generator.GetLength();
                length += slideLength;

                var slideHolder = new GameObject();
                slideHolder.name = $"Slide ({generatorData.segment.slideType})";
                slideHolder.transform.parent = parentObject?.transform;

                var slideStar = UnityEngine.Object.Instantiate(SlideStarPrefab, slideHolder.transform, true);
                slideStar.name = "SlideStar";
                slideStar.SetActive(false);
                this.slideStar = slideStar;

                var slideId = 0;
                for (var i = RenderManager.SlideSpacing; i < slideLength; i += RenderManager.SlideSpacing)
                {
                    generator.GetPoint(Mathf.Clamp(i / slideLength, 0, 1), out var location, out var rotation);

                    var slide = UnityEngine.Object.Instantiate(SlideSegmentPrefab, slideHolder.transform);
                    slide.name = $"SlideSegment ({slideId})";
                    slide.transform.position = location;
                    slide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation * Mathf.Rad2Deg));
                    slide.SetActive(false);

                    gameObjects.Add(slide);

                    slideId++;
                }

                SlideObjectList.Add(gameObjects);
                SlideContainerList.Add(slideHolder);
            }
        }

        public void SetVisible(bool visible)
        {
            slideStar.SetActive(visible);
            foreach (var obj in SlideObjectList.SelectMany(pathNodeList => pathNodeList)) obj.SetActive(visible);
        }

        /**
         * @param t 0-1
         */
        public void SetVisible(float t)
        {
            if (slideStar)
                slideStar.SetActive(t is > 0 and < 1);

            if (SlideGeneratorDatas.Count == 1)
            {
                SlideGeneratorDatas[0].generator.GetPoint(t, out var location, out var rotation);
                slideStar.transform.position = location;
                slideStar.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation * Mathf.Rad2Deg));
            }

            var currentLength = 0f;

            foreach (var obj in SlideObjectList.SelectMany(pathNodeList => pathNodeList))
            {
                obj.SetActive(currentLength / length >= Mathf.Clamp(t, 0, 1));
                currentLength += RenderManager.SlideSpacing;
            }
        }

        public void Destroy()
        {
            foreach (var slideContainer in SlideContainerList) UnityEngine.Object.Destroy(slideContainer);

            SlideGeneratorDatas.Clear();
            SlideContainerList.Clear();
            SlideObjectList.Clear();
        }
    }
}