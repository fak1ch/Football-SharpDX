using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.CustomUnity.Utils
{
    public static class Converter
    {
        public static string Vector3ToString(Vector3 vector3)
        {
            float x = vector3.X;
            float y = vector3.Y;
            float z = vector3.Z;

            return $"{x}.{y}.{z}";
        }

        public static Vector3 StringToVector3(string message)
        {
            string[] result = message.Split('.');

            float x = float.Parse(result[0]);
            float y = float.Parse(result[1]);
            float z = float.Parse(result[2]);

            return new Vector3(x, y, z);
        }
    }
}
