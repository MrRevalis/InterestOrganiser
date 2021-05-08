using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace InterestOrganiser.ViewModels
{
    [QueryProperty("Url", "url")]
    public class PlayerViewModel : BaseViewModel
    {
        private string url;
        public string Url
        {
            get => url;
            set => SetProperty(ref url, value);
        }

        private MediaSource streamVideo;
        public MediaSource StreamVideo
        {
            get => streamVideo;
            set => SetProperty(ref streamVideo, value);
        }

        public ICommand MediaOpenedCommand { get; private set; }
        public ICommand MediaFailerCommand { get; private set; }
        public ICommand MediaEndedCommand { get; private set; }
        public ICommand AppearingCommand { get; private set; }
        public ICommand CloseButton { get; private set; }

        public PlayerViewModel()
        {
            AppearingCommand = new Command(async () => await OnAppearing());
        }

        private async Task OnAppearing()
        {
            if (String.IsNullOrEmpty(Url))
                return;
            IsBusy = true;
            string link = "https://www.youtube.com/watch?v=" + Url;
            YoutubeClient youtube = new YoutubeClient();
            Video video = await youtube.Videos.GetAsync(link);

            StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(link);
            IVideoStreamInfo streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

            if(streamInfo != null)
            {
                Stream stream = await youtube.Videos.Streams.GetAsync(streamInfo);
                StreamVideo = streamInfo.Url;
            }
            IsBusy = false;
        }
    }
}
