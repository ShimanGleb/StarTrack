﻿using System;
using System.Collections.Generic;
using System.Linq;

using Urho;
using Urho.Gui;

namespace StartTrack
{
    public class StarSky : Application
    {
        List<Node> stars = new List<Node>();
        Scene scene;
        Octree octree;
        Camera camera;
        
        Star SelectedStar=null;

        public StarSky(ApplicationOptions options = null) : base(new ApplicationOptions(assetsFolder: "Data") { Height = 1024, Width = 576, Orientation = ApplicationOptions.OrientationType.Portrait }) { }
        
        protected override void Start()
        {
            CreateScene();            
            Input.SubscribeToKeyDown(args => { if (args.Key == Key.Esc) Exit(); });
        }

        void CreateScene()
        {
            Input.SubscribeToTouchEnd(OnTouched);

            // UI text 
            var helloText = new Text(Context);
            helloText.Value = "Select your start track!";
            helloText.HorizontalAlignment = HorizontalAlignment.Center;
            helloText.VerticalAlignment = VerticalAlignment.Top;
            helloText.SetColor(new Color(r: 0f, g: 1f, b: 1f));
            helloText.SetFont(font: ResourceCache.GetFont("Fonts/Font.ttf"), size: 30);
            UI.Root.AddChild(helloText);

            // 3D scene with Octree
            scene = new Scene(Context);
            octree = scene.CreateComponent<Octree>();

            Node cameraNode = scene.CreateChild(name: "camera");
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3(30, 50, -150);
            // Viewport
            Renderer.SetViewport(0, new Viewport(Context, scene, camera, null));

            var lightNode = scene.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.Range = 200;
            light.Brightness = 150f;
            lightNode.Position = new Vector3(0, 0, -150);

            for (int i = 0; i < 10; i++)
            {
                var starNode = scene.CreateChild();
                Random randomY = new Random();
                
                starNode.Position = new Vector3(6*i,randomY.Next(0,20)*5,0);                            
                var star = new Star();
                starNode.AddComponent(star);
                stars.Add(starNode);                
            }                  
        }

        void OnTouched(TouchEndEventArgs e)
        {
            Ray cameraRay = camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
            var results = octree.Raycast(cameraRay, RayQueryLevel.Triangle, 500, DrawableFlags.Geometry);
            if (results != null && results.Any())
            {
                var star = results[0].Node?.Parent?.GetComponent<Star>();
                if (SelectedStar != star)
                {
                    if (SelectedStar != null)
                    {
                        var trackNode = scene.CreateChild();
                        Random randomY = new Random();

                        trackNode.Position = SelectedStar.Node.Position;
                        var track = new Track();
                        trackNode.AddComponent(track);                        

                        var angle=TrigonometryHelper.GetAngle(SelectedStar.Node.Position, star.Node.Position);

                        trackNode.Rotation = new Quaternion(0, 0, angle);
                        trackNode.Position = TrigonometryHelper.GetBetween(SelectedStar.Node.Position,star.Node.Position);
                        trackNode.Position = new Vector3(trackNode.Position.X, trackNode.Position.Y,0);
                        trackNode.Scale = new Vector3(TrigonometryHelper.GetDistance(SelectedStar.Node.Position,star.Node.Position),0.5f,1);
                    }                    
                    SelectedStar?.Deselect();
                    SelectedStar = star;
                    SelectedStar?.Select();                    
                }
                else
                {
                    SelectedStar?.Deselect();
                    SelectedStar = null;
                }                
            }
        }
    }
}