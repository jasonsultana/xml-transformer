using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using XMLTransformer.Services;
using XMLTransformer.ViewModels;

namespace XMLTransformer.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private string _sourceBackup = string.Empty;
        
        private MainWindowViewModel ViewModel => this.DataContext as MainWindowViewModel;
            
        private async void BrowseSourceFile_Clicked(object? sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                AllowMultiple = false
            };
            dialog.Filters.Add(new FileDialogFilter() { Name = "XML Config file", Extensions =  { "xml", "config" } });

            string[] result = await dialog.ShowAsync(this);
            if (result.Any())
            {
                ((MainWindowViewModel) this.DataContext).SourceFileUrl = result.First();
            }
        }

        private async void BrowseTransformFile_Clicked(object? sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                AllowMultiple = false
            };
            dialog.Filters.Add(new FileDialogFilter() { Name = "XML Transform file", Extensions =  { "xml", "config" } });

            string[] result = await dialog.ShowAsync(this);
            if (result.Any())
            {
                ((MainWindowViewModel) this.DataContext).TransformFileUrl = result.First();
            }
        }

        private async void Preview_Clicked(object? sender, RoutedEventArgs e)
        {
            var sourceXml = await File.ReadAllTextAsync(ViewModel.SourceFileUrl);
            var transformXml = await File.ReadAllTextAsync(ViewModel.TransformFileUrl);
            var transformedXml = new XmlTransformService().Transform(sourceXml, transformXml);
            var tempFileUrl = Path.GetTempPath() + Path.DirectorySeparatorChar + "temp.xml";

            await File.WriteAllTextAsync(tempFileUrl, transformedXml);
            
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = tempFileUrl,
                UseShellExecute = true
            };
            Process.Start (psi);
        }

        private async void Transform_Clicked(object? sender, RoutedEventArgs e)
        {
            var sourceXml = await File.ReadAllTextAsync(ViewModel.SourceFileUrl);
            var transformXml = await File.ReadAllTextAsync(ViewModel.TransformFileUrl);
            var transformedXml = new XmlTransformService().Transform(sourceXml, transformXml);
            
            _sourceBackup = new string(sourceXml);

            await File.WriteAllTextAsync(ViewModel.SourceFileUrl, transformedXml);
        }

        private async void Revert_Clicked(object? sender, RoutedEventArgs e)
        {
            await File.WriteAllTextAsync(ViewModel.SourceFileUrl, _sourceBackup);
        }
    }
}