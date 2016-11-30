using System;
using Urho;

namespace StartTrack
{
    public static class TrigonometryHelper
    {
        public static float GetDistance(Vector3 a, Vector3 b)
        {
            var height = a.Y-b.Y;

            var width = a.X - b.X;

            var distance = (float)Math.Sqrt(height*height+width*width);

            return distance;
        }

        public static float GetAngle(Vector3 a, Vector3 b)
        {
            var height = a.Y - b.Y;            
            var width = a.X - b.X;            
            var hypotenuse = (float)Math.Sqrt(height * height + width * width);
            
            var sin = height / hypotenuse;
            var angle=(float)Math.Asin(sin)*57;
            if (a.X < b.X && a.Y != b.Y) angle *= -1;     
            return angle;
        }

        public static Vector3 GetBetween(Vector3 a, Vector3 b)
        {
            var pointX = 0f;
            var pointY = 0f;

            var diffecrence = (a.X - b.X)/2;
            if (diffecrence < 0)
            {
                pointX = a.X - diffecrence;
            }
            else
            {
                pointX = b.X + diffecrence;
            }
            diffecrence = (a.Y - b.Y)/2;
            if (diffecrence < 0)
            {
                pointY = a.Y - diffecrence;
            }
            else
            {
                pointY = b.Y + diffecrence;
            }

            return new Vector3(pointX,pointY,0);
        }
    }
}
