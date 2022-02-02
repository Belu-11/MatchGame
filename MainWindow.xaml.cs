using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            /*
             * List of eight pairs of emoji
             */
            List<string> animalEmoji = new List<string>()
            {
                "🐈","🐈",
                "🦏","🦏",
                "🦔","🦔",
                "🐉","🐉",
                "🦕","🦕",
                "🐳","🐳",
                "🦉","🦉",
                "🦚","🦚",
            };
            /*
             * Create a new random number generator
             */
            Random random = new Random();

            /*
             * Find every TextBlock in the main grid and 
             * repeat the following statements for each of them
             */
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    /*
                     * Pick a random number between 0 and the number of emoji 
                     * left in the list and call it "index"
                     */
                    int index = random.Next(animalEmoji.Count);
                    /*
                     * Use the random number called "index" to get a random emoji from the list"
                     */
                    string nextEmoji = animalEmoji[index];
                    /*
                     * Update the TextBlock with the random emoji from the list
                     */
                    textBlock.Text = nextEmoji;
                    /*
                     * Remove the random emoji from the list
                     */
                    animalEmoji.RemoveAt(index);
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        /*
        * if it's the first in the pair being clicked, keep track of 
        * which TextBlock was clicked and make the animal disapear.
        * If it's the second one, either make it disapear 
        * (if it's a match) or bring back the first one
        * (if it's not).
        * 
        * Event Caller: Method that the app calls in response to an event,
        * like a mouse click, keypress or window resize.
        */
        TextBlock lastTextBlockClicked; //field
        /*
         * Keeps track of whether or not the player just clicked on 
         * the first animal in a pair and is now trying to find its match
         */
        bool findingMatch = false;//field
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

            TextBlock textBlock = sender as TextBlock;
            /*
             * The player just clicked the first animal in a pair, so it makes
             * that naimal invisible and keeps track of its TextBlock in case 
             * it needs to make it visible again.
             */
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            /*
             * The player found a match! So it makes the second animal in the pair invisible
             * (and unclickable) too, and resets findingMatch so the next animal clicked on
             * is the first one in a pair again.
             */
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            /*
             * the player clicked on an animal that doesn't match, so it makes the first animal 
             * that was clicked visible again and resets findingMatch.
             */
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
