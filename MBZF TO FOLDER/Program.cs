using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBZF_TO_FOLDER
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No File given!");
                Console.ReadLine();
                return;
            }

            byte[] bytes = File.ReadAllBytes(args[0]);

            //using (BinaryWriter writer = new BinaryWriter(new FileStream($"{args[0]}.mbzf", FileMode.Create))) // marcel binary zip format
            //{
            //    writer.Write(size); //filesize
            //    writer.Write(files.Length); // amount of files
            //    for (int i = 0; i < files.Length; i++)
            //    {
            //        writer.Write(names[i].Length);
            //        foreach (char c in names[i])
            //            writer.Write((byte)c);

            //        writer.Write(filedata[i].LongLength);
            //        writer.Write(filedata[i]);
            //    }
            //}

            int index = 0;
            long size = BitConverter.ToInt64(bytes, index);
            index += 8;
            int fileCount = BitConverter.ToInt32(bytes, index);
            index += 4;

            Console.WriteLine($"Filesize: {size}");
            Console.WriteLine($"Filecount: {fileCount}");


            string folderPath = Path.GetFileNameWithoutExtension(args[0]);
            Directory.CreateDirectory(folderPath);

            for (int i = 0; i < fileCount; i++)
            {
                Console.WriteLine($"File {i}:");
                int fileNameSize = BitConverter.ToInt32(bytes, index);
                Console.WriteLine($" - Filename size: {fileNameSize} bytes");
                index += 4;
                byte[] filenameByteArr = new byte[fileNameSize];

                for (int i2 = 0; i2 < fileNameSize; i2++)
                    filenameByteArr[i2] = bytes[i2 + index];
                index += fileNameSize;


                string filename = Encoding.UTF8.GetString(filenameByteArr);

                Console.WriteLine($" - Filename: \"{filename}\"");

                long fileSize = BitConverter.ToInt64(bytes, index);
                index += 8;
                Console.WriteLine($" - Filesize: {fileSize} bytes");

                byte[] fileData = new byte[fileSize];
                for (int i2 = 0; i2 < fileSize; i2++)
                    fileData[i2] = bytes[i2 + index];

                index += (int)fileSize;



                using (BinaryWriter writer = new BinaryWriter(new FileStream($"{folderPath}/{filename}", FileMode.Create)))
                {
                    writer.Write(fileData);
                }

            }










            //string[] files = Directory.GetFiles(args[0]);

            //long size = 12;

            //byte[][] filedata = new byte[files.Length][];
            //string[] names = new string[files.Length];
            //{
            //    int i = 0;
            //    foreach (string file in files)
            //    {
            //        string name = Path.GetFileName(file);
            //        size += 4 + name.Length;
            //        names[i] = name;

            //        filedata[i] = File.ReadAllBytes(file);
            //        size += 8 + filedata[i].LongLength;
            //        i++;
            //    }
            //}

            //Console.WriteLine($"Filesize: {size} Bytes.");

            //using (BinaryWriter writer = new BinaryWriter(new FileStream($"{args[0]}.mbzf", FileMode.Create))) // marcel binary zip format
            //{
            //    writer.Write(size); //filesize
            //    writer.Write(files.Length); // amount of files
            //    for (int i = 0; i < files.Length; i++)
            //    {
            //        writer.Write(names[i].Length);
            //        foreach (char c in names[i])
            //            writer.Write((byte)c);

            //        writer.Write(filedata[i].LongLength);
            //        writer.Write(filedata[i]);
            //    }
            //}

            Console.WriteLine("\n\nEnd.");
            Console.ReadLine();
            return;
        }
    }
}
