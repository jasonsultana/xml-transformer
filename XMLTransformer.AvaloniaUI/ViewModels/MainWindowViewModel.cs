using JetBrains.Annotations;
using ReactiveUI;

namespace XMLTransformer.AvaloniaUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _sourceFileUrl = "";
        private string _transformFileUrl = "";
        private string _transformFileUrl2 = "";
        
        [NotNull]
        public string SourceFileUrl
        {
            get => _sourceFileUrl;
            set => this.RaiseAndSetIfChanged(ref _sourceFileUrl, value);
        }
        
        [NotNull]
        public string TransformFileUrl
        {
            get => _transformFileUrl;
            set => _transformFileUrl = this.RaiseAndSetIfChanged(ref _transformFileUrl, value);
        }

        [NotNull]
        public string TransformFileUrl2
        {
            get => _transformFileUrl2;
            set => _transformFileUrl2 = this.RaiseAndSetIfChanged(ref _transformFileUrl2, value);
        }

        public bool IsPreviewEnabled => !string.IsNullOrWhiteSpace(SourceFileUrl)
                                        && !string.IsNullOrWhiteSpace(TransformFileUrl);

        public MainWindowViewModel()
        {
            this.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TransformFileUrl) ||
                    args.PropertyName == nameof(SourceFileUrl))
                {
                    this.RaisePropertyChanged(nameof(IsPreviewEnabled));
                }
            };
        }
    }
}