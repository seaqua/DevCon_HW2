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


        static void Main(string[] args)
        {
            Face face = new Face();
            Task t = face.MainAsync(args);
            t.Wait();
            System.Diagnostics.Debug.WriteLine("End.");
        }

    }
}
