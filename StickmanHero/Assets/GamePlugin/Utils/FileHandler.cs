using System.IO;
using UnityEngine;

namespace mygame.sdk
{
    public class FileHandler
    {
        public static void writeBytes(string path, byte[] data)
        {
            Debug.Log("mysdk: writeBytes path=" + path);
#if UNITY_WP8
			FileStream fileStream = new FileStream (path, FileMode.Create);
			fileStream.Write (data, 0, data.Length);
				
			fileStream.Close ();
			fileStream.Dispose ();
#else
            File.WriteAllBytes(path, data);
#endif
        }

        public static byte[] readBytes(string path)
        {
            Debug.Log("mysdk: readBytes path=" + path);
#if UNITY_WP8
			byte[] buffer;
			FileStream fileStream = new FileStream (path, FileMode.Open, FileAccess.Read);

			try
			{
				int length = (int)fileStream.Length;  // get file length
				buffer = new byte[length];            // create buffer
				int count;                            // actual number of bytes read
				int sum = 0;                          // total number of bytes read
				
				// read until Read method returns 0 (end of the stream has been reached)
				while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
					sum += count;  // sum is a buffer offset for next reading
			}
			finally
			{
				fileStream.Close();
			}
			return buffer;
#else
            return File.ReadAllBytes(path);
#endif
        }

        public static bool isExist(string path)
        {
            return File.Exists(path);
        }

        public static void delete(string path)
        {
            File.Delete(path);
        }
    }
}