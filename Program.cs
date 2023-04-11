using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        string apiKey = "";
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"
██╗░░░██╗████████╗██████╗░██╗░░░░░░█████╗░██╗░░░██╗░░░███╗░░██╗███████╗████████╗
╚██╗░██╔╝╚══██╔══╝██╔══██╗██║░░░░░██╔══██╗╚██╗░██╔╝░░░████╗░██║██╔════╝╚══██╔══╝
░╚████╔╝░░░░██║░░░██████╔╝██║░░░░░███████║░╚████╔╝░░░░██╔██╗██║█████╗░░░░░██║░░░
░░╚██╔╝░░░░░██║░░░██╔═══╝░██║░░░░░██╔══██║░░╚██╔╝░░░░░██║╚████║██╔══╝░░░░░██║░░░
░░░██║░░░░░░██║░░░██║░░░░░███████╗██║░░██║░░░██║░░░██╗██║░╚███║███████╗░░░██║░░░
░░░╚═╝░░░░░░╚═╝░░░╚═╝░░░░░╚══════╝╚═╝░░╚═╝░░░╚═╝░░░╚═╝╚═╝░░╚══╝╚══════╝░░░╚═╝░░░
        ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Please provide method: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("1) Download");
        Console.WriteLine("2) Play");
        string method = Console.ReadLine();
        Console.Clear();
        if (method.ToLower() == "play")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Please provide a string in order to search for a video");
            Console.ResetColor();
            var Args = Console.ReadLine();
            if (string.IsNullOrEmpty(Args))
            {
                Console.Clear();
                Console.WriteLine("Please provide a string");
            }
            else
            {
                SearchResource.ListRequest searchRequest = new SearchResource.ListRequest(new YouTubeService(new BaseClientService.Initializer { ApiKey = apiKey }), "snippet");
                searchRequest.Q = Args;
                searchRequest.Type = "video";
                searchRequest.MaxResults = 1;
                SearchListResponse searchResponse = searchRequest.Execute();
                string videoId = searchResponse.Items[0].Id.VideoId;
                string videoURL = "https://www.youtube.com/watch?v=" + videoId;
                ProcessStartInfo YTDlp = new ProcessStartInfo();
                YTDlp.FileName = "yt-dlp.exe";
                YTDlp.Arguments = $"-f best --get-url {videoURL} --no-warning";
                YTDlp.UseShellExecute = false;
                YTDlp.RedirectStandardOutput = true;

                using (Process process = Process.Start(YTDlp))
                {
                    Console.Clear();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    string streamURL = output.Trim();
                    Console.WriteLine("How do you want to play this video?");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("1)Normal");
                    Console.WriteLine("2)AudioVisualize");
                    Console.ResetColor();
                    var PlayMethod = Console.ReadLine();
                    if (PlayMethod.ToLower() == "normal")
                    {
                        Console.Clear();
                        Process ff_process = new Process();
                        ff_process.StartInfo.FileName = "ffplay";
                        ff_process.StartInfo.Arguments = $"-autoexit -hide_banner \"{streamURL}\"";
                        ff_process.StartInfo.UseShellExecute = false;
                        ff_process.StartInfo.RedirectStandardOutput = true;
                        ff_process.EnableRaisingEvents = true;
                        ff_process.Start();
                        Console.Clear();
                        Console.ResetColor();
                    }
                    else if (PlayMethod.ToLower() == "audiovisualize")
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Choose visualize method: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("1)Bars");
                        Console.WriteLine("2)WavesPoint");
                        Console.WriteLine("3)WavesLine");
                        Console.WriteLine("4)WaveP2p");
                        Console.WriteLine("5)WavesCline");
                        var VisualizeMethod = Console.ReadLine();
                        if (VisualizeMethod.ToLower() == "bars")
                        {
                            Console.Clear();
                            string v = "\\";
                            var modified = streamURL.Insert(5, v);
                            ProcessStartInfo FFBars = new ProcessStartInfo();
                            FFBars.FileName = "ffplay";
                            FFBars.Arguments = $"-hide_banner -sync audio -autoexit  -volume 80 -hide_banner  -f lavfi \"amovie=filename='{modified}',asplit=[out0][out1];[out0]showvolume=r=25:w=1920:h=40:f=0.5:ds=lin:v=0\"";
                            FFBars.UseShellExecute = false;
                            FFBars.RedirectStandardOutput = true;
                            Process bars = Process.Start(FFBars);
                        }
                        else if (VisualizeMethod.ToLower() == "wavespoint")
                        {
                            Console.Clear();
                            string v = "\\";
                            var modified = streamURL.Insert(5, v);
                            ProcessStartInfo FFBars = new ProcessStartInfo();
                            FFBars.FileName = "ffplay";
                            FFBars.Arguments = $"-hide_banner -sync audio -autoexit  -volume 80 -hide_banner  -f lavfi \"amovie=filename='{modified}',asplit=[out0][out1];[out0]showwaves=mode=point:s=hd480:colors=White:r=25\"";
                            FFBars.UseShellExecute = false;
                            FFBars.RedirectStandardOutput = true;
                            Process bars = Process.Start(FFBars);
                        }
                        else if (VisualizeMethod.ToLower() == "wavesline")
                        {
                            Console.Clear();
                            string v = "\\";
                            var modified = streamURL.Insert(5, v);
                            ProcessStartInfo FFBars = new ProcessStartInfo();
                            FFBars.FileName = "ffplay";
                            FFBars.Arguments = $"-hide_banner -sync audio -autoexit  -volume 80 -hide_banner  -f lavfi \"amovie=filename='{modified}',asplit=[out0][out1];[out0]showwaves=mode=line:s=hd480:colors=White:r=25\"";
                            FFBars.UseShellExecute = false;
                            FFBars.RedirectStandardOutput = true;
                            Process bars = Process.Start(FFBars);
                        }
                        else if (VisualizeMethod.ToLower() == "wavesp2p")
                        {
                            Console.Clear();
                            string v = "\\";
                            var modified = streamURL.Insert(5, v);
                            ProcessStartInfo FFBars = new ProcessStartInfo();
                            FFBars.FileName = "ffplay";
                            FFBars.Arguments = $"-hide_banner -sync audio -autoexit  -volume 80 -hide_banner  -f lavfi \"amovie=filename='{modified}',asplit=[out0][out1];[out0]showwaves=mode=p2p:s=hd480:colors=White:r=25\"";
                            FFBars.UseShellExecute = false;
                            FFBars.RedirectStandardOutput = true;
                            Process bars = Process.Start(FFBars);
                        }
                        else if (VisualizeMethod.ToLower() == "wavescline")
                        {
                            Console.Clear();
                            string v = "\\";
                            var modified = streamURL.Insert(5, v);
                            ProcessStartInfo FFBars = new ProcessStartInfo();
                            FFBars.FileName = "ffplay";
                            FFBars.Arguments = $"-hide_banner -sync audio -autoexit  -volume 80 -hide_banner  -f lavfi \"amovie=filename='{modified}',asplit=[out0][out1];[out0]showwaves=mode=cline:s=hd480:colors=White:r=25\"";
                            FFBars.UseShellExecute = false;
                            FFBars.RedirectStandardOutput = true;
                            Process bars = Process.Start(FFBars);
                        }
                        else
                        {
                            Console.WriteLine("Invalid or no input");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid method");
                    }
                }
            }
        }

        else if (method.ToLower() == "download")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Please provide a valid string");
            string Args = Console.ReadLine();
            if(string.IsNullOrEmpty(Args)){
                Console.WriteLine("Please provide a valid string");
            }else{
            SearchResource.ListRequest searchRequest = new SearchResource.ListRequest(new YouTubeService(new BaseClientService.Initializer { ApiKey = apiKey }), "snippet");
            searchRequest.Q = Args;
            searchRequest.Type = "video";
            searchRequest.MaxResults = 1;
            SearchListResponse searchResponse = searchRequest.Execute();
            string videoId = searchResponse.Items[0].Id.VideoId;
            string videoURL_d = "https://www.youtube.com/watch?v=" + videoId;
            var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Downloads\\YTPlay";
            if (!Directory.Exists(path))
            {
                
                Console.Clear();
                Directory.CreateDirectory(path);
                var file = $"{path}";
                var f = file.Replace("\\","/");
                ProcessStartInfo YTDlp_d = new ProcessStartInfo();
                YTDlp_d.FileName = "yt-dlp.exe";
                YTDlp_d.WorkingDirectory = f;
                YTDlp_d.Arguments = $"-f best -o \"%(title)s.%(ext)s\" {videoURL_d} --no-warning";
                YTDlp_d.UseShellExecute = false;
                YTDlp_d.RedirectStandardOutput = false;
                YTDlp_d.RedirectStandardInput = false;
                Process dp = Process.Start(YTDlp_d);
            }else{
                var file = $"{path}";
                var f = file.Replace("\\","/");
                ProcessStartInfo YTDlp_d = new ProcessStartInfo();
                YTDlp_d.FileName = "yt-dlp.exe";
                YTDlp_d.WorkingDirectory = f;
                YTDlp_d.Arguments = $"-f best -o \"%(title)s.%(ext)s\" {videoURL_d} --no-warning";
                YTDlp_d.UseShellExecute = false;
                YTDlp_d.RedirectStandardOutput = false;
                YTDlp_d.RedirectStandardInput = false;
                Process dp = Process.Start(YTDlp_d);
            }
        }
        }
        else
        {
            Console.WriteLine("Please provide a valid method");
        }


    }
}

