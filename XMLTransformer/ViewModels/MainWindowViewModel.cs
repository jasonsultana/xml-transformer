using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;

namespace XMLTransformer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _sourceFileUrl = "";
        
        [NotNull]
        public string SourceFileUrl
        {
            get => _sourceFileUrl;
            set => this.RaiseAndSetIfChanged(ref _sourceFileUrl, value);
        }

        private string _transformFileUrl = "";

        [NotNull]
        public string TransformFileUrl
        {
            get => _transformFileUrl;
            set => _transformFileUrl = this.RaiseAndSetIfChanged(ref _transformFileUrl, value);
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