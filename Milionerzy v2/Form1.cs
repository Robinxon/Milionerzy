using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WMPLib;

namespace Milionerzy_v2
{
    public partial class Form1 : Form
    {
        #region Some Additional Stuff
        protected override CreateParams CreateParams
        {
            //teoretycznie ma zapobiegać miganiu przy przerysowywaniu interfejsu
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        #endregion

        #region Declarations
        FormWindowState LastWindowState = FormWindowState.Minimized;
        WindowsMediaPlayer MusicPlayer = new WindowsMediaPlayer();
        List<Question> QuestionEasy = new List<Question>();
        List<Question> QuestionMedium = new List<Question>();
        List<Question> QuestionHard = new List<Question>();
        List<HighScore> HighScore = new List<HighScore>();
        List<HighScore> SortedHighScore = new List<HighScore>();
        Status status = new Status();
        Game game = new Game();
        #endregion

        #region Form
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //wersja programu
            this.Text += " v1.1.0";

            //ustawienie wielkości wewnętrzenej dla okna
            this.ClientSize = new Size(600, 400);

            //usunięcie tła (umieszczone tylko dla czytelności)
            this.BackgroundImage = null;

            //ustawianie parentów
            #region PictureBackgroundMain
            PictureLogo.Parent = PictureBackgroundMain;
            LabelInputName.Parent = PictureBackgroundMain;
            TextBoxPlayerName.Parent = PictureBackgroundMain;
            PictureStartButton.Parent = PictureBackgroundMain;
            #endregion
            #region PictureBackgroundGame
            PictureJoker5050.Parent = PictureBackgroundGame;
            PictureJokerChangeQuestion.Parent = PictureBackgroundGame;
            PictureJokerAskAudience.Parent = PictureBackgroundGame;
            PictureQuestionReward.Parent = PictureBackgroundGame;
            LabelQuestionReward.Parent = PictureQuestionReward;
            PictureQuestion.Parent = PictureBackgroundGame;
            LabelQuestion.Parent = PictureQuestion;
            PictureAnswerA.Parent = PictureBackgroundGame;
            LabelAnswerA.Parent = PictureAnswerA;
            PictureAnswerB.Parent = PictureBackgroundGame;
            LabelAnswerB.Parent = PictureAnswerB;
            PictureAnswerC.Parent = PictureBackgroundGame;
            LabelAnswerC.Parent = PictureAnswerC;
            PictureAnswerD.Parent = PictureBackgroundGame;
            LabelAnswerD.Parent = PictureAnswerD;
            #endregion
            #region PictureBackgroundReward
            LabelPlayerName.Parent = PictureBackgroundReward;
            PictureRewardNow.Parent = PictureBackgroundReward;
            LabelRewardNow.Parent = PictureRewardNow;
            #endregion
            #region PictureBackgroundLeaderboard
            LabelLeaderboardTitle.Parent = PictureBackgroundLeaderboard;
            LabelResult1.Parent = PictureBackgroundLeaderboard;
            LabelResult2.Parent = PictureBackgroundLeaderboard;
            LabelResult3.Parent = PictureBackgroundLeaderboard;
            LabelResult4.Parent = PictureBackgroundLeaderboard;
            LabelResult5.Parent = PictureBackgroundLeaderboard;
            LabelResult6.Parent = PictureBackgroundLeaderboard;
            LabelResult7.Parent = PictureBackgroundLeaderboard;
            LabelResult8.Parent = PictureBackgroundLeaderboard;
            LabelResult9.Parent = PictureBackgroundLeaderboard;
            LabelResult10.Parent = PictureBackgroundLeaderboard;
            #endregion

            //dopasowanie wielkości elementów
            LoadQuestions();
            ResizeElements();
            ShowWindow("Menu");
        }
        #endregion

