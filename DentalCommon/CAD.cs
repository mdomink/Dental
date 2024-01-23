//using System.IO;
//using System.Web;
//using Microsoft.AspNetCore.Http;
//using Rhino.FileIO;
//using Rhino.Geometry;
//using SharpGLTF;
//using SharpGLTF.Schema2;

//namespace DentalCommon
//{
//    public static class CAD
//    {
//        public static void ConvertCADtoGLTF(IFormFile cadFile)
//        {
//            // Provjera da li je datoteka postavljena
//            if (cadFile != null && cadFile.Length > 0)
//            {
//                // Load CAD file (Rhino 3D format in this example)
//                var doc = File3dm.Read(cadFile);

//                // Create a new GLTF model
//                ModelRoot gltfModel = ModelRoot.CreateModel();

//                // Iterate through CAD model and add relevant information to GLTF model
//                foreach (var obj in doc.Objects)
//                {
//                    if (obj.Geometry is Brep brep)
//                    {
//                        // Convert Brep to Mesh or add other geometry information to GLTF model
//                        // ...

//                        // Example: Convert Brep to Mesh (simplified)
//                        var mesh = brep.ToMesh(MeshType.Render);
//                        var node = gltfModel.LogicalNodes().CreateNode();
//                        node.Mesh = gltfModel.CreateMeshBuilder().AddTrianglePrimitives(mesh.Vertices, mesh.Faces).ToMesh();
//                    }
//                }

//                // Save GLTF model to file
//                gltfModel.SaveGLTF("path/to/your/output.gltf");
//            }
//        }

//        public async static void SaveFileAsync(IFormFile file)
//        {
//            if (file.Length > 0)
//            {
//                string filePath = Path.Combine(Path.GetTempFileName(), file.FileName);
//                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
//                {
//                    await file.CopyToAsync(fileStream);
//                }               
//            }           
//        }

//        private static void ConvertToGLTF(string cadFilePath)
//        {
//            // Učitavanje CAD datoteke pomoću Rhino3dm
//            var doc = File3dm.Read(cadFilePath);

//            // Postavljanje postavki za izvoz GLTF
//            var gltfOptions = new glTFLoaderWriter.Options
//            {
//                SeparateBuffers = false // Postavke prema vašim potrebama
//            };

//            // Stvaranje izvozne instancije
//            var gltfWriter = new glTFLoaderWriter.GltfWriter(gltfOptions);

//            // Konverzija CAD modela u GLTF
//            gltfWriter.Write(doc, "output.gltf");

//            // Alternativno, možete koristiti GLB format ako je potrebno
//            // gltfWriter.Write(doc, "output.glb");
//        }
//    }
//}