using Urho;
using Urho.Actions;

namespace StartTrack
{
    public class Track : Component
    {
        Node trackNode;
        Color color;

        public void SetValueWithAnimation(float value) => trackNode.RunActionsAsync(new EaseBackOut(new ScaleTo(0, 1, value, 1)));

        public Track()
        {
            color = new Color(1f, 1f, 1f);
            ReceiveSceneUpdates = true;
        }

        public override void OnAttachedToNode(Node node)
        {
            trackNode = node.CreateChild();
            trackNode.Position = new Vector3(0, 0, 0);
            trackNode.SetScale(0.5f);            

            var starModel = trackNode.CreateComponent<StaticModel>();
            var cache = Application.ResourceCache;
            starModel.Model = cache.GetModel("Models/Track.mdl");
            starModel.SetMaterial(cache.GetMaterial("Materials/Track.xml"));

            var lightNode = node.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.Range = 10;
            light.Brightness = 10f;
            lightNode.Position = new Vector3(0, 0, -10);

            base.OnAttachedToNode(node);
        }        
    }
}
