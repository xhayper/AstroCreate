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
        public static readonly GameObject SlidePrefab = Resources.Load("Prefabs/Slide") as GameObject;

        public readonly float length;

        public readonly List<List<GameObject>> SlideObjectList = new();
        public readonly List<SlidePath> SlidePaths;

        public Slide(GameObject parentObject, Note note, List<SlidePath> slidePaths)
        {
            SlidePaths = slidePaths;
            
            foreach (var generator in note.slidePaths
                         .Select(slidePath => SlideUtility.MakeSlideGenerator(note, slidePath))
                         .SelectMany(slideGenerators => slideGenerators))
            {
                List<GameObject> gameObjects = new();
            
                var slideLength = generator.GetLength();
                length += slideLength;
            
                for (var i = RenderManager.SlideSpacing; i < slideLength; i += RenderManager.SlideSpacing)
                {
                    generator.GetPoint(Mathf.Clamp(i / slideLength, 0, 1), out var location, out var rotation);
            
                    var gridPosition = location * 50;
                    var modifiedPosition = new Vector2(1280, 720) / 2;
                    modifiedPosition.x += gridPosition.x;
                    modifiedPosition.y += gridPosition.y;
            
                    var slide = UnityEngine.Object.Instantiate(SlidePrefab, parentObject.transform);
                    slide.transform.position = gridPosition;
                    slide.transform.Rotate(new Vector3(0, 0, rotation));
            
                    gameObjects.Add(slide);
                }
            
                SlideObjectList.Add(gameObjects);
            }
        }

        public void SetVisible(bool visible)
        {
            foreach (var node in SlideObjectList.SelectMany(pathNodeList => pathNodeList)) node.SetActive(visible);
        }

        /**
         * @param t 0-1
         */
        public void SetVisible(float t)
        {
            var currentLength = 0f;

            foreach (var node in SlideObjectList.SelectMany(pathNodeList => pathNodeList))
            {
                node.SetActive(currentLength / length >= Mathf.Clamp(t, 0, 1));
                currentLength += RenderManager.SlideSpacing;
            }
        }
    }
}