using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Emotion;


namespace HomeWork2
{
    class Program
    {

        // https://www.microsoft.com/cognitive-services/en-us/face-api/documentation/face-api-how-to-topics/howtoidentifyfacesinimage
        static void Main(string[] args)
        {
            Face face = new Face();
            Task t = face.MainAsync(args);
            t.Wait();
            System.Diagnostics.Debug.WriteLine("End.");
        }

    }
}
