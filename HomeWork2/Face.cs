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
        string personGroupId = "group1";
        string personName = "Idol";


        public async Task MainAsync(string[] args)
        {

            if (!await CreatePerson(personGroupId, personName)) { return; }

            string filePath = @"img\seq103.png";
            await IdentityPerson(filePath);

        }

        private async Task<bool> CreatePerson(string personGroupId, string personName)
        {
            try
            {
                //PersonGroup[] personGroups = await faceServiceClient.ListPersonGroupsAsync();
                //if (personGroups != null && personGroups.Length > 0)
                //{
                //    var b = Array.Exists(personGroups, "red");
                //    if (personGroups..Contains(personGroupId))
                //}

                PersonGroup personGroup = await faceServiceClient.GetPersonGroupAsync(personGroupId);
                if (personGroup == null)
                {
                    // Create an empty person group
                    await faceServiceClient.CreatePersonGroupAsync(personGroupId, "Group " + personGroupId);

                }

                // Define Idol
                CreatePersonResult idol = await faceServiceClient.CreatePersonAsync(
                                    // Id of the person group that the person belonged to
                                    personGroupId,
                                    // Name of the person
                                    personName
                                );
                // Directory contains image files of Idol
                string personImageDir = @"img\" + personName;

                foreach (string imagePath in Directory.GetFiles(personImageDir, "*.png"))
                {
                    using (Stream s = File.OpenRead(imagePath))
                    {
                        // Detect faces in the image and add to Idol
                        await faceServiceClient.AddPersonFaceAsync(
                            personGroupId, idol.PersonId, s);
                    }
                }
                //Train the person group
                await faceServiceClient.TrainPersonGroupAsync(personGroupId);
                TrainingStatus trainingStatus = null;
                while (true)
                {
                    trainingStatus = await faceServiceClient.GetPersonGroupTrainingStatusAsync(personGroupId);

                    if (!trainingStatus.Status.Equals("running"))
                    {
                        break;
                    }

                    await Task.Delay(1000);
                }


                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return false;
            }

        }



        private async Task IdentityPerson(string imageFile)
        {
            System.Diagnostics.Debug.WriteLine("File {0}", imageFile);
            using (Stream s = File.OpenRead(imageFile))
            {
                var faces = await faceServiceClient.DetectAsync(s);
                var faceIds = faces.Select(face => face.FaceId).ToArray();

                var results = await faceServiceClient.IdentifyAsync(personGroupId, faceIds);
                foreach (var identifyResult in results)
                {
                    System.Diagnostics.Debug.WriteLine("Result of face: {0}", identifyResult.FaceId);
                    if (identifyResult.Candidates.Length == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("No one identified");
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = await faceServiceClient.GetPersonAsync(personGroupId, candidateId);
                        System.Diagnostics.Debug.WriteLine("Identified as {0}", person.Name);
                    }
                }
            }

        }
    }
}
