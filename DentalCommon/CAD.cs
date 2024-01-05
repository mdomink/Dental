using System.IO;
using System.Web;
//using Rhino.FileIO;
//using glTFLoader;

//namespace DentalCommon
//{
//    public static class CAD
//    {
//        public static void ConvertCADtoGLTF(HttpPostedFileBase cadFile)
//        {
//            // Provjera da li je datoteka postavljena
//            if (cadFile != null && cadFile.ContentLength > 0)
//            {
//                // Dobavljanje staze do privremene datoteke
//                var tempFilePath = Path.GetTempFileName();

//                // Spremanje CAD datoteke na disk
//                cadFile.SaveAs(tempFilePath);

//                // Konverzija CAD datoteke u GLTF format
//                ConvertToGLTF(tempFilePath);

//                // Brišemo privremenu datoteku
//                File.Delete(tempFilePath);
//            }
//        }

//    }
//}