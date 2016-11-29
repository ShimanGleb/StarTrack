using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urho;
using Urho.Gui;
using Urho.Actions;
using Urho.Shapes;

namespace StartTrack
{
    public class StarSky : Application
    {
        List<Star> stars = new List<Star>();
        Camera camera;
        Octree octree;

        public StarSky(ApplicationOptions options = null) : base(new ApplicationOptions(assetsFolder: "Data") { Height = 1024, Width = 576, Orientation = ApplicationOptions.OrientationType.Portrait }) { }
        
        protected override void Start()
        {
            CreateScene();

            // Subscribe to Esc key:
            //Input.SubscribeToKeyDown(args => { if (args.Key == Key.Esc) Exit(); });
        }

        async void CreateScene()
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
            var scene = new Scene(Context);
            octree = scene.CreateComponent<Octree>();

            // Box	
            Node starNode = scene.CreateChild(name: "Box node");
            starNode.Position = new Vector3(0, 0, 0);
            starNode.SetScale(1);
            starNode.Rotation = new Quaternion(0,90,0);
            
            StaticModel starModel = starNode.CreateComponent<StaticModel>();
            starModel.Model = ResourceCache.GetModel("Models/Star.mdl");
            starModel.SetMaterial(ResourceCache.GetMaterial("Materials/Star.xml"));
            
            // Light
            Node lightNode = scene.CreateChild(name: "light");
            var light = lightNode.CreateComponent<Light>();
            light.Range = 10;
            light.Brightness = 10f;
            lightNode.Position = new Vector3(0,0,-10);
            
            // Camera
            Node cameraNode = scene.CreateChild(name: "camera");
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3(30,50,-150);
            // Viewport
            Renderer.SetViewport(0, new Viewport(Context, scene, camera, null));

            // Do actions
            /*await starNode.RunActionsAsync(new EaseBounceOut(new ScaleTo(duration: 1f, scale: 1)));
            await starNode.RunActionsAsync(new RepeatForever(
                new RotateBy(duration: 1, deltaAngleX: 90, deltaAngleY: 0, deltaAngleZ: 0)));*/
        }

        void OnTouched(TouchEndEventArgs e)
        {
            Ray cameraRay = camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
            var results = octree.Raycast(cameraRay, RayQueryLevel.Triangle, 500, DrawableFlags.Geometry);
            if (results != null && results.Any())
            {
                var star = results[0];                
                star.Node.RunActionsAsync(new RepeatForever(new EaseBackOut(new ScaleTo(duration: 0.7f, scale: 1.3f)), new EaseBackOut(new ScaleTo(duration: 0.7f, scale: 1))));
            }
        }
    }
}