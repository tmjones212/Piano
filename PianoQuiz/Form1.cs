using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace PianoQuiz
{
    public partial class Form1 : Form
    {
        public List<PianoKey> keys;
        public List<PianoKey> blackKeys;
        public List<PianoKey> whiteKeys;
        static Random rnd = new Random();
        public string randomNote { get; set; }
        public string selectedNote { get; set; }
        public PianoKey currentKeyInQuestion { get; set; }
        public int correctAnswers { get; set; }
        public int wrongAnswers { get; set; }
        public Boolean playScale { get; set; }
        public Boolean gradeScale { get; set; }
        public Boolean playNote { get; set; }
        public string testingKeyColor { get; set; }
        public List<Scale> scales { get; set; }
        public List<Scale> majorScales { get; set; }
        public List<Chord> chords { get; set; }
        public List<Chord> majorChords { get; set; }
        public List<Chord> minorChords { get; set; }
        public List<Chord> augmentedChords { get; set; }

        public List<PianoKey> keysInScale { get; set; }
        public List<PianoKey> keysInRequestedScale { get; set; }
        public List<PianoKey> keysInRequestedChord { get; set; }
        public DateTime startDateTime = DateTime.Now;
        public int currentRandomNumber;
        public List<Control> pianoKeyControls { get; set; }
        public List<string> chordTypes { get; set; }
        Graphics g;

        public int trebleStaffStartingYCoordinate { get; set; }
        public int trebleStaffEndingYCoordinate { get; set; }

        public int bassStaffStartingYCoordinate { get; set; }
        public int bassStaffEndingYCoordinate { get; set; }
        public int staffLineSpacing { get; set; }
        public int noteSize { get; set; }
        public List<Rectangle> noteRectangles { get; set; }
        public List<PianoKey> keysOnStaff { get; set; }
        private Pen whitePen;
        private Pen backgroundColorPen;
        public Boolean staffGame { get; set; }
        public string staffGameType { get; set; }

        public Boolean chordGame { get; set; }

        public Boolean playChord { get; set; }
        public Chord correctAnswerChord { get; set; }

        //private PictureBox pictureBox1 = new PictureBox();

        //public System.Timers.Timer timer1 = new System.Timers.Timer();

        public int timeLeft;


        /*
         * 2/27/2020
         * Things to add:
         * Some way to only use the keyboard for guess the chord
         * Something to light up the notes of a scale and we guess that
         *
         * 
         * */
        public Form1()
        {
            InitializeComponent();

            g = this.CreateGraphics();

            //correctAnswerChord = null;
            

            listBoxNotes.Items.Add("A");
            listBoxNotes.Items.Add("A#");
            listBoxNotes.Items.Add("B");
            listBoxNotes.Items.Add("C");
            listBoxNotes.Items.Add("C#");
            listBoxNotes.Items.Add("D");
            listBoxNotes.Items.Add("D#");
            listBoxNotes.Items.Add("E");
            listBoxNotes.Items.Add("F");
            listBoxNotes.Items.Add("F#");
            listBoxNotes.Items.Add("G");
            listBoxNotes.Items.Add("G#");

            listBoxChordType.Items.Add("Major");
            listBoxChordType.Items.Add("Major");
            listBoxChordType.Items.Add("Augmented");
            listBoxChordType.Items.Add("Diminished");

            staffGame = false;
            staffGameType = "";

            chordGame = false;
            playChord = false;

            trebleStaffStartingYCoordinate = 120+40;
            trebleStaffEndingYCoordinate = 260+40;

            bassStaffStartingYCoordinate = 340+40;
            bassStaffEndingYCoordinate = 480+40;
            staffLineSpacing = 50;

            //noteSize = 40;
            noteSize = staffLineSpacing - 10;

            whitePen = new Pen(Color.White);
            backgroundColorPen = new Pen(Color.FromArgb(36, 38, 45));

            currentRandomNumber = 1;
            correctAnswers = 0;
            wrongAnswers = 0;
            labelResult.Visible = false;
            labelCorrectScaleKeys.Visible = false;
            labelYourScaleAnswer.Visible = false;
            playScale = false;
            gradeScale = false;
            playNote = false;
            keys = new List<PianoKey>();
            blackKeys = new List<PianoKey>();
            whiteKeys = new List<PianoKey>();
            scales = new List<Scale>();
            majorScales = new List<Scale>();
            keysInScale = new List<PianoKey>();

            chords = new List<Chord>();
            majorChords = new List<Chord>();
            minorChords = new List<Chord>();
            augmentedChords = new List<Chord>();

            chordTypes = new List<string>();
            pianoKeyControls = new List<Control>();
            noteRectangles = new List<Rectangle>();
            keysOnStaff = new List<PianoKey>();

            labelKeyToPress.Visible = false;

            //timer1.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //timer1.Interval = (1000);   //1 second
            //timeLeft = 1000 * 60; //1 minute
            //Console.WriteLine("The time left is: " + timeLeft);
            //timer1.Enabled = true;
            //timer1.Start();
            
            //SetKeyArrays();


            currentKeyInQuestion = null;
            CreateKeys();
            CreateScales();
            CreateChords();
            //PickKeyAtRandom("All");

            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);

            /*
            pictureBox1.SetBounds(0, 0, 250, 150);

            //pictureBox1.Dock = DockStyle.Fill;
            //pictureBox1.BackColor = Color.White;
            // Connect the Paint event of the PictureBox to the event handler method.
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);

            // Add the PictureBox control to the Form.
            this.Controls.Add(pictureBox1);
            */



        }

        /*
        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;

            Pen whitePen = new Pen(Brushes.White);
            whitePen.Width = 3.0F;
            // Draw a line in the PictureBox.
            //g.DrawLine(System.Drawing.Pens.Red,pictureBox1.Left, pictureBox1.Top,pictureBox1.Right, pictureBox1.Bottom);
            g.DrawLine(whitePen, pictureBox1.Left,10,250,10);
            g.DrawLine(whitePen, pictureBox1.Left, 30, 250, 30);
            g.DrawLine(whitePen, pictureBox1.Left, 50, 250, 50);
            g.DrawLine(whitePen, pictureBox1.Left, 70, 250, 70);
            g.DrawLine(whitePen, pictureBox1.Left, 90, 250, 90);
        }
        */

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            

            if(!(e.Modifiers == Keys.Shift) && !(e.Modifiers == Keys.Control))
            {
                selectedNote = e.KeyCode.ToString();
            }
            else if(e.Modifiers == Keys.Control)
            {
                Console.WriteLine("we are in control...");
                selectedNote = e.KeyCode.ToString();
                selectedNote += "b";

                selectedNote = FlatConversion(selectedNote);
            }
            else
            {
                selectedNote = e.KeyCode.ToString();
                selectedNote += "#";
            }

            Console.WriteLine("You pressed: " + selectedNote);

            if(selectedNote != "ShiftKey#" && !selectedNote.Contains("ControlKey"))
            {
                CheckForMatch();
            }



            int looper = 0;

            for (int i = 0; i < listBoxNotes.Items.Count; i++)
            {
                string item = (string)listBoxNotes.Items[looper];

                Console.WriteLine("The selected note is: " + selectedNote + " and the item in the list box is: " + item);

                if((string) item == selectedNote)
                {
                    listBoxNotes.SelectedIndex = looper;
                    Console.WriteLine("it is a match");
                } else
                {
                    //listBoxNotes.SetSelected(looper, false);
                }

                looper++;
            }

            this.Update();
            


            //throw new NotImplementedException();
        }

        public string FlatConversion(string s)
        {
            string returnString = "";

            if(s == "Ab")
            {
                returnString = "G#";
            } else if(s == "Bb")
            {
                returnString = "A#";
            }
            else if (s == "Db")
            {
                returnString = "C#";
            }
            else if (s == "Eb")
            {
                returnString = "D#";
            }
            else if (s == "Gb")
            {
                returnString = "F#";
            }

            Console.WriteLine("The return string is: " + returnString);

            return returnString;
        }

        public void CreateChords()
        {
            foreach(Scale scale in scales.FindAll(item=>item.scaleType == "Major"))
            {
                chords.Add(new Chord(scale.rootNote, scale.scaleType, scale.keys,keys));
                majorChords.Add(chords[chords.Count-1]);
                chords[chords.Count - 1].PrintChord();

                chords.Add(new Chord(scale.rootNote, "Augmented", scale.keys,keys));
                augmentedChords.Add(chords[chords.Count - 1]);
                chords[chords.Count - 1].PrintChord();

                chords.Add(new Chord(scale.rootNote, "Diminished", scale.keys, keys));
                augmentedChords.Add(chords[chords.Count - 1]);
                chords[chords.Count - 1].PrintChord();

            }

            foreach (Scale scale in scales.FindAll(item => item.scaleType == "Minor"))
            {
                chords.Add(new Chord(scale.rootNote, scale.scaleType, scale.keys,keys));
                minorChords.Add(chords[chords.Count - 1]);
                chords[chords.Count - 1].PrintChord();
            }

            chordTypes.Add("Major");
            chordTypes.Add("Augmented");
            chordTypes.Add("Diminished");
            chordTypes.Add("Minor");


        }

        public void CreateScales()
        {
            //List<PianoKey> octave3Keys = keys.FindAll(item => item.octave == 3).ToList();
            for(int i = 0;i<7;i++)
            {
                //white keys
                string scaleNote = keys[i].note;
                scales.Add(new Scale("Major", scaleNote, keys));
                scales.Add(new Scale("Minor", scaleNote, keys));
                scales.Add(new Scale("Dorian", scaleNote, keys));
                scales.Add(new Scale("Phrygian", scaleNote, keys));
                scales.Add(new Scale("Lydian", scaleNote, keys));
                scales.Add(new Scale("Mixolydian", scaleNote, keys));
                scales.Add(new Scale("Locrian", scaleNote, keys));
                
            }

            for(int i = 65; i<65+5;i++)
            {
                //black keys
                string scaleNote = keys[i].note;
                scales.Add(new Scale("Major", scaleNote, keys));
                scales.Add(new Scale("Minor", scaleNote, keys));
                scales.Add(new Scale("Dorian", scaleNote, keys));
                scales.Add(new Scale("Phrygian", scaleNote, keys));
                scales.Add(new Scale("Lydian", scaleNote, keys));
                scales.Add(new Scale("Mixolydian", scaleNote, keys));
                scales.Add(new Scale("Locrian", scaleNote, keys));
            }

            majorScales = scales.FindAll(item => item.scaleType == "Major").ToList();

        }

        public void SetKeyArrays()
        {
            foreach(PianoKey key in keys)
            {
                if(key.color == "White")
                {
                    whiteKeys.Add(key);
                }

                if(key.color == "Black")
                {
                    blackKeys.Add(key);
                }
            }
        }

        public void PickKeyAtRandom(string keyColor)
        {
            Console.WriteLine("The time left is: " + timeLeft);
            List<PianoKey> keySetToUse;


            if(keyColor == "All")
            {
                keySetToUse = keys.ToList();
            } else if(keyColor == "Black")
            {
                keySetToUse = blackKeys.ToList();
            }
            else if (keyColor == "White")
            {
                keySetToUse = whiteKeys.ToList();
            } else
            {
                keySetToUse = keys.ToList();
            }

            if (currentKeyInQuestion != null)
            {
                currentKeyInQuestion.keyButton.BackColor = currentKeyInQuestion.GetOriginalKeyColor();
            }

            Console.WriteLine(keySetToUse.Count);

            //int r = rnd.Next(keySetToUse.Count);
            //Console.WriteLine(keySetToUse[r].note + " " + keySetToUse[r].id);

            //randomNote = keySetToUse[r].note;

            //randomNote = GetRandomKey(keySetToUse);

            //currentKeyInQuestion = keySetToUse[r];

            currentKeyInQuestion = GetRandomKey(keySetToUse);
            randomNote = currentKeyInQuestion.note;

            currentKeyInQuestion.keyButton.BackColor = Color.Green;




        }


        public void CreateKeys()
        {
            int startingWhiteKeyX = 42;//76;
            int whiteKeyincrement = 32;//26;
            int whiteKeyY = 176;//216;
            int whiteKeyHeight = 140; //98;

            int startingBlackKeyX = (startingWhiteKeyX + whiteKeyincrement) - (whiteKeyincrement/4) ;
            int blackKeyHeight = (whiteKeyHeight / 2)+ (whiteKeyHeight / 8);
            int blackKeyY = (whiteKeyY);

            for (int i = 1;i<53;i++)
            {
                keys.Add(new PianoKey());
                PianoKey thisKey = keys[keys.Count - 1];
                thisKey.color = "White";
                thisKey.height = whiteKeyHeight;
                thisKey.width = whiteKeyincrement;
                thisKey.xValue = startingWhiteKeyX;
                thisKey.yValue = whiteKeyY;

                if (i == 1)
                {
                    thisKey.note = "A";
                    thisKey.midiNoteName = "A0";
                    thisKey.octave = 0;
                    thisKey.midiNoteNumber = 21;
                }

                PianoKey previousKey = null;

                if (i > 1)
                {
                    previousKey = keys[keys.Count - 2];
                    thisKey.note = thisKey.GetNextNoteOfSameColor(previousKey.note);


                    if (thisKey.note == "F" || thisKey.note == "C")
                    {
                        thisKey.midiNoteNumber = previousKey.midiNoteNumber + 1;
                        thisKey.octave = previousKey.octave;
                        thisKey.midiNoteName = thisKey.note + thisKey.octave;
                    }
                    else if (thisKey.note == "A")
                    {
                        thisKey.midiNoteNumber = previousKey.midiNoteNumber + 2;
                        thisKey.octave = previousKey.octave + 1;
                        thisKey.midiNoteName = thisKey.note + thisKey.octave;
                    }
                    else
                    {
                        thisKey.midiNoteNumber = previousKey.midiNoteNumber + 2;
                        thisKey.octave = previousKey.octave;
                        thisKey.midiNoteName = thisKey.note + thisKey.octave;
                    }
                }
            

                Button b = new Button();
                b.Height = whiteKeyHeight;
                b.Width = whiteKeyincrement;
                b.Location = new Point(startingWhiteKeyX, whiteKeyY);
                b.BackColor = Color.White;
                Console.WriteLine("We just created a new button at (" + startingWhiteKeyX + "," + whiteKeyY + ")");
                b.Visible = true;
                Controls.Add(b);
                pianoKeyControls.Add(b);
                b.Tag = thisKey;

                b.Click += handleTheClick;


                startingWhiteKeyX += whiteKeyincrement;
                keys[keys.Count - 1].keyButton = b;

                whiteKeys.Add(keys[keys.Count - 1]);
            }

            for(int k = 1; k<37;k++)
            {

                keys.Add(new PianoKey());
                PianoKey thisKey = keys[keys.Count - 1];
                thisKey.color = "Black";
                

                if (k == 1)
                {
                    thisKey.note = "A#";
                    thisKey.midiNoteNumber = 22;
                    thisKey.octave = 0;
                    thisKey.midiNoteName = thisKey.note + thisKey.octave;
                    Console.WriteLine(thisKey.note + "|" + thisKey.octave + "|" + thisKey.midiNoteName);
                }

                PianoKey previousKey = null;

                if(k > 1)
                {
                    previousKey = keys[keys.Count - 2];
                    thisKey.note = thisKey.GetNextNoteOfSameColor(previousKey.note);
                    Console.WriteLine("We just created a black key of note " + thisKey.note);

                    if(thisKey.note == "F#" || thisKey.note == "C#")
                    {
                        //we need extra spacing for these
                        startingBlackKeyX += whiteKeyincrement;
                        thisKey.octave = previousKey.octave;
                        thisKey.midiNoteNumber = previousKey.midiNoteNumber + 3;
                        thisKey.midiNoteName = thisKey.note + thisKey.octave;
                        Console.WriteLine(thisKey.note + "|" + thisKey.octave + "|" + thisKey.midiNoteName);

                    } else if (thisKey.note == "A#")
                    {
                        thisKey.octave = previousKey.octave + 1;
                        thisKey.midiNoteName = thisKey.note + thisKey.octave;
                        thisKey.midiNoteNumber = previousKey.midiNoteNumber + 2;
                        Console.WriteLine(thisKey.note + "|" + thisKey.octave + "|" + thisKey.midiNoteName);
                    } else
                    {
                        thisKey.octave = previousKey.octave;
                        thisKey.midiNoteName = thisKey.note + thisKey.octave;
                        thisKey.midiNoteNumber = previousKey.midiNoteNumber + 2;
                        Console.WriteLine(thisKey.note + "|" + thisKey.octave + "|" + thisKey.midiNoteName);
                    }
                }

                thisKey.height = blackKeyHeight;
                thisKey.width = whiteKeyincrement / 2;
                thisKey.xValue = startingBlackKeyX;
                thisKey.yValue = blackKeyY;


                Button b = new Button();
                b.Tag = thisKey;

                b.Click += handleTheClick;

                b.Height = blackKeyHeight;
                b.Width = whiteKeyincrement / 2;
                b.Location = new Point(startingBlackKeyX, blackKeyY);
                b.BackColor = Color.Black;
                Console.WriteLine("We just created a new button at (" + startingBlackKeyX + "," + blackKeyY + ")");
                b.Visible = true;
                Controls.Add(b);
                pianoKeyControls.Add(b);
                startingBlackKeyX += whiteKeyincrement;
                b.BringToFront();
                keys[keys.Count - 1].keyButton = b;

                blackKeys.Add(keys[keys.Count - 1]);


            }

            //The keys are created in order of White -> Black (low to high)
            //We just want them low to high so let's reorder them

            keys.OrderBy(key => key.midiNoteNumber);
            //Now let's reorder the ID's

            /*
            int counter = 1;

            foreach(PianoKey k in keys)
            {
                k.id = counter;
                counter++;
            }
            */

            this.Update();
        }

        

        private void buttonA_Click(object sender, EventArgs e)
        {
            selectedNote = "A";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonB_Click(object sender, EventArgs e)
        {
            selectedNote = "B";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonC_Click(object sender, EventArgs e)
        {
            selectedNote = "C";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonD_Click(object sender, EventArgs e)
        {
            selectedNote = "D";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonE_Click(object sender, EventArgs e)
        {
            selectedNote = "E";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonF_Click(object sender, EventArgs e)
        {
            selectedNote = "F";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonG_Click(object sender, EventArgs e)
        {
            selectedNote = "G";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonASharp_Click(object sender, EventArgs e)
        {
            selectedNote = "A#";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonCSharp_Click(object sender, EventArgs e)
        {
            selectedNote = "C#";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonDSharp_Click(object sender, EventArgs e)
        {
            selectedNote = "D#";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonFSharp_Click(object sender, EventArgs e)
        {
            selectedNote = "F#";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonGSharp_Click(object sender, EventArgs e)
        {
            selectedNote = "G#";
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void CheckForMatch()
        {
            labelResult.Visible = true;

            if(playScale)
            {
                labelResult.Text += ", " + selectedNote;
                this.Update();
            } else if(playNote) //if we aren't playing the same
            {
                Console.WriteLine("we are in the else...");
                if (selectedNote == randomNote)
                {

                    if (GetFlatVersion(randomNote) != "None")
                    {
                        randomNote = randomNote + " (or " + GetFlatVersion(randomNote) + ")";
                    }

                    labelResult.Text = "Correct. The note was " + randomNote;
                    labelResult.ForeColor = Color.Lime;
                    correctAnswers++;
                    labelCorrectAnswers.Text = "Correct Answers: " + correctAnswers;
                }
                else
                {

                    if (GetFlatVersion(randomNote) != "None")
                    {
                        randomNote = randomNote + " (or " + GetFlatVersion(randomNote) + ")";
                    }

                    labelResult.Text = "Wrong. You guessed " + selectedNote + " and it was " + randomNote;
                    labelResult.ForeColor = Color.Red;
                    wrongAnswers++;
                    labelWrongAnswers.Text = "Wrong Answers: " + wrongAnswers;
                    MessageBox.Show("Wrong. You guessed " + selectedNote + " and it was " + randomNote);

                }

                UpdateTime();
                //PickKeyAtRandom(testingKeyColor);
            } else if (playChord)
            {
                if(selectedNote == randomNote)
                {
                    labelResult.Text = "Correct. The note was " + randomNote;
                    labelResult.ForeColor = Color.Lime;
                    correctAnswers++;
                    labelCorrectAnswers.Text = "Correct Answers: " + correctAnswers;
                } else
                {
                    labelResult.Text = "Wrong. You guessed " + selectedNote + " and it was " + randomNote;
                    labelResult.ForeColor = Color.Red;
                    wrongAnswers++;
                    labelWrongAnswers.Text = "Wrong Answers: " + wrongAnswers;
                    MessageBox.Show("Wrong. You guessed " + selectedNote + " and it was " + randomNote);
                }
            }

            if(!staffGame && !chordGame)
            {
                Console.WriteLine("not staff game and not chord game");
                PickKeyAtRandom(testingKeyColor);
            } else if(chordGame)
            {
                Console.WriteLine("Chord game");

                foreach(PianoKey key in correctAnswerChord.keys)
                {

                    if(key.color == "White")
                    {
                        key.keyButton.BackColor = Color.White;
                    } else
                    {
                        key.keyButton.BackColor = Color.Black;
                    }

                    
                }

                PickRandomChord();
                
            } else if(staffGameType == "note")
            {
                //draw over the current note
                //g.DrawRectangle(backgroundColorPen, 0, 0, 1000, 1000);
                this.Refresh();
                DrawStaff();
                PickStaffNoteAtRandom();
                this.Update();

                //PickStaffNoteAtRandom();
            } else if(staffGameType == "chord")
            {
                this.Refresh();
                DrawStaff();
                PickStaffChordAtRandom();
                this.Update();
            }

            
        }

        private void buttonAFlat_Click(object sender, EventArgs e)
        {
            selectedNote = NoteConversion("Ab");
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonBFlat_Click(object sender, EventArgs e)
        {
            selectedNote = NoteConversion("Bb");
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonDFlat_Click(object sender, EventArgs e)
        {
            selectedNote = NoteConversion("Db");
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonEFlat_Click(object sender, EventArgs e)
        {
            selectedNote = NoteConversion("Eb");
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        private void buttonGflat_Click(object sender, EventArgs e)
        {
            selectedNote = NoteConversion("Gb");
            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == selectedNote));
            CheckForMatch();
        }

        public string NoteConversion(string thisNote)
        {
            string returnString = "";

            if(thisNote == "Ab")
            {
                returnString = "G#";
            } else if (thisNote == "Bb")
            {
                returnString = "A#";
            }
            else if (thisNote == "Db")
            {
                returnString = "C#";
            }
            else if (thisNote == "Eb")
            {
                returnString = "D#";
            }
            else if (thisNote == "Gb")
            {
                returnString = "F#";
            }

            return returnString;
        }

        public string GetFlatVersion(string thisNote)
        {

            string returnString = "None";

            if(thisNote.Contains("#"))
            {
                if(thisNote == "A#")
                {
                    returnString = "Bb";
                } else if(thisNote == "C#")
                {
                    returnString = "Db";
                }
                else if (thisNote == "D#")
                {
                    returnString = "Eb";
                }
                else if (thisNote == "F#")
                {
                    returnString = "Gb";
                }
                else if (thisNote == "G#")
                {
                    returnString = "Ab";
                }
            }

            return returnString;
        }

        private void buttonAllKeys_Click(object sender, EventArgs e)
        {
            testingKeyColor = "All";
            playScale = false;
            playNote = true;
            PickKeyAtRandom(testingKeyColor);
        }

        private void buttonBlackKeysOnly_Click(object sender, EventArgs e)
        {
            testingKeyColor = "Black";
            playScale = false;
            playNote = true;
            PickKeyAtRandom(testingKeyColor);
        }

        private void buttonWhiteKeysOnly_Click(object sender, EventArgs e)
        {
            testingKeyColor = "White";
            playScale = false;
            playNote = true;
            PickKeyAtRandom(testingKeyColor);
        }

        private void buttonPlayTheNote_Click(object sender, EventArgs e)
        {
            labelKeyToPress.Visible = true;
            playScale = false;
            playNote = true;
            randomNote = GetRandomKey(keys).note;

            labelKeyToPress.Text = "Click a " + randomNote;
        }

        public PianoKey GetRandomKey(List<PianoKey> keyListing)
        {
            //int r = rnd.Next(keyListing.Count);

            //keyListing.OrderBy(k => k.midiNoteNumber);

            /*
             * The first note is C0 which is midi number 24
             * Our last note is C8 which is 106
             * */

            int minMidiNumber = keyListing.Min(k => k.midiNoteNumber);
            int maxMidiNumber = keyListing.Max(k => k.midiNoteNumber);

            Console.WriteLine("The min possible is: " + minMidiNumber + " and the max is " + maxMidiNumber);



            int r = 0;

            //Currently it's getting stuck on the left... fix that
            int lowerBound = currentRandomNumber - 4;
            int upperBound = currentRandomNumber + 7;

            if (lowerBound < minMidiNumber)
            {
                r = rnd.Next(minMidiNumber+10, minMidiNumber + 20);
                Console.WriteLine("We are below the min bound");
            } else if(upperBound > maxMidiNumber)
            {
                r = rnd.Next(minMidiNumber, minMidiNumber + 20);
                Console.WriteLine("We are above the max bound");
            } else
            {
                r = rnd.Next(lowerBound, upperBound);
                Console.WriteLine("The lower bound is : " + lowerBound + " and the upper bound is " + upperBound);
            }


            Console.WriteLine("The random number is: " + r);


            //If the number is the same as last time... try again
            if(currentRandomNumber == r)
            {
                GetRandomKey(keyListing);
            }

            //int r = 0;

            //We want to move to the right until we get close to the end, then start back over
            /*
            if(currentRandomNumber > 45)
            {
                r = rnd.Next(1, 10);
            } else
            {
                r = rnd.Next(currentRandomNumber, keyListing.Count);
            }
            */

            //Instead let's try it can only move within one octave in either direction

            //int r = rnd.Next(currentRandomNumber,keyListing.Count);
            //Console.WriteLine(keyListing[r].note + " " + keyListing[r].id);

            //PianoKey returnKey = keyListing[keyListing.Count-r];
            PianoKey returnKey = keyListing.Find(k => k.midiNoteNumber == r);

            currentRandomNumber = r;

            /*
            foreach(PianoKey k in keyListing)
            {
                Console.WriteLine(k.id + "|"+ k.midiNoteNumber + "|" + k.midiNoteName + "|" + r);
            }
            */


            Console.WriteLine("The randomly selected note is : " + returnKey.midiNoteName);
            Console.WriteLine("The randomly selected note midi number is : " + returnKey.midiNoteNumber);


            Console.WriteLine("The current random number is " + r);
           // Console.WriteLine("The current midi note is: " + returnKey.midiNoteNumber);

            return returnKey;
        }

        private void handleTheClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            PianoKey clickedKey = (PianoKey) btn.Tag;
            selectedNote = clickedKey.note;

            //keysInScale.Add(clickedKey);

            keysInScale.Add(keys.Find(item => item.octave == 4 && item.note == clickedKey.note));

            Console.WriteLine("You click a button: " + clickedKey.note + " " + clickedKey.midiNoteName + " " + clickedKey.midiNoteNumber);
            string noteClicked = clickedKey.note;

            //if we aren't playing a scale
            if(playNote)
            {
                if (noteClicked == randomNote)
                {
                    correctAnswers++;
                    //MessageBox.Show("Correct");
                }
                else
                {
                    wrongAnswers++;
                    MessageBox.Show("Wrong");
                }

                randomNote = GetRandomKey(keys).note;

                SetGradeText();

                
            } else if(playScale)
            {
                CheckForMatch();
            }

            this.Update();



        }

        public void SetGradeText()
        {
            labelCorrectAnswers.Text = "Correct Answers: " + correctAnswers;
            labelWrongAnswers.Text = "Incorrect Answers: " + wrongAnswers;
            labelKeyToPress.Text = "Click a " + randomNote;
            this.Update();
        }

        private void buttonPlayScale_Click(object sender, EventArgs e)
        {
            playScale = true;
            keysInScale.Clear();
            labelCorrectScaleKeys.Visible = false;
            labelYourScaleAnswer.Visible = false;

            PianoKey randomKey = GetRandomKey(keys);

            //get the notes in that scale
            keysInRequestedScale = GetKeysInScale(randomKey.note, "Major").ToList();

            labelKeyToPress.Text = "Play the " + randomKey.note + " Major Scale";
            labelKeyToPress.Visible = true;

            this.Update();
        }

        private void buttonSubmitScale_Click(object sender, EventArgs e)
        {

            gradeScale = true;
            CheckForMatch();
            Boolean isCorrect = true;

            labelCorrectScaleKeys.Visible = true;
            labelCorrectScaleKeys.Text = "Correct Scales in: " + keysInRequestedScale[0].note + " Major: ";

            labelYourScaleAnswer.Visible = true;
            labelYourScaleAnswer.Text = "Your guess in " + keysInRequestedScale[0].note + " Major:       ";

            int counter = 0;

            foreach(PianoKey k in keysInRequestedScale)
            {
                if(counter == 0)
                {
                    labelCorrectScaleKeys.Text += k.note;
                } else
                {
                    labelCorrectScaleKeys.Text += ", " + k.note;
                }

                if(!k.note.Contains("#") && !k.note.Contains("b"))
                {
                    labelCorrectScaleKeys.Text += " ";
                }

                counter++;
                    
            }

            counter = 0;

            foreach (PianoKey k in keysInScale)
            {
                if (counter == 0)
                {
                    labelYourScaleAnswer.Text += k.note;
                }
                else
                {
                    labelYourScaleAnswer.Text += ", " + k.note;
                }

                if (!k.note.Contains("#") && !k.note.Contains("b"))
                {
                    labelYourScaleAnswer.Text += " ";
                }

                counter++;

            }


            //if they have the same # of keys or if our guess is 1 short (we didn't put the root back in at the end)
            if (keysInScale.Count == keysInRequestedScale.Count || keysInRequestedScale.Count == (keysInScale.Count + 1))
            {
                for (int i = 0; i < keysInScale.Count; i++)
                {
                    Console.WriteLine(keysInScale[i].note + "|" + keysInRequestedScale[i].note);
                    if (keysInScale[i].note != keysInRequestedScale[i].note || keysInRequestedScale[i] is null)
                    {
                        isCorrect = false;
                    }
                }
            } else
            {
                isCorrect = false;
                Console.WriteLine("Counts dont match");
            }
            


            if (isCorrect)
            {
                MessageBox.Show("Correct");
                
            } else
            {
                MessageBox.Show("Wrong");
            }
            


            keysInScale.Clear();
            keysInRequestedScale.Clear();
            labelResult.Text = "Scale: ";
            this.Update();
        }

        public List<PianoKey> GetKeysInScale(string rootNote, string scaleType) {

            List<PianoKey> returnKeys = new List<PianoKey>();

            returnKeys = scales.Find(item => item.GetRootKey().note == rootNote && item.scaleType == scaleType).keys.ToList();

            return returnKeys;


        }

        public List<PianoKey> GetKeysInChord(string rootNote, string chordType)
        {

            List<PianoKey> returnKeys = new List<PianoKey>();

            returnKeys = chords.Find(item => item.GetRootKey().note == rootNote && item.chordType == chordType).keys.ToList();

            return returnKeys;


        }

        private void UpdateTime() { 
            DateTime nowTime = DateTime.Now;

            TimeSpan span = nowTime.Subtract(startDateTime);

            double averageTime = Math.Round(span.TotalSeconds / correctAnswers, 2);

            labelShowTime.Text = Math.Round(span.TotalMinutes,0) + " minutes and " + Math.Round(span.TotalSeconds,0) + " seconds\nAn average of: " + averageTime + " seconds per correct guess";

            this.Update();

        }

        private void buttonResetTimer_Click(object sender, EventArgs e)
        {
            startDateTime = DateTime.Now;
            correctAnswers = 0;
            wrongAnswers = 0;


            SetGradeText();
            UpdateTime();
        }

        private void buttonGuessTheChord_Click(object sender, EventArgs e)
        {
            PickRandomChord();



            /*
            Console.WriteLine("printing all the chords...");

            foreach(Chord chord in chords)
            {
                Console.WriteLine(chord.rootNote + "|" + chord.chordType);
            }


            Console.WriteLine("Done printing all the chords");
            */

        }

        public void PickRandomChord()
        {
            PianoKey randomKey = GetRandomKey(keys);

            var random = new Random();
            int index = random.Next(chordTypes.Count);

            string chordType = chordTypes[index];

            //get the notes in that scale
            keysInRequestedChord = GetKeysInChord(randomKey.note, chordType).ToList();
            correctAnswerChord = chords.Find(i => i.chordName == keysInRequestedChord[0].note + " " + chordType);
            randomNote = chords.Find(i => i.chordName == keysInRequestedChord[0].note + " " + chordType).chordName;

            //Console.WriteLine("The answer is: " + correctAnswerChord.chordName);

            Console.WriteLine("Printing the chord: " + keysInRequestedChord[0].note + " " + chordType);

            foreach (PianoKey key in keysInRequestedChord)
            {
                Console.WriteLine(key.note);
                key.keyButton.BackColor = Color.Green;
            }
        }

        private void buttonTestCode_Click(object sender, EventArgs e)
        {

            foreach (Control c in pianoKeyControls)
            {
                c.Visible = false;
            }

            /*
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(42, 0, 800, 800));
            myBrush.Dispose();
            formGraphics.Dispose();
            */

            


            foreach (Scale scale in scales)
            {
                int interval = 0;
                Console.WriteLine("The scale is: " + scale.rootNote + " " + scale.scaleType);
                Console.WriteLine("The 4th interval is: " + scale.GetNoteAtInterval(4).midiNoteName);
                //scale.PrintScale();
                /*
                foreach(PianoKey key in scale.keys)
                {
                    interval++;
                    Console.WriteLine(key.midiNoteName + " Interval: " + interval);
                }
                */
            }
        }

        private void buttonShowKeyboard_Click(object sender, EventArgs e)
        {
            FlipKeyVisibility();
        }

        public void FlipKeyVisibility()
        {
            foreach (Control c in pianoKeyControls)
            {
                if(c.Visible)
                {
                    c.Visible = false;
                } else
                {
                    c.Visible = true;
                }
            }
        }

        public void HideKeys()
        {
            foreach (Control c in pianoKeyControls)
            {
                    c.Visible = false;
            }
        }

        private void buttonShowStaff_Click(object sender, EventArgs e)
        {
            staffGameType = "note";
            DrawStaff();
            PickStaffNoteAtRandom();

        }

        private void DrawStaff()
        {
            //hide the keys

            HideKeys();

            //Pen pen = new Pen(Color.White);
            whitePen.Width = 5;
            int yCoord = trebleStaffStartingYCoordinate;

            //Treble clef
            for (int i = 1; i <= 5; i++)
            {
                //x1, y1, x2, y2
                g.DrawLine(whitePen, 50, yCoord, 1600, yCoord);
                yCoord = yCoord + noteSize;
            }

            //We already incremented it 40 from the end of the for loop above and we need 80 so just do 40 more
            yCoord = yCoord + noteSize;
            //Now the bass clef...

            for (int i = 1; i <= 5; i++)
            {
                //x1, y1, x2, y2
                g.DrawLine(whitePen, 50, yCoord, 1600, yCoord);
                yCoord = yCoord + noteSize;
            }

            List<PianoKey> WhiteKeysDesc = whiteKeys.OrderByDescending(o => o.midiNoteNumber).ToList();

            Boolean notesOnStaff = false;
            int thisY = 0;

            foreach (PianoKey key in WhiteKeysDesc)
            {
                Console.WriteLine(key.midiNoteName);
                if (key.midiNoteName == "F6")
                {
                    notesOnStaff = true;
                    key.SetStaffRectangle(new Rectangle(1000, thisY, noteSize, noteSize),thisY);
                    keysOnStaff.Add(key);
                }
                else if (key.midiNoteName == "G1")
                {
                    notesOnStaff = false;
                }
                else if (notesOnStaff)
                {
                    thisY += 20;
                    key.SetStaffRectangle(new Rectangle(1000, thisY, noteSize, noteSize),thisY);
                    keysOnStaff.Add(key);
                }
            }


            //Now do the black keys..
                List<PianoKey> BlackKeysDesc = blackKeys.OrderByDescending(o => o.midiNoteNumber).ToList();


                foreach (PianoKey key in BlackKeysDesc)
                {




                Console.WriteLine(key.midiNoteName);

                if (key.midiNoteName == "F#6")
                {
                    notesOnStaff = true;
                    PianoKey whiteKey = whiteKeys.Find(i => i.midiNoteName == key.midiNoteName.Replace("#",string.Empty));

                    key.SetStaffRectangle(whiteKey.staffRectangle, whiteKey.staffRectangle.Y);
                    keysOnStaff.Add(key);
                }
                else if (key.midiNoteName == "G#1")
                {
                    notesOnStaff = false;
                }
                else if (notesOnStaff)
                {
                    PianoKey whiteKey = whiteKeys.Find(i => i.midiNoteName == key.midiNoteName.Replace("#", string.Empty));

                    key.SetStaffRectangle(whiteKey.staffRectangle, whiteKey.staffRectangle.Y);
                    keysOnStaff.Add(key);
                }
            }

            //PickStaffNoteAtRandom();
        }

        //Currently the old note doesn't go away and the new note doesn't come out automatically
        public void PickStaffNoteAtRandom()
        {
            staffGame = true;
            //It won't check anything if we don't set this, we need to make the CheckForMatch() method more complex
            playNote = true;


            var rand = new Random();
            //PianoKey thisKey = keysOnStaff[rand.Next(keysOnStaff.Count)];
            currentKeyInQuestion = keysOnStaff[rand.Next(keysOnStaff.Count)];
            randomNote = currentKeyInQuestion.note;

            //Console.WriteLine("The random key is: " + thisKey.midiNoteName);

            g.DrawEllipse(whitePen, currentKeyInQuestion.staffRectangle);
            //CheckForMatch();
        }

        private void buttonChordsOnStaff_Click(object sender, EventArgs e)
        {
            staffGameType = "chord";
            DrawStaff();
            PickStaffChordAtRandom();
        }

        public void PickStaffChordAtRandom()
        {
            staffGame = true;
            //It won't check anything if we don't set this, we need to make the CheckForMatch() method more complex
            playNote = true;


            var rand = new Random();
            //PianoKey thisKey = keysOnStaff[rand.Next(keysOnStaff.Count)];
            currentKeyInQuestion = keysOnStaff[rand.Next(keysOnStaff.Count)];
            randomNote = currentKeyInQuestion.note;

            Chord chordInQuestion = majorChords.Find(i => i.rootNote == randomNote);
            chordInQuestion.PrintChord();

            foreach(PianoKey key in chordInQuestion.keys)
            {
                g.DrawEllipse(whitePen, key.staffRectangle);

                if(key.midiNoteName.Contains("#"))
                {
                    Console.WriteLine("We have a sharp...");
                    Console.WriteLine(key.rectangleY);
                    Console.WriteLine(key.staffRectangle.ToString());
                    g.DrawString("#", new Font("Arial", 24, FontStyle.Bold), SystemBrushes.ControlLight, new PointF(key.staffRectangle.X-20, key.staffRectangle.Y));
                    //g.DrawString("#", new Font("Arial", 24, FontStyle.Bold), SystemBrushes.ControlText, new PointF(200, 50));
                    
                }

            }

            //Console.WriteLine("The random key is: " + thisKey.midiNoteName);

            //g.DrawEllipse(whitePen, currentKeyInQuestion.staffRectangle);
            //CheckForMatch();
        }



        private void buttonPlayChord_Click(object sender, EventArgs e)
        {

        }

        private void buttonSubmitChord_Click(object sender, EventArgs e)
        {
            playChord = true;
            chordGame = true;
            selectedNote = listBoxNotes.GetItemText(listBoxNotes.SelectedItem) + " " + listBoxChordType.GetItemText(listBoxChordType.SelectedItem);
            CheckForMatch();
        }

        private void buttonGuessScale_Click(object sender, EventArgs e)
        {
            playScale = true;
            keysInScale.Clear();
            labelCorrectScaleKeys.Visible = false;
            labelYourScaleAnswer.Visible = false;

            PianoKey randomKey = GetRandomKey(keys);

            //get the notes in that scale
            keysInRequestedScale = GetKeysInScale(randomKey.note, "Major").ToList();

            foreach(PianoKey key in keysInRequestedScale)
            {
                key.keyButton.BackColor = Color.Green;
            }

            //labelKeyToPress.Text = "Play the " + randomKey.note + " Major Scale";
            labelKeyToPress.Visible = true;

            this.Update();
        }






        /*
        private void OnTimedEvent(object sender, EventArgs e)
        {
            Console.WriteLine("we are here...");
            if (timeLeft > 0)
            {
                timeLeft -= 1000;
                Console.WriteLine("There are: " + (timeLeft/1000) + " seconds left");
                labelTimeRemaining.Text = timeLeft + " seconds remaining";
                this.Update();
            }
            else
            {
                Console.WriteLine("Time is up. You got " + correctAnswers + " correct answers.");
                timer1.Stop();
            }
        }
        */
    }
}
