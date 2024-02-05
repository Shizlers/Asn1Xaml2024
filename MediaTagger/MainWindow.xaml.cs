using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaTagger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TagLib.File currentFile;
        string ArtistMover;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            try { 
                //instantiating an OpenFileDialog
                OpenFileDialog fileDlg = new OpenFileDialog();

                //Create a file filter / shows mp3 only is search but fails?
                fileDlg.Filter = //"MP3 files (*.MP3)|*.MP3 | " +
                    "All files (*.*)|*.*";

                //ShowDialog shows onscreen for the user
                //By default it return true if the user selects a file and hits "Open"
                if (fileDlg.ShowDialog() == true)
                {
                    //The filename property stores the full path that was selected
                    fileNameBox.Text = fileDlg.FileName;

                    //Example of creating a TagLib file object, for accessing mp3 metadata
                    currentFile = TagLib.File.Create(fileDlg.FileName);

                    //Set the source of the media player element.
                    myMediaPlayer.Source = new Uri(fileDlg.FileName);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //Play
        private void PlayCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = currentFile != null;
        }

        private void PlayExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaPlayer.LoadedBehavior = MediaState.Manual;

            myMediaPlayer.Play();
        }
        //Pause
        private void PauseCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //This exists but makes the pause button behave weird
            e.CanExecute = //myMediaPlayer.CanPause && 
                currentFile != null;
        }

        private void PauseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaPlayer.LoadedBehavior = MediaState.Manual;

            myMediaPlayer.Pause();
        }
        //Open
        private void OpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = currentFile != null;
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //reading tag data from currently selected file.
            try
            {
                //Get data
                var title = currentFile.Tag.Title;
                var artist = currentFile.Tag.AlbumArtists;
                var album = currentFile.Tag.Album;
                var year = currentFile.Tag.Year;

                //Place data
                TITLE.Text = title;
                ArtistMover = "";
                for (int i = 0; i < artist.Length; i++) {
                    if (i != 0) { ArtistMover += ", "; }
                    ArtistMover += artist[i];
                }
                
                ARTIST.Text = ArtistMover; //seems to be an array, concat into a single string first


                ALBUM.Text = album;
                YEAR.Text = year.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //Save
        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = currentFile != null;
        }
        
        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //TODO needs fixing up, seems to not work
            try { 
                ////todo: Saves locally, make write to main

                currentFile.Tag.Title = TITLE.Text;
                
                //Need to make a array first (maybe also check for commas)

                currentFile.Tag.AlbumArtists = ARTIST.Text.Split(", "); // works, needs to become array, did

                currentFile.Tag.Album = ALBUM.Text;

                //convert string to uint
                currentFile.Tag.Year = UInt32.Parse(YEAR.Text, NumberStyles.Integer);//Works
                //currentFile.Tag.Year = uint.Parse(YEAR.Text, System.Globalization.NumberStyles.HexNumber);//wrong 
                //currentFile.Tag.Year = Convert.ToUInt32(YEAR.Text, 16); //wrong number
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}