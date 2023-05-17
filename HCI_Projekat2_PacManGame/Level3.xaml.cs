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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HCI_Projekat2_PacManGame
{
    /// <summary>
    /// Interaction logic for Level3.xaml
    /// </summary>
    public partial class Level3 : Window
    {
        int score;
        int scoreOfLevel;
        int speedOfMoving = 6;
        bool left, right, up, down;
        bool wallLeft, wallRight, wallUp, wallDown;

        DispatcherTimer timer = new DispatcherTimer();

        Rect currentPosition = new Rect();
        //ghost settings 
        int ghostSpeed = 6;
        int ghostMoveInPixel = 80;
        int currentGhostStep;

        public Level3(int lastScore)
        {
            InitializeComponent();
            score = lastScore;
            startGame();
        }

        private void keyDown_Action(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && wallLeft == false)
            {
                right = up = down = false;
                wallRight = wallUp = wallDown = false;

                left = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);

            }
            if (e.Key == Key.Right && wallRight == false)
            {
                left = up = down = false;
                wallLeft = wallUp = wallDown = false;

                right = true;

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);

            }
            if (e.Key == Key.Up && wallUp == false)
            {
                right = left = down = false;
                wallRight = wallLeft = wallDown = false;

                up = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);

            }
            if (e.Key == Key.Down && wallDown == false)
            {
                right = up = left = false;
                wallRight = wallUp = wallLeft = false;

                down = true;

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);

            }


        }

        public void startGame()
        {
            level2Canvas.Focus();
            timer.Tick += gameLoop;
            timer.Interval = TimeSpan.FromMilliseconds(25);
            timer.Start();
            currentGhostStep = ghostMoveInPixel; // duh trenutno se pomjera za 50

            ImageBrush pacmanImage = new ImageBrush();
            BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg")); ;
            pacmanImage.ImageSource = image;
            pacman.Fill = pacmanImage;

            ImageBrush redGhostImage = new ImageBrush();
            redGhostImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/redGhost.jpg"));

            redGhost.Fill = redGhostImage;

            ImageBrush yellowGhostImage = new ImageBrush();
            yellowGhostImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/yellowGhost.jpg"));
            yellowGhost.Fill = yellowGhostImage;

           
            yellowGhost2.Fill = yellowGhostImage;



        }

        private void gameLoop(object sender, EventArgs e)
        {

            scoreText.Content = "Score:" + score;
            if (right)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speedOfMoving);
            }
            if (left)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speedOfMoving);
            }
            if (up)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speedOfMoving);
            }
            if (down)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speedOfMoving);
            }

            if (down && Canvas.GetTop(pacman) + 90 > Application.Current.MainWindow.Height)
            {
                wallDown = true;
                down = false;
            }

            if (up && Canvas.GetTop(pacman) < 1)
            {
                wallUp = true;
                up = false;
            }

            if (left && Canvas.GetLeft(pacman) < 1)
            {
                wallLeft = true;
                left = false;
            }

            if (right && Canvas.GetLeft(pacman) + 90 > Application.Current.MainWindow.Width)
            {
                wallRight = true;
                right = false;
            }

            currentPosition = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            foreach (var x in level2Canvas.Children.OfType<Rectangle>())
            {


                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); // create a new rect called hit box for all of the available rectangles inside of the game


                if ((string)x.Tag == "wall")
                {

                    if (left == true && currentPosition.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        wallLeft = true;
                        left = false;
                    }

                    if (right == true && currentPosition.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        wallRight = true;
                        right = false;
                    }

                    if (down == true && currentPosition.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        wallDown = true;
                        down = false;
                    }

                    if (up == true && currentPosition.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        wallUp = true;
                        up = false;
                    }
                }

                if ((string)x.Tag == "gold")
                {

                    if (currentPosition.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {

                        x.Visibility = Visibility.Hidden; // pokupi se zlatnik
                        scoreOfLevel++;
                        score++;

                    }
                }


                if ((string)x.Tag == "ghost")
                {

                    if (currentPosition.IntersectsWith(hitBox))
                    {

                        gameOver("Ghosts got you!");
                    }


                    if (x.Name.ToString() == "yellowGhost" || x.Name.ToString()=="yellowGhost2" )
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);

                    }
                    else
                    {

                        Canvas.SetTop(x, Canvas.GetTop(x) + ghostSpeed);


                    }


                    currentGhostStep--;


                    if (currentGhostStep < 1)
                    {

                        currentGhostStep = ghostMoveInPixel;

                        ghostSpeed = -ghostSpeed;
                    }
                }
            }



            if (scoreOfLevel == 43) //43
            {
                timer.Stop();
                CustomMessageBox customMessageBox = new CustomMessageBox("Congratulations, you have passed this level!", score);
                customMessageBox.Show();
                this.Close();

            }


        }

        public void gameOver(string text)
        {
            timer.Stop();
            CustomMessageBox customMessageBox = new CustomMessageBox(text, score);
            customMessageBox.Show();
            this.Close();


        }
        public void levelPassed(string text)
        {

            MessageBox.Show(text, "Information", MessageBoxButton.OK, MessageBoxImage.Information);


        }
    }
}
