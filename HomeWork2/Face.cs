using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Face.Contract;
using System.IO;
using Microsoft.ProjectOxford.Face;

namespace HomeWork2
{
    class Face
    {
        IFaceServiceClient faceServiceClient = new FaceServiceClient("6df4db950c2243dc99d413d4de205f03");

        public async Task MainAsync(string[] args)
        {
            string filePath = "img\\seq103.png";

            FaceRectangle[] faceRects = await UploadAndDetectFaces(filePath);
            if (faceRects != null)
            {
                System.Diagnostics.Debug.WriteLine(faceRects);

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Face is not detected.");
            }
        }

        private async Task<FaceRectangle[]> UploadAndDetectFaces(string imageFilePath)
        {
            try
            {
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    var faces = await faceServiceClient.DetectAsync(imageFileStream);
                    var faceRects = faces.Select(face => face.FaceRectangle);
                    return faceRects.ToArray();
                }
            }
            catch (Exception)
            {
                return new FaceRectangle[0];
            }
        }
    }
}
