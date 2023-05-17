using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Shapes;

namespace HCI_Projekat2_PacManGame
{
    /// <summary>
    /// Interaction logic for SaveScoreWindow.xaml
    /// </summary>
    public partial class SaveScoreWindow : Window
    {
        public int score;
        public SaveScoreWindow(int lastScore)
        {
            InitializeComponent();
            score = lastScore;
            scoreLabel.Content = "Score:" + score;
         
            listViewSetting();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(textBox.Text!="")
            {
                SaveScore();
            }
            else
            {
                infoLabel.Content = "Write your username to save score!";
            }

        }
        private void listViewSetting()
        {
            String scoresFilePath = "C:/Users/HP/source/repos/HCI_Projekat2_PacManGame/scores.txt";

            try
            {
                listView.Items.Clear();
               
                List<User> scores = new List<User>();
              



                using (StreamReader streamReader = new StreamReader(scoresFilePath))
                {
                    streamReader.ReadLine();

                    while (!streamReader.EndOfStream)
                    { 
                         string[] row = streamReader.ReadLine().Split(',');
                         scores.Add(new User { Name = row[0], Score = Convert.ToInt32(row[1]) }); 
                         listView.Items.Add(new User { Name = row[0], Score = Convert.ToInt32(row[1])});



                    }

                }
             
            }catch(Exception ex) { Console.WriteLine(ex.StackTrace); }
        }

                private void SaveScore()
                {
                    String scoresFilePath = "C:/Users/HP/source/repos/HCI_Projekat2_PacManGame/scores.txt";

                    try
                    {

                        listView.Items.Clear();
                        if (!File.Exists(scoresFilePath))
                        {
                            File.Create(scoresFilePath).Dispose();
                        }

                        List<User> scores=new List<User>();
               
                        using (StreamReader streamReader = new StreamReader(scoresFilePath))
                        {
                            streamReader.ReadLine();

                            while (!streamReader.EndOfStream)
                            {
                                string[] row = streamReader.ReadLine().Split(',');
                                scores.Add(new User { Name = row[0], Score = Convert.ToInt32(row[1]) });
                                listView.Items.Add(new User { Name = row[0], Score = Convert.ToInt32(row[1]) });



                    }
                }

                        Boolean scoreFound = false;

             
                        foreach (User user in scores)
                        {

                            if (user.Name.Equals(textBox.Text))
                            {
                                if (user.Score < score)
                                {
                                     user.Score = score;
                                }

                                scoreFound = true;
                                break;
                            }
                        }

                        if (!scoreFound)
                        {
                            User user = new User();
                            user.Name = textBox.Text;
                            user.Score = score;
                            scores.Add(user);
                            listView.Items.Add(user);


                         }


                File.WriteAllText(scoresFilePath, string.Empty);

                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("name,score");

                        foreach (User user in scores)
                        {
                            stringBuilder.AppendLine(user.Name + "," + user.Score);
                        }

                        File.WriteAllText(scoresFilePath, stringBuilder.ToString());
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving high score:\n\n" + ex.ToString(), "Error");
                    }
                }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            this.Close();
        }
    }
}
