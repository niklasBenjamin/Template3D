using Godot;

namespace Utilities
{
    public static class FileManager 
    {
        public static object LoadJSON(string path) {
            File file = new File();
            object result = null;

            if(file.FileExists(path) && file.Open(path, File.ModeFlags.Read) == Error.Ok) {
                result = JSON.Parse(file.GetAsText()).Result;
            }
            file.Close();
            return result;
        }
        
        public static bool SaveJSON(string path, object data) {
            var file = new File();

            if(file.Open(path, File.ModeFlags.Write) == Error.Ok){
                file.StoreString(JSON.Print(data));
                file.Close();
                return true;
            }
            file.Close();
            return false;
        }
    }
}