        #region Resizing Window
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            ResizeElements();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState != LastWindowState)
            {
                LastWindowState = WindowState;

                if (WindowState == FormWindowState.Maximized)
                {
                    ResizeElements();
                }
                if (WindowState == FormWindowState.Normal)
                {
                    ResizeElements();
                }
            }
        }

        public void ResizeElements()
        {
            #region PictureBackgroundMain
            PictureBackgroundMain.Left = 0;
            PictureBackgroundMain.Top = 0;
            PictureBackgroundMain.Width = ClientSize.Width;
            PictureBackgroundMain.Height = ClientSize.Height;
            #endregion
            #region PictureLogo
            PictureLogo.Left = ClientSize.Width / 3;
            PictureLogo.Top = ClientSize.Height / 20;
            PictureLogo.Width = ClientSize.Width / 3;
            PictureLogo.Height = ClientSize.Height / 2;
            #endregion
            #region LabelPlayerName
            LabelInputName.Left = (ClientSize.Width * 5) / 12;
            LabelInputName.Top = (ClientSize.Height * 3) / 5;
            LabelInputName.Width = ClientSize.Width / 6;
            LabelInputName.Height = ClientSize.Height / 20;
            #endregion
            #region TextBoxPlayerName
            TextBoxPlayerName.Left = (ClientSize.Width * 3) / 8;
            TextBoxPlayerName.Top = (ClientSize.Height * 13) / 20;
            TextBoxPlayerName.Width = ClientSize.Width / 4;
            TextBoxPlayerName.Height = (ClientSize.Height * 3) / 40;
            #endregion
            #region PictureStartButton
            PictureStartButton.Left = (ClientSize.Width * 5) / 12;
            PictureStartButton.Top = (ClientSize.Height * 3) / 4;
            PictureStartButton.Width = ClientSize.Width / 6;
            PictureStartButton.Height = (ClientSize.Height * 11) / 80;
            #endregion

            #region PictureBackgroundGame
            PictureBackgroundGame.Left = 0;
            PictureBackgroundGame.Top = 0;
            PictureBackgroundGame.Width = ClientSize.Width;
            PictureBackgroundGame.Height = ClientSize.Height;
            #endregion
            #region PictureJoker5050
            PictureJoker5050.Left = ClientSize.Width / 3;
            PictureJoker5050.Top = ClientSize.Height / 40;
            PictureJoker5050.Width = ClientSize.Width / 10;
            PictureJoker5050.Height = (ClientSize.Height * 9) / 80;
            #endregion
            #region PictureJokerChangeQuestion
            PictureJokerChangeQuestion.Left = (ClientSize.Width * 9) / 20;
            PictureJokerChangeQuestion.Top = ClientSize.Height / 40;
            PictureJokerChangeQuestion.Width = ClientSize.Width / 10;
            PictureJokerChangeQuestion.Height = (ClientSize.Height * 9) / 80;
            #endregion
            #region PictureJokerAskAudience
            PictureJokerAskAudience.Left = (ClientSize.Width * 17) / 30;
            PictureJokerAskAudience.Top = ClientSize.Height / 40;
            PictureJokerAskAudience.Width = ClientSize.Width / 10;
            PictureJokerAskAudience.Height = (ClientSize.Height * 9) / 80;
            #endregion
            #region PictureQuestionReward
            PictureQuestionReward.Left = 0;
            PictureQuestionReward.Top = (ClientSize.Height * 21) / 80;
            PictureQuestionReward.Width = ClientSize.Width;
            PictureQuestionReward.Height = (ClientSize.Height * 7) / 80;
            #endregion
            #region LabelQuestionReward
            LabelQuestionReward.Left = (PictureQuestionReward.Width * 5) / 12;
            LabelQuestionReward.Top = PictureQuestionReward.Height / 8;
            LabelQuestionReward.Width = ClientSize.Width / 6;
            LabelQuestionReward.Height = ClientSize.Height / 16;
            #endregion
            #region PictureQuestion
            PictureQuestion.Left = 0;
            PictureQuestion.Top = (ClientSize.Height * 19) / 40;
            PictureQuestion.Width = ClientSize.Width;
            PictureQuestion.Height = ClientSize.Height / 8;
            #endregion
            #region LabelQuestion
            LabelQuestion.Left = (PictureQuestion.Width * 13) / 120;
            LabelQuestion.Top = PictureQuestion.Height / 10;
            LabelQuestion.Width = (ClientSize.Width * 47) / 60;
            LabelQuestion.Height = (PictureQuestion.Height * 4) / 5;
            #endregion
            #region PictureAnswerA
            PictureAnswerA.Left = 0;
            PictureAnswerA.Top = (ClientSize.Height * 5) / 8;
            PictureAnswerA.Width = ClientSize.Width / 2;
            PictureAnswerA.Height = (ClientSize.Height * 7) / 80;
            #endregion
            #region LabelAnswerA
            LabelAnswerA.Left = (PictureAnswerA.Width * 8) / 31;
            LabelAnswerA.Top = PictureAnswerA.Height / 5;
            LabelAnswerA.Width = (PictureAnswerA.Width * 19) / 30;
            LabelAnswerA.Height = (PictureAnswerA.Height * 4) / 7;
            #endregion
            #region PictureAnswerB
            PictureAnswerB.Left = ClientSize.Width / 2;
            PictureAnswerB.Top = (ClientSize.Height * 5) / 8;
            PictureAnswerB.Width = ClientSize.Width / 2;
            PictureAnswerB.Height = (ClientSize.Height * 7) / 80;
            #endregion
            #region LabelAnswerB
            LabelAnswerB.Left = (PictureAnswerB.Width * 5) / 32;
            LabelAnswerB.Top = PictureAnswerB.Height / 5;
            LabelAnswerB.Width = (PictureAnswerB.Width * 19) / 30;
            LabelAnswerB.Height = (PictureAnswerB.Height * 4) / 7;
            #endregion
            #region PictureAnswerC
            PictureAnswerC.Left = 0;
            PictureAnswerC.Top = (ClientSize.Height * 29) / 40;
            PictureAnswerC.Width = ClientSize.Width / 2;
            PictureAnswerC.Height = (ClientSize.Height * 7) / 80;
            #endregion
            #region LabelAnswerC
            LabelAnswerC.Left = (PictureAnswerC.Width * 8) / 31;
            LabelAnswerC.Top = PictureAnswerC.Height / 5;
            LabelAnswerC.Width = (PictureAnswerC.Width * 19) / 30;
            LabelAnswerC.Height = (PictureAnswerC.Height * 4) / 7;
            #endregion
            #region PictureAnswerD
            PictureAnswerD.Left = ClientSize.Width / 2;
            PictureAnswerD.Top = (ClientSize.Height * 29) / 40;
            PictureAnswerD.Width = ClientSize.Width / 2;
            PictureAnswerD.Height = (ClientSize.Height * 7) / 80;
            #endregion
            #region LabelAnswerD
            LabelAnswerD.Left = (PictureAnswerD.Width * 5) / 32;
            LabelAnswerD.Top = PictureAnswerD.Height / 5;
            LabelAnswerD.Width = (PictureAnswerD.Width * 19) / 30;
            LabelAnswerD.Height = (PictureAnswerD.Height * 4) / 7;
            #endregion

            #region PictureBackgroundReward
            PictureBackgroundReward.Left = 0;
            PictureBackgroundReward.Top = 0;
            PictureBackgroundReward.Width = ClientSize.Width;
            PictureBackgroundReward.Height = ClientSize.Height;
            #endregion
            #region LabelPlayerName
            LabelPlayerName.Left = 0;
            LabelPlayerName.Top = (ClientSize.Height * 33) / 80;
            LabelPlayerName.Width = ClientSize.Width;
            LabelPlayerName.Height = (ClientSize.Height * 3) / 40;
            #endregion
            #region PictureRewardNow
            PictureRewardNow.Left = 0;
            PictureRewardNow.Top = ClientSize.Height / 2;
            PictureRewardNow.Width = ClientSize.Width;
            PictureRewardNow.Height = (ClientSize.Height * 7) / 80;
            #endregion
            #region LabelRewardNow
            LabelRewardNow.Left = (PictureRewardNow.Width * 5) / 12;
            LabelRewardNow.Top = PictureRewardNow.Height / 8;
            LabelRewardNow.Width = ClientSize.Width / 6;
            LabelRewardNow.Height = ClientSize.Height / 16;
            #endregion

            #region PictureBackgroundLeaderboard
            PictureBackgroundLeaderboard.Left = 0;
            PictureBackgroundLeaderboard.Top = 0;
            PictureBackgroundLeaderboard.Width = ClientSize.Width;
            PictureBackgroundLeaderboard.Height = ClientSize.Height;
            #endregion
            #region LabelLeaderboardTitle
            LabelLeaderboardTitle.Left = 0;
            LabelLeaderboardTitle.Top = 0;
            LabelLeaderboardTitle.Width = ClientSize.Width;
            LabelLeaderboardTitle.Height = ClientSize.Height / 8;
            #endregion
            #region LabelResult1
            LabelResult1.Left = 0;
            LabelResult1.Top = ClientSize.Height * 15 / 80;
            LabelResult1.Width = ClientSize.Width;
            LabelResult1.Height = ClientSize.Height * 3 / 40;
            #endregion
            #region LabelResult2
            LabelResult2.Left = 0;
            LabelResult2.Top = ClientSize.Height * 21 / 80;
            LabelResult2.Width = ClientSize.Width;
            LabelResult2.Height = ClientSize.Height * 3 / 40;
            #endregion
            #region LabelResult3
            LabelResult3.Left = 0;
            LabelResult3.Top = ClientSize.Height * 27 / 80;
            LabelResult3.Width = ClientSize.Width;
            LabelResult3.Height = ClientSize.Height * 3 / 40;
            #endregion
            #region LabelResult4
            LabelResult4.Left = 0;
            LabelResult4.Top = ClientSize.Height * 33 / 80;
            LabelResult4.Width = ClientSize.Width;
            LabelResult4.Height = ClientSize.Height * 3 / 40;
            #endregion
            #region LabelResult5
            LabelResult5.Left = 0;
            LabelResult5.Top = ClientSize.Height * 39 / 80;
            LabelResult5.Width = ClientSize.Width;
            LabelResult5.Height = ClientSize.Height * 3 / 40;
            #endregion
            #region LabelResult6
            LabelResult6.Left = 0;
            LabelResult6.Top = ClientSize.Height * 45 / 80;
            LabelResult6.Width = ClientSize.Width;
            LabelResult6.Height = ClientSize.Height * 3 / 40;
            #endregion
            #region LabelResult7
            LabelResult7.Left = 0;
            LabelResult7.Top = ClientSize.Height * 51 / 80;
            LabelResult7.Width = ClientSize.Width;
            LabelResult7.Height = ClientSize.Height * 3 / 40;
            #endregion
            #region LabelResult8
            LabelResult8.Left = 0;
            LabelResult8.Top = ClientSize.Height * 57 / 80;
            LabelResult8.Width = ClientSize.Width;
            LabelResult8.Height = ClientSize.Height * 3 / 40;
            #endregion
            #region LabelResult9
            LabelResult9.Left = 0;
            LabelResult9.Top = ClientSize.Height * 63 / 80;
            LabelResult9.Width = ClientSize.Width;
            LabelResult9.Height = ClientSize.Height * 3 / 40;
            #endregion
            #region LabelResult10
            LabelResult10.Left = 0;
            LabelResult10.Top = ClientSize.Height * 69 / 80;
            LabelResult10.Width = ClientSize.Width;
            LabelResult10.Height = ClientSize.Height * 3 / 40;
            #endregion

            ResizeFont();
        }

        public void ResizeFont()
        {
            List<Label> labels = new List<Label>
            {
                LabelInputName,
                LabelQuestionReward,
                LabelQuestion,
                LabelAnswerA,
                LabelAnswerB,
                LabelAnswerC,
                LabelAnswerD,
                LabelPlayerName,
                LabelRewardNow,
                LabelLeaderboardTitle,
                LabelResult1,
                LabelResult2,
                LabelResult3,
                LabelResult4,
                LabelResult5,
                LabelResult6,
                LabelResult7,
                LabelResult8,
                LabelResult9,
                LabelResult10
            };

            foreach (Label aLabel in labels)
            {
                aLabel.Font = new Font(aLabel.Font.FontFamily, 72, aLabel.Font.Style);
                while (aLabel.Width < TextRenderer.MeasureText(aLabel.Text, new Font(aLabel.Font.FontFamily, aLabel.Font.Size, aLabel.Font.Style)).Width)
                {
                    aLabel.Font = new Font(aLabel.Font.FontFamily, aLabel.Font.Size - 0.5f, aLabel.Font.Style);
                }
                while (aLabel.Height < TextRenderer.MeasureText(aLabel.Text, new Font(aLabel.Font.FontFamily, aLabel.Font.Size, aLabel.Font.Style)).Height)
                {
                    aLabel.Font = new Font(aLabel.Font.FontFamily, aLabel.Font.Size - 0.5f, aLabel.Font.Style);
                }
            }
        }


        #endregion

        #region Mouse Events
        #region PictureStartButton
        private void PictureStartButton_MouseEnter(object sender, EventArgs e)
        {
            PictureStartButton.Image = Properties.Resources.start_using;
        }

        private void PictureStartButton_MouseLeave(object sender, EventArgs e)
        {
            PictureStartButton.Image = Properties.Resources.start;
        }

        private void PictureStartButton_Click(object sender, EventArgs e)
        {
            if(TextBoxPlayerName.Text != "")
            {
                game.PlayerName = TextBoxPlayerName.Text;
                StartRound();
            }
            else
            {
                TextBox TB = (TextBox)TextBoxPlayerName;
                int VisibleTime = 2000;  //in milliseconds

                ToolTip tt = new ToolTip();
                tt.Show("Wpisz nazwę!", TB, -85, 3, VisibleTime);
                return;
            }
        }
        #endregion
        #region PictureJoker5050
        private void PictureJoker5050_MouseEnter(object sender, EventArgs e)
        {
            if(status._5050 && game.Active)
                PictureJoker5050.Image = Properties.Resources._5050_using;
            
        }

        private void PictureJoker5050_MouseLeave(object sender, EventArgs e)
        {
            if (status._5050 && game.Active)
                PictureJoker5050.Image = Properties.Resources._5050;
        }

        private void PictureJoker5050_Click(object sender, EventArgs e)
        {
            if (status._5050 && game.Active)
                Joker5050();
        }
        #endregion
        #region PictureJokerChangeQuestion
        private void PictureJokerChangeQuestion_MouseEnter(object sender, EventArgs e)
        {
            if (status.ChangeQuestion && game.Active)
                PictureJokerChangeQuestion.Image = Properties.Resources.change_question_using;
        }

        private void PictureJokerChangeQuestion_MouseLeave(object sender, EventArgs e)
        {
            if (status.ChangeQuestion && game.Active)
                PictureJokerChangeQuestion.Image = Properties.Resources.change_question;
        }

        private void PictureJokerChangeQuestion_Click(object sender, EventArgs e)
        {
            if (status.ChangeQuestion && game.Active)
                JokerChangeQuestion();
        }
        #endregion
        #region PictureJokerAskAudience
        private void PictureJokerAskAudience_MouseEnter(object sender, EventArgs e)
        {
            if (status.AskAudience && game.Active && status.AskAudienceActive == false)
                PictureJokerAskAudience.Image = Properties.Resources.ask_the_audience_using;
        }

        private void PictureJokerAskAudience_MouseLeave(object sender, EventArgs e)
        {
            if (status.AskAudience && game.Active && status.AskAudienceActive == false)
                PictureJokerAskAudience.Image = Properties.Resources.ask_the_audience;
        }

        private void PictureJokerAskAudience_Click(object sender, EventArgs e)
        {
            if (status.AskAudience && game.Active)
                JokerAskAudience();
        }
        #endregion
        #region PictureAnswerA
        private void PictureAnswerA_MouseEnter(object sender, EventArgs e)
        {
            if(game.Active && status.Answer1)
            {
                PictureAnswerA.Image = Properties.Resources.answer_box_a_using;
                LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#f89b1c");
                LabelAnswerA.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void PictureAnswerA_MouseLeave(object sender, EventArgs e)
        {
            if (game.Active && status.Answer1)
            {
                PictureAnswerA.Image = Properties.Resources.answer_box_a;
                LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                LabelAnswerA.ForeColor = System.Drawing.Color.White;
            }
        }

        private void PictureAnswerA_Click(object sender, EventArgs e)
        {
            if (game.Active && status.Answer1)
                CheckAnswer(1);
        }
        #endregion
        #region PictureAnswerB
        private void PictureAnswerB_MouseEnter(object sender, EventArgs e)
        {
            if (game.Active && status.Answer2)
            {
                PictureAnswerB.Image = Properties.Resources.answer_box_b_using;
                LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#f89b1c");
                LabelAnswerB.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void PictureAnswerB_MouseLeave(object sender, EventArgs e)
        {
            if (game.Active && status.Answer2)
            {
                PictureAnswerB.Image = Properties.Resources.answer_box_b;
                LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                LabelAnswerB.ForeColor = System.Drawing.Color.White;
            }
        }

        private void PictureAnswerB_Click(object sender, EventArgs e)
        {
            if (game.Active && status.Answer2)
                CheckAnswer(2);
        }
        #endregion
        #region PictureAnswerC
        private void PictureAnswerC_MouseEnter(object sender, EventArgs e)
        {
            if (game.Active && status.Answer3)
            {
                PictureAnswerC.Image = Properties.Resources.answer_box_c_using;
                LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#f89b1c");
                LabelAnswerC.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void PictureAnswerC_MouseLeave(object sender, EventArgs e)
        {
            if (game.Active && status.Answer3)
            {
                PictureAnswerC.Image = Properties.Resources.answer_box_c;
                LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                LabelAnswerC.ForeColor = System.Drawing.Color.White;
            }
        }

        private void PictureAnswerC_Click(object sender, EventArgs e)
        {
            if (game.Active && status.Answer3)
                CheckAnswer(3);
        }
        #endregion
        #region PictureAnswerD
        private void PictureAnswerD_MouseEnter(object sender, EventArgs e)
        {
            if (game.Active && status.Answer4)
            {
                PictureAnswerD.Image = Properties.Resources.answer_box_d_using;
                LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#f89b1c");
                LabelAnswerD.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void PictureAnswerD_MouseLeave(object sender, EventArgs e)
        {
            if (game.Active && status.Answer4)
            {
                PictureAnswerD.Image = Properties.Resources.answer_box_d;
                LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                LabelAnswerD.ForeColor = System.Drawing.Color.White;
            }
        }

        private void PictureAnswerD_Click(object sender, EventArgs e)
        {
            if (game.Active && status.Answer4)
                CheckAnswer(4);
        }
        #endregion
        #endregion

        public void ShowWindow(string name)
        {
            switch (name)
            {
                case "Menu":
                    PictureBackgroundMain.Visible = true;
                    PictureBackgroundGame.Visible = false;
                    PictureBackgroundReward.Visible = false;
                    PictureBackgroundLeaderboard.Visible = false;
                    PlaySound("intro");
                    break;
                case "Game":
                    PictureJoker5050.Parent = PictureBackgroundGame;
                    PictureJokerChangeQuestion.Parent = PictureBackgroundGame;
                    PictureJokerAskAudience.Parent = PictureBackgroundGame;
                    LabelQuestionReward.Text = game.Reward;

                    PictureBackgroundGame.Visible = true;
                    PictureBackgroundMain.Visible = false;
                    PictureBackgroundReward.Visible = false;
                    PictureBackgroundLeaderboard.Visible = false;
                    break;
                case "Reward":
                    PictureJoker5050.Parent = PictureBackgroundReward;
                    PictureJokerChangeQuestion.Parent = PictureBackgroundReward;
                    PictureJokerAskAudience.Parent = PictureBackgroundReward;
                    LabelPlayerName.Text = game.PlayerName;
                    LabelRewardNow.Text = game.Reward;

                    PictureBackgroundReward.Visible = true;
                    PictureBackgroundMain.Visible = false;
                    PictureBackgroundGame.Visible = false;
                    PictureBackgroundLeaderboard.Visible = false;
                    break;
                case "Win":
                    LabelPlayerName.Text = "Wygrana";
                    LabelRewardNow.Text = game.Reward;

                    PictureBackgroundReward.Visible = true;
                    PictureBackgroundMain.Visible = false;
                    PictureBackgroundGame.Visible = false;
                    PictureBackgroundLeaderboard.Visible = false;
                    break;
                case "Leaderboard":
                    PictureBackgroundLeaderboard.Visible = true;
                    PictureBackgroundMain.Visible = false;
                    PictureBackgroundGame.Visible = false;
                    PictureBackgroundReward.Visible = false;
                    break;
                default:
                    PictureBackgroundLeaderboard.Visible = false;
                    PictureBackgroundMain.Visible = false;
                    PictureBackgroundGame.Visible = false;
                    PictureBackgroundReward.Visible = false;
                    MessageBox.Show("Taki ekran nie istnieje!");
                    break;
            }
        }

        public void LoadQuestions()
        {
            string qDifficulty = "";

            try
            {
                using (XmlReader reader = XmlReader.Create("./resources/anime.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "easy":
                                    qDifficulty = "easy";
                                    break;
                                case "medium":
                                    qDifficulty = "medium";
                                    break;
                                case "hard":
                                    qDifficulty = "hard";
                                    break;
                                case "question":
                                    if (qDifficulty == "easy")
                                    {
                                        QuestionEasy.Add(new Question(reader["text"], reader["answer1"], reader["answer2"], reader["answer3"], reader["answer4"]));
                                    }
                                    if (qDifficulty == "medium")
                                    {
                                        QuestionMedium.Add(new Question(reader["text"], reader["answer1"], reader["answer2"], reader["answer3"], reader["answer4"]));
                                    }
                                    if (qDifficulty == "hard")
                                    {
                                        QuestionHard.Add(new Question(reader["text"], reader["answer1"], reader["answer2"], reader["answer3"], reader["answer4"]));
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie mozna załadować pliku z pytaniami!\nError: " + ex.Message);
            }
        }

        public void ResetGame()
        {
            #region Clear Parameters
            game.Reset();
            status.ResetAll();
            PictureJoker5050.Image = Properties.Resources._5050;
            PictureJokerChangeQuestion.Image = Properties.Resources.change_question;
            PictureJokerAskAudience.Image = Properties.Resources.ask_the_audience;
            PictureAnswerA.Image = Properties.Resources.answer_box_a;
            PictureAnswerB.Image = Properties.Resources.answer_box_b;
            PictureAnswerC.Image = Properties.Resources.answer_box_c;
            PictureAnswerD.Image = Properties.Resources.answer_box_d;
            LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            LabelAnswerA.ForeColor = System.Drawing.Color.White;
            LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            LabelAnswerB.ForeColor = System.Drawing.Color.White;
            LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            LabelAnswerC.ForeColor = System.Drawing.Color.White;
            LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            LabelAnswerD.ForeColor = System.Drawing.Color.White;
            TextBoxPlayerName.Text = "";
            #endregion

            #region Check Questions
            if (QuestionEasy.Count < 3 || QuestionMedium.Count < 6 || QuestionHard.Count < 6)
            {
                MessageBox.Show("Error: Niewystarczająca liczba pytań do rozpoczęcia rundy!");
                ShowWindow("Menu");
                return;
            }
            #endregion

            ShowWindow("Menu");
        }

        public void DrawQuestion()
        {
            status.ResetAnswers();

            #region Declarations
            int randomQuestion = 0, randomAnswer = 0, questionDifficulty = 0;
            Random rnd = new Random();
            bool[] isChecked = { false, false, false, false, false };
            #endregion

            #region Set Question Difficulty
            if (game.Round < 2)
            {
                questionDifficulty = 1;
            }
            else if (game.Round < 7)
            {
                questionDifficulty = 2;
            }
            else if (game.Round < 12)
            {
                questionDifficulty = 3;
            }
            #endregion

            #region Random Question
            try
            {
                int countSpaces;
                switch (questionDifficulty)
                {

                    case 1:
                        randomQuestion = rnd.Next(0, QuestionEasy.Count);
                        countSpaces = QuestionEasy[randomQuestion].Text.Count(Char.IsWhiteSpace);
                        if (QuestionEasy[randomQuestion].Text.Length > 80 && countSpaces > 10)
                        {
                            string textToSplit = QuestionEasy[randomQuestion].Text;
                            int positionOf10Space = textToSplit.Split(' ').Take(10).Sum(a => a.Length + 1) - 1;
                            string newString = QuestionEasy[randomQuestion].Text.Insert(positionOf10Space, "\n");
                            LabelQuestion.Text = newString.Replace(@"\n", "\n");
                        }
                        else
                        {
                            LabelQuestion.Text = QuestionEasy[randomQuestion].Text;
                        }
                        break;
                    case 2:
                        randomQuestion = rnd.Next(0, QuestionMedium.Count);
                        countSpaces = QuestionMedium[randomQuestion].Text.Count(Char.IsWhiteSpace);
                        if (QuestionMedium[randomQuestion].Text.Length > 80 && countSpaces > 10)
                        {
                            string textToSplit2 = QuestionMedium[randomQuestion].Text;
                            int positionOf10Space2 = textToSplit2.Split(' ').Take(10).Sum(a => a.Length + 1) - 1;
                            string newString2 = QuestionMedium[randomQuestion].Text.Insert(positionOf10Space2, "\n");
                            LabelQuestion.Text = newString2.Replace(@"\n", "\n");
                        }
                        else
                        {
                            LabelQuestion.Text = QuestionMedium[randomQuestion].Text;
                        }
                        break;
                    case 3:
                        randomQuestion = rnd.Next(0, QuestionHard.Count);
                        countSpaces = QuestionHard[randomQuestion].Text.Count(Char.IsWhiteSpace);
                        if (QuestionHard[randomQuestion].Text.Length > 80 && countSpaces > 10)
                        {
                            string textToSplit3 = QuestionHard[randomQuestion].Text;
                            int positionOf10Space3 = textToSplit3.Split(' ').Take(10).Sum(a => a.Length + 1) - 1;
                            string newString3 = QuestionHard[randomQuestion].Text.Insert(positionOf10Space3, "\n");
                            LabelQuestion.Text = newString3.Replace(@"\n", "\n");
                        }
                        else
                        {
                            LabelQuestion.Text = QuestionHard[randomQuestion].Text;
                        }
                        break;
                }
            }
            catch
            {
                ShowWindow("error");
                MessageBox.Show("ERROR: Brak pytań!");
                return;
            }
            #endregion

            #region Random Answers
            status.ResetAnswers();
            status.CurrentQuestion = randomQuestion;

            #region Answer A
            //losowanie miejsca odpowiedzi A
            randomAnswer = rnd.Next(1, 5);
            if (isChecked[randomAnswer] == false)
            {
                switch (questionDifficulty)
                {
                    case 1:
                        LabelAnswerA.Text = QuestionEasy[randomQuestion].Answer(randomAnswer);
                        break;
                    case 2:
                        LabelAnswerA.Text = QuestionMedium[randomQuestion].Answer(randomAnswer);
                        break;
                    case 3:
                        LabelAnswerA.Text = QuestionHard[randomQuestion].Answer(randomAnswer);
                        break;
                }
                isChecked[randomAnswer] = true;
                if (randomAnswer == 1) { status.CorrectAnswer = 1; }
            }
            #endregion

            #region Answer B
            //losowanie miejsca odpowiedzi B
            do
            {
                randomAnswer = rnd.Next(1, 5);
                if (isChecked[randomAnswer] == false)
                {
                    switch (questionDifficulty)
                    {
                        case 1:
                            LabelAnswerB.Text = QuestionEasy[randomQuestion].Answer(randomAnswer);
                            break;
                        case 2:
                            LabelAnswerB.Text = QuestionMedium[randomQuestion].Answer(randomAnswer);
                            break;
                        case 3:
                            LabelAnswerB.Text = QuestionHard[randomQuestion].Answer(randomAnswer);
                            break;
                    }
                    isChecked[randomAnswer] = true;
                    if (randomAnswer == 1) { status.CorrectAnswer = 2; }
                    break;
                }
            } while (true);
            #endregion

            #region Answer C
            //losowanie miejsca odpowiedzi C
            do
            {
                randomAnswer = rnd.Next(1, 5);
                if (isChecked[randomAnswer] == false)
                {
                    switch (questionDifficulty)
                    {
                        case 1:
                            LabelAnswerC.Text = QuestionEasy[randomQuestion].Answer(randomAnswer);
                            break;
                        case 2:
                            LabelAnswerC.Text = QuestionMedium[randomQuestion].Answer(randomAnswer);
                            break;
                        case 3:
                            LabelAnswerC.Text = QuestionHard[randomQuestion].Answer(randomAnswer);
                            break;
                    }
                    isChecked[randomAnswer] = true;
                    if (randomAnswer == 1) { status.CorrectAnswer = 3; }
                    break;
                }
            } while (true);
            #endregion

            #region Answer D
            //losowanie miejsca odpowiedzi D
            do
            {
                randomAnswer = rnd.Next(1, 5);
                if (isChecked[randomAnswer] == false)
                {
                    switch (questionDifficulty)
                    {
                        case 1:
                            LabelAnswerD.Text = QuestionEasy[randomQuestion].Answer(randomAnswer);
                            break;
                        case 2:
                            LabelAnswerD.Text = QuestionMedium[randomQuestion].Answer(randomAnswer);
                            break;
                        case 3:
                            LabelAnswerD.Text = QuestionHard[randomQuestion].Answer(randomAnswer);
                            break;
                    }
                    isChecked[randomAnswer] = true;
                    if (randomAnswer == 1) { status.CorrectAnswer = 4; }
                    break;
                }
            } while (true);
            #endregion

            #endregion

            #region Remove Question From List
            switch (questionDifficulty)
            {
                case 1:
                    QuestionEasy.RemoveAt(randomQuestion);
                    break;
                case 2:
                    QuestionMedium.RemoveAt(randomQuestion);
                    break;
                case 3:
                    QuestionHard.RemoveAt(randomQuestion);
                    break;
            }
            #endregion

            ResizeFont();
        }

        public async void CheckAnswer(int chosen)
        {
            async void Blink(int answer)
            {
                switch(answer)
                {
                    case 1:
                        PictureAnswerA.Image = Properties.Resources.answer_box_a_good;
                        LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerA.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerA.Image = Properties.Resources.answer_box_a;
                        LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerA.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        PictureAnswerA.Image = Properties.Resources.answer_box_a_good;
                        LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerA.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerA.Image = Properties.Resources.answer_box_a;
                        LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerA.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        PictureAnswerA.Image = Properties.Resources.answer_box_a_good;
                        LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerA.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerA.Image = Properties.Resources.answer_box_a;
                        LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerA.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        break;
                    case 2:
                        PictureAnswerB.Image = Properties.Resources.answer_box_b_good;
                        LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerB.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerB.Image = Properties.Resources.answer_box_b;
                        LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerB.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        PictureAnswerB.Image = Properties.Resources.answer_box_b_good;
                        LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerB.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerB.Image = Properties.Resources.answer_box_b;
                        LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerB.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        PictureAnswerB.Image = Properties.Resources.answer_box_b_good;
                        LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerB.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerB.Image = Properties.Resources.answer_box_b;
                        LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerB.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        break;
                    case 3:
                        PictureAnswerC.Image = Properties.Resources.answer_box_c_good;
                        LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerC.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerC.Image = Properties.Resources.answer_box_c;
                        LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerC.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        PictureAnswerC.Image = Properties.Resources.answer_box_c_good;
                        LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerC.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerC.Image = Properties.Resources.answer_box_c;
                        LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerC.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        PictureAnswerC.Image = Properties.Resources.answer_box_c_good;
                        LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerC.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerC.Image = Properties.Resources.answer_box_c;
                        LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerC.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        break;
                    case 4:
                        PictureAnswerD.Image = Properties.Resources.answer_box_d_good;
                        LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerD.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerD.Image = Properties.Resources.answer_box_d;
                        LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerD.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        PictureAnswerD.Image = Properties.Resources.answer_box_d_good;
                        LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerD.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerD.Image = Properties.Resources.answer_box_d;
                        LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerD.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        PictureAnswerD.Image = Properties.Resources.answer_box_d_good;
                        LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#00c300");
                        LabelAnswerD.ForeColor = System.Drawing.Color.Black;
                        await Task.Delay(300);
                        PictureAnswerD.Image = Properties.Resources.answer_box_d;
                        LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        LabelAnswerD.ForeColor = System.Drawing.Color.White;
                        await Task.Delay(300);
                        break;
                }
            }

            game.Active = false;
            switch (status.CorrectAnswer)
            {
                case 1:
                    PlaySound("logged_in_start");
                    await Task.Delay(3000);
                    if (chosen == 1)
                    {
                        PlaySound("correct_answer_level_1");
                    }
                    else
                    {
                        PlaySound("wrong_answer");
                    }
                    Blink(1);
                    if (chosen == 1)
                    {
                        if (game.Round >= 12)
                        {
                            await Task.Delay(3000);
                            WonGame();
                        }
                        else
                        {
                            await Task.Delay(3000);
                            StartRound();
                        }
                    }
                    else
                    {
                        await Task.Delay(3000);
                        EndGame();
                    }
                    break;
                case 2:
                    PlaySound("logged_in_start");
                    await Task.Delay(3000);
                    if (chosen == 2)
                    {
                        PlaySound("correct_answer_level_1");
                    }
                    else
                    {
                        PlaySound("wrong_answer");
                    }
                    Blink(2);
                    if (chosen == 2)
                    {
                        if (game.Round >= 12)
                        {
                            await Task.Delay(3000);
                            WonGame();
                        }
                        else
                        {
                            await Task.Delay(3000);
                            StartRound();
                        }
                    }
                    else
                    {
                        await Task.Delay(3000);
                        EndGame();
                    }
                    break;
                case 3:
                    PlaySound("logged_in_start");
                    await Task.Delay(3000);
                    if (chosen == 3)
                    {
                        PlaySound("correct_answer_level_1");
                    }
                    else
                    {
                        PlaySound("wrong_answer");
                    }
                    Blink(3);
                    if (chosen == 3)
                    {
                        if (game.Round >= 12)
                        {
                            await Task.Delay(3000);
                            WonGame();
                        }
                        else
                        {
                            await Task.Delay(3000);
                            StartRound();
                        }
                    }
                    else
                    {
                        await Task.Delay(3000);
                        EndGame();
                    }
                    break;
                case 4:
                    PlaySound("logged_in_start");
                    await Task.Delay(3000);
                    if (chosen == 4)
                    {
                        PlaySound("correct_answer_level_1");
                    }
                    else
                    {
                        PlaySound("wrong_answer");
                    }
                    Blink(4);
                    if (chosen == 4)
                    {
                        if (game.Round >= 12)
                        {
                            await Task.Delay(3000);
                            WonGame();
                        }
                        else
                        {
                            await Task.Delay(3000);
                            StartRound();
                        }
                    }
                    else
                    {
                        await Task.Delay(3000);
                        EndGame();
                    }
                    break;
            }
        }

        public void InsertHighScore()
        {
            SortedHighScore = HighScore.OrderByDescending(o => o.Score).ToList();

            int count = 0, place = 1;
            List<Label> labels = new List<Label>
            {
                LabelResult1,
                LabelResult2,
                LabelResult3,
                LabelResult4,
                LabelResult5,
                LabelResult6,
                LabelResult7,
                LabelResult8,
                LabelResult9,
                LabelResult10
            };

            foreach (Label label in labels)
            {
                try
                {
                    label.Text = place + ": " + SortedHighScore.ElementAt(count).Name + " - " + SortedHighScore.ElementAt(count).Score + " zł";
                }
                catch
                {
                    break;
                }

                count++;
                place++;
            }

            ResizeFont();
        }

        public void WriteHighScoreToFile()
        {
            DateTime dateNow = DateTime.Now;

            using (StreamWriter outputFile = new StreamWriter("highscore/HighScore " + dateNow.ToString("dd-MM-yyyy HH.mm.ss") + ".txt", true))
            {
                int place = 1;
                foreach (HighScore score in SortedHighScore)
                {
                    outputFile.WriteLine(place + ": " + score.Name + " - " + score.Score + " zł");
                    place++;
                }
            }
        }

        public async void StartRound()
        {
            PlaySound("level_1");
            DrawQuestion();
            ShowWindow("Reward");
            await Task.Delay(3000);
            game.Round++;
            ShowWindow("Game");
            game.Active = true;
        }

        public async void WonGame()
        {
            ShowWindow("Win");

            PlaySound("correct_answer_wins_milion");
            await Task.Delay(23000);

            HighScore.Add(new HighScore(game.PlayerName, game.Money));
            InsertHighScore();
            WriteHighScoreToFile();

            ShowWindow("Leaderboard");
            await Task.Delay(5000);

            ResetGame();
        }

        public async void EndGame()
        {
            PlaySound("end_candidate_leaves");

            game.Round--;
            ShowWindow("Win");
            await Task.Delay(3000);

            HighScore.Add(new HighScore(game.PlayerName, game.Money));
            InsertHighScore();
            WriteHighScoreToFile();

            ShowWindow("Leaderboard");
            await Task.Delay(5000);

            ResetGame();
        }

        public async void PlaySound(string name)
        {
            await Task.Delay(1);
            try
            {
                MusicPlayer.URL = "./resources/" + name + ".mp3";
                MusicPlayer.settings.setMode("loop", true);
                MusicPlayer.controls.play();
            }
            catch
            {
                MessageBox.Show("Error: Nie mozna odtworzyc pliku dźwiękowego!");
            }
        }

        #region Jokers
        public async void Joker5050()
        {
            status._5050 = false;
            int randomAnswer;
            bool[] isChecked = { false, false, false, false, false };
            int[] discarded = { 0, 0 };
            isChecked[status.CorrectAnswer] = true;
            Random rnd = new Random();
            PictureJoker5050.Image = Properties.Resources._5050_using;
            await Task.Delay(1000);

            //losowanie pierwszej odpowiedzi do odrzucenia
            do
            {
                randomAnswer = rnd.Next(1, 5);
                if (isChecked[randomAnswer] == false)
                {
                    isChecked[randomAnswer] = true;
                    discarded[0] = randomAnswer;
                    break;
                }
            } while (true);

            //losowanie drugiej odpowiedzi do odrzucenia
            do
            {
                randomAnswer = rnd.Next(1, 5);
                if (isChecked[randomAnswer] == false)
                {
                    isChecked[randomAnswer] = true;
                    discarded[1] = randomAnswer;
                    break;
                }
            } while (true);

            //odrzucenie odpowiedzi
            for (int i = 0; i < 2; i++)
            {
                switch (discarded[i])
                {
                    case 1:
                        LabelAnswerA.Text = "";
                        status.Answer1 = false;
                        PictureAnswerA.Image = Properties.Resources.answer_box_a;
                        LabelAnswerA.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        break;
                    case 2:
                        LabelAnswerB.Text = "";
                        status.Answer2 = false;
                        PictureAnswerB.Image = Properties.Resources.answer_box_b;
                        LabelAnswerB.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        break;
                    case 3:
                        LabelAnswerC.Text = "";
                        status.Answer3 = false;
                        PictureAnswerC.Image = Properties.Resources.answer_box_c;
                        LabelAnswerC.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        break;
                    case 4:
                        LabelAnswerD.Text = "";
                        status.Answer4 = false;
                        PictureAnswerD.Image = Properties.Resources.answer_box_d;
                        LabelAnswerD.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
                        break;
                }
            }

            //wyłączenie 5050
            PictureJoker5050.Image = Properties.Resources._5050_off;

            //dzwięk
            PlaySound("50_50_joker");
            await Task.Delay(1000);
            PlaySound("level_1");
        }

        public async void JokerChangeQuestion()
        {
            status.ChangeQuestion = false;
            PictureJokerChangeQuestion.Image = Properties.Resources.change_question_using;
            await Task.Delay(1000);

            DrawQuestion();
            PictureJokerChangeQuestion.Image = Properties.Resources.change_question_off;

            PlaySound("50_50_joker");
            await Task.Delay(1000);
            PlaySound("level_1");
        }

        public async void JokerAskAudience()
        {
            if(status.AskAudienceActive)
            {
                PictureJokerAskAudience.Image = Properties.Resources.ask_the_audience_off;
                PlaySound("audience_joker_end");
                await Task.Delay(1000);
                PlaySound("level_1");
                status.AskAudienceActive = false;
                status.AskAudience = false;
            }
            else
            {
                PictureJokerAskAudience.Image = Properties.Resources.ask_the_audience_using;
                PlaySound("audience_joker_loop");
                status.AskAudienceActive = true;
            }
        }
        #endregion
    }

    public class Question
    {
        string sText;
        string sAnswer1, sAnswer2, sAnswer3, sAnswer4;

        public Question(string sText, string sAnswer1, string sAnswer2, string sAnswer3, string sAnswer4)
        {
            this.sText = sText;
            this.sAnswer1 = sAnswer1;
            this.sAnswer2 = sAnswer2;
            this.sAnswer3 = sAnswer3;
            this.sAnswer4 = sAnswer4;
        }

        public string Text
        {
            get
            {
                return sText;
            }
        }

        public string Answer1
        {
            get
            {
                return sAnswer1;
            }
        }

        public string Answer2
        {
            get
            {
                return sAnswer2;
            }
        }

        public string Answer3
        {
            get
            {
                return sAnswer3;
            }
        }

        public string Answer4
        {
            get
            {
                return sAnswer4;
            }
        }

        public string Answer(int number)
        {
            switch (number)
            {
                case 1:
                    return sAnswer1;
                case 2:
                    return sAnswer2;
                case 3:
                    return sAnswer3;
                case 4:
                    return sAnswer4;
                default:
                    return "error";

            }
        }
    }

    public class Game
    {
        bool bActive = false;
        string sPlayerName = "error";
        int iRound = 0;

        public void Reset()
        {
            bActive = false;
            sPlayerName = "error";
            iRound = 0;
        }

        public bool Active
        {
            get
            {
                return bActive;
            }
            set
            {
                bActive = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return sPlayerName;
            }
            set
            {
                sPlayerName = value;
            }
        }

        public int Round
        {
            get
            {
                return iRound;
            }
            set
            {
                iRound = value;
            }
        }

        public string Reward
        {
            get
            {
                switch(iRound)
                {
                    case 0:
                        return "0 zł";
                    case 1:
                        return "500 zł";
                    case 2:
                        return "1000 zł";
                    case 3:
                        return "2000 zł";
                    case 4:
                        return "5000 zł";
                    case 5:
                        return "10 000 zł";
                    case 6:
                        return "20 000 zł";
                    case 7:
                        return "40 000 zł";
                    case 8:
                        return "75 000 zł";
                    case 9:
                        return "125 000 zł";
                    case 10:
                        return "250 000 zł";
                    case 11:
                        return "500 000 zł";
                    case 12:
                        return "1 000 000 zł";
                    default:
                        return "error";
                }
            }
        }

        public int Money
        {
            get
            {
                switch (iRound)
                {
                    case 0:
                        return 0;
                    case 1:
                        return 500;
                    case 2:
                        return 1000;
                    case 3:
                        return 2000;
                    case 4:
                        return 5000;
                    case 5:
                        return 10000;
                    case 6:
                        return 20000;
                    case 7:
                        return 40000;
                    case 8:
                        return 75000;
                    case 9:
                        return 125000;
                    case 10:
                        return 250000;
                    case 11:
                        return 500000;
                    case 12:
                        return 1000000;
                    default:
                        return -1;
                }
            }
        }
    }

    public class Status
    {
        bool b5050 = true, bChangeQuestion = true, bAskAudience = true, bAskAudienceActive = false;
        bool bAnswer1 = true, bAnswer2 = true, bAnswer3 = true, bAnswer4 = true;
        int iCurrentQuestion = 0, iCorrectAnswer = 0;

        public void ResetAll()
        {
            b5050 = true;
            bChangeQuestion = true;
            bAskAudience = true;
            bAskAudienceActive = false;
            bAnswer1 = true;
            bAnswer2 = true;
            bAnswer3 = true;
            bAnswer4 = true;
            iCurrentQuestion = 0;
            iCorrectAnswer = 0;
        }

        public void ResetAnswers()
        {
            bAnswer1 = true;
            bAnswer2 = true;
            bAnswer3 = true;
            bAnswer4 = true;
            iCurrentQuestion = 0;
            iCorrectAnswer = 0;
        }

        public bool _5050
        {
            get
            {
                return b5050;
            }
            set
            {
                b5050 = value;
            }
        }

        public bool ChangeQuestion
        {
            get
            {
                return bChangeQuestion;
            }
            set
            {
                bChangeQuestion = value;
            }
        }

        public bool AskAudience
        {
            get
            {
                return bAskAudience;
            }
            set
            {
                bAskAudience = value;
            }
        }

        public bool AskAudienceActive
        {
            get
            {
                return bAskAudienceActive;
            }
            set
            {
                bAskAudienceActive = value;
            }
        }

        public bool Answer1
        {
            get
            {
                return bAnswer1;
            }
            set
            {
                bAnswer1 = value;
            }
        }

        public bool Answer2
        {
            get
            {
                return bAnswer2;
            }
            set
            {
                bAnswer2 = value;
            }
        }

        public bool Answer3
        {
            get
            {
                return bAnswer3;
            }
            set
            {
                bAnswer3 = value;
            }
        }

        public bool Answer4
        {
            get
            {
                return bAnswer4;
            }
            set
            {
                bAnswer4 = value;
            }
        }

        public int CurrentQuestion
        {
            get
            {
                return iCurrentQuestion;
            }
            set
            {
                iCurrentQuestion = value;
            }
        }

        public int CorrectAnswer
        {
            get
            {
                return iCorrectAnswer;
            }
            set
            {
                iCorrectAnswer = value;
            }
        }
    }

    public class HighScore
    {
        string sName;
        int iScore;

        public HighScore(string name, int score)
        {
            sName = name;
            iScore = score;
        }

        public string Name
        {
            get
            {
                return sName;
            }
        }

        public int Score
        {
            get
            {
                return iScore;
            }
        }
    }

    public class PixelBox : PictureBox
    {
        //jest to specjalna wersja pictureboxa która zawiera miedzy innymi włączony antyaliasing
        //włączone opcje w sekcji initialization

        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="PixelBox"> class.
        /// </see></summary>
        public PixelBox()
        {
            // Set default.
            InterpolationMode = InterpolationMode.High;
            SizeMode = PictureBoxSizeMode.StretchImage;
            BackColor = Color.Transparent;
            BackgroundImageLayout = ImageLayout.Stretch;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the interpolation mode.
        /// </summary>
        /// <value>The interpolation mode.</value>
        [Category("Behavior")]
        [DefaultValue(InterpolationMode.Default)]
        public InterpolationMode InterpolationMode { get; set; }
        #endregion

        #region Overrides of PictureBox
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"> event.
        /// </see></summary>
        /// <param name="pe" />A <see cref="T:System.Windows.Forms.PaintEventArgs"> that contains the event data. 
        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(pe);
        }
        #endregion
    }
}
