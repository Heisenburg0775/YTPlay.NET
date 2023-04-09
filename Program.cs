using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Diagnostics;
class Program
{
    static void Main(string[] args)
    {
        string apiKey = "";
        Console.WriteLine("Please give an arguement to search");
        string Args = Console.ReadLine();
        SearchResource.ListRequest searchRequest = new SearchResource.ListRequest(new YouTubeService(new BaseClientService.Initializer { ApiKey = apiKey }), "snippet");
        searchRequest.Q = Args;
        searchRequest.Type = "video";
        searchRequest.MaxResults = 1;
        SearchListResponse searchResponse = searchRequest.Execute();
        string videoId = searchResponse.Items[0].Id.VideoId;
        string videoURL = "https://www.youtube.com/watch?v=" + videoId;
        ProcessStartInfo YTDlp = new ProcessStartInfo();
        YTDlp.FileName = "yt-dpl.exe";
        YTDlp.Arguments = $"-f best --get-url {videoURL} --no-warning";
        YTDlp.UseShellExecute = false;
        YTDlp.RedirectStandardOutput = true;

        using (Process process = Process.Start(YTDlp))
        {
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            string streamURL = output.Trim();
            ProcessStartInfo FFplay = new ProcessStartInfo();
            FFplay.FileName = "ffplay";
            FFplay.Arguments = $"-autoexit \"{streamURL}\"";
            FFplay.UseShellExecute = false;
            FFplay.RedirectStandardOutput = true;
            Process ff_process = Process.Start(FFplay);
        }
    }
    
}

