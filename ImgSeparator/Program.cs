using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using OpenCvSharp;


namespace ImgSeparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string MainDir = "";
            string ResultDir = "";
            string Others = "Others";
            string OK = "OK";
            string Overkill = "Overkill";
            string Error = "Error";

            DirCheck(ResultDir);

            var MainDirs = Directory.GetDirectories(MainDir);
            foreach (var sub_dir in MainDirs)
            {
                Console.WriteLine(sub_dir);
                var files = Directory.GetFiles(sub_dir);
                foreach (var img_file in files)
                {
                    //Console.WriteLine(file);
                    var name = Path.GetFileNameWithoutExtension(img_file);
                    Console.WriteLine(name);

                    var s = name.Split('_');
                    if (s.ToList().Count > 4)
                    {
                        //이미지 폴더 체크
                        var temp_name = new ArraySegment<string>(s, 3, s.ToList().Count - 4);
                        var real_name = string.Join("_", temp_name.ToArray());
                        Console.WriteLine(real_name);
                        var temp_dir = Path.Combine(ResultDir, real_name);
                        var ok_dir = Path.Combine(temp_dir, OK);
                        var overkill_dir = Path.Combine(temp_dir, Overkill);
                        var error_dir = Path.Combine(temp_dir, Error);
                        DirCheck(temp_dir);
                        DirCheck(ok_dir);
                        DirCheck(overkill_dir);
                        DirCheck(error_dir);

                        using (Mat mat = Cv2.ImRead(img_file))
                        {
                            Cv2.ImShow("img", mat);
                            int key = 0;
                            while (key is 0)
                            {
                                key = Cv2.WaitKey();
                                var resultname = Path.GetFileName(img_file);
                                Console.WriteLine(key);
                                if (key == '1')  //Good
                                {
                                    File.Copy(img_file, Path.Combine(ok_dir, resultname));
                                    break;
                                }
                                else if (key == '2') //Overkill
                                {
                                    File.Copy(img_file, Path.Combine(overkill_dir, resultname));
                                    break;
                                }
                                else if (key == '3') //Error
                                {
                                    File.Copy(img_file, Path.Combine(error_dir, resultname));
                                    break;
                                }
                                
                            }
                        }
                    }
                    else
                    {
                        //이미지 폴더 체크
                        DirCheck(Path.Combine(ResultDir, Others));

                    }
                }
            }

        }

        static void DirCheck(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}
