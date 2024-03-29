using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TestingMVVM
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {   
        private string? text;
        public event PropertyChangedEventHandler? PropertyChanged;

        // Function to generate Texts
        private List<string> WritingTexts()
        {
            List<string> _writingTexts = new List<string>
            {
                "The sun was setting behind the mountains, casting a warm glow over the valley below. Birds chirped happily in the trees as a gentle breeze rustled the leaves.",
                "She walked along the cobblestone path, admiring the quaint shops and colorful flowers that lined the street. The smell of freshly baked bread wafted through the air.",
                "In the heart of the forest, a hidden waterfall cascaded down into a crystal-clear pool. The sound of rushing water echoed through the trees, creating a sense of tranquility.",
                "As the train sped along the tracks, she watched the world outside blur into a kaleidoscope of colors. Each passing moment was fleeting yet filled with possibility.",
                "Underneath the starry sky, they danced in the moonlight, their laughter mingling with the sound of waves crashing against the shore. Time seemed to stand still in that moment."
            };
            return _writingTexts;
        }

        public MainWindow()
        {
            DataContext = this;
            text = WritingMaterial;
            InitializeComponent();
            InitializeTimer();
            txtBox.Focus();
            TextChange();

        }
        public void TextChange()
        {
            if (curIndex.Equals(0))
            {   
                TextPart1 = null;
                TextPart2 = text[curIndex].ToString();
                TextPart3 = text.Substring(curIndex + 1).ToString();
            }
            else
            {
            TextPart1 = text.Substring(0, curIndex+1).ToString();
            TextPart2 = text[curIndex + 1].ToString();
            TextPart3 = text.Substring(curIndex + 2).ToString();
            }
        }

        // Function To Randomly choose a text
        public string ChooseRandomText()
        {
            List<string> textsList = WritingTexts();
            Random random = new Random();
            int index = random.Next(0,textsList.Count);
            return textsList[index];
        }

        // Filling text field with a random data of Words
        public string WritingMaterial {
            get
            {
                text = ChooseRandomText();
                return text;
            }
            }

        private int curIndex = 0;
        private string? txtChanged;
        public string TextChanged
        {
            get { return txtChanged; }
            set
            {
                txtChanged = value;
                
                if (txtChanged.Length > 0)
                {
                    if (!_isTimerRunning)
                    {
                        timer.Start();
                        _isTimerRunning = true;
                    }

                }

                if (text.StartsWith(txtChanged))
                {
                    txtBox.MaxLength = text.Length;
                    OnPropertyChange();
                }
                else
                {
                    txtBox.MaxLength = txtChanged.Length;
                }
                curIndex = txtChanged.Length - 1;
                if (curIndex == text.Length - 1)
                {
                    timer.Stop();
                    _isTimerRunning = false;
                    ShowMessageBox($"Congratulations !! Your Typing Speed was: {text.Length / _seconds} chars/sec", "Congratulations");
                }
                else
                {
                    TextChange();
                }
            }
        }

        // Implementing the TextPart
        private string txtPart1;
        private string txtPart2;
        private string txtPart3;

        public string TextPart1
        {
            get { 

                return txtPart1; 
            }
            set {
                txtPart1 = value;
                OnPropertyChange();
            }
        }

        public string TextPart2
        {
            get
            {
                return txtPart2;
            }
            set {
                txtPart2 = value;
                OnPropertyChange();
            }
        }

        public string TextPart3
        {
            get
            {
                return txtPart3;
            }
            set {
                txtPart3 = value;
                OnPropertyChange();}
        }



        // For Implementing the Timer
        private DispatcherTimer timer;
        private int _seconds = 1;
        private bool _isTimerRunning = false;

        private void InitializeTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
        }

        public int Seconds
        {
            get { return _seconds; }
            set 
            {   
                if (_seconds.Equals(40)) {
                    timer.Stop();
                    _isTimerRunning = false;
                }
                _seconds = value;
                OnPropertyChange();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Seconds++;
        }

        // Reset Button
        private void BtnReset(object sender,RoutedEventArgs e)
        {
            _ = WritingMaterial;
            TextChanged = "";
            Seconds = 1;
            timer.Stop();
        }


        // Message Box
        private void ShowMessageBox(string textToDisplay,string Title)
        {
            MessageBoxResult result = MessageBox.Show(textToDisplay, Title, MessageBoxButton.OK);
            if (result == MessageBoxResult.OK)
            {
                // Close the window or handle other actions
                this.Close();
            }
        }

        // I Notify PropertyChange
        public void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
