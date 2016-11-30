using System;

using Urho;
using Urho.Actions;

namespace StartTrack
{
    public class Star : Component
    {
        Node starNode;                
        Color color;
        
        public void SetValueWithAnimation(float value) => starNode.RunActionsAsync(new EaseBackOut(new ScaleTo(0, 1, value, 1)));

        public Star()
        {
            color = new Color(1f, 1f, 1f);
            ReceiveSceneUpdates = true;
        }
        
        public override void OnAttachedToNode(Node node)
        {
            starNode = node.CreateChild();            
            starNode.Position = new Vector3(0, 0, 0);
            starNode.SetScale(1);
            starNode.Rotation = new Quaternion(0, 90, 0);

            var starModel = starNode.CreateComponent<StaticModel>();
            var cache = Application.ResourceCache;
            starModel.Model = cache.GetModel("Models/Star.mdl");
            starModel.SetMaterial(cache.GetMaterial("Materials/Star.xml"));            

            var lightNode = node.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.Range = 10;
            light.Brightness = 10f;
            lightNode.Position = new Vector3(0, 0, -10);

            base.OnAttachedToNode(node);
        }

        public void Deselect()
        {
            starNode.RemoveAllActions();
            starNode.SetScale(1);            
        }

        public void Select()
        {
            Selected?.Invoke(this);            
            starNode.RunActionsAsync(new RepeatForever(new EaseBackOut(new ScaleTo(duration: 0.7f, scale: 1.3f)), new EaseBackOut(new ScaleTo(duration: 0.7f, scale: 1))));
        }

        public event Action<Star> Selected;
    }
}
