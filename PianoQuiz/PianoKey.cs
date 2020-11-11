using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PianoQuiz
{
    public class PianoKey
    {
        public static int keyCounter;
        public string color { get; set; }
        public Color keyColor { get; set; }
        public string note { get; set; }
        public int id { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int xValue { get; set; }
        public int yValue { get; set; }
        public Button keyButton { get; set; }

        public int midiNoteNumber { get; set; }
        public string midiNoteName { get; set; }
        public int octave { get; set; }
        public Rectangle staffRectangle { get; set; }
        public Boolean onStaff { get; set; }
        public int rectangleY { get; set; }
        public PianoKey()
        {
            id = keyCounter;
            keyCounter++;
            SetNote();

            //by default this is false, unless we set it manually in SetStaffRectangle
            onStaff = false;

        }

        //Not sure how we're going to do this yet...
        public void SetStaffRectangle(Rectangle r, int tmpRectY)
        {
            staffRectangle = r;
            rectangleY = tmpRectY;
            onStaff = true;
        }

        

        public System.Drawing.Color GetOriginalKeyColor()
        {
            System.Drawing.Color result = System.Drawing.Color.Aqua;

            if (color == "White")
            {
                result = System.Drawing.Color.White;
            } else
            {
                result = System.Drawing.Color.Black;
            }

            return result;
        }

        public string GetPreviousNote(string n)
        {
            string returnString = "";

            if(n == "C")
            {
                returnString = "B";
            }
            else if (n == "D")
            {
                returnString = "C";
            }
            else if (n == "E")
            {
                returnString = "D";
            }
            else if (n == "F")
            {
                returnString = "E";
            }
            else if (n == "G")
            {
                returnString = "F";
            }
            else if (n == "A")
            {
                returnString = "G";
            }
            else if (n == "B")
            {
                returnString = "A";
            }

            return returnString;
        }

        public string GetNextNoteOfSameColor(string n)
        {
            string returnString = "";

            if (n == "C")
            {
                returnString = "D";
            }
            else if (n == "D")
            {
                returnString = "E";
            }
            else if (n == "E")
            {
                returnString = "F";
            }
            else if (n == "F")
            {
                returnString = "G";
            }
            else if (n == "G")
            {
                returnString = "A";
            }
            else if (n == "A")
            {
                returnString = "B";
            }
            else if (n == "B")
            {
                returnString = "C";
            }
            else if (n == "C#")
            {
                returnString = "D#";
            }
            else if (n == "D#")
            {
                returnString = "F#";
            }
            else if (n == "F#")
            {
                returnString = "G#";
            }
            else if (n == "G#")
            {
                returnString = "A#";
            }
            else if (n == "A#")
            {
                returnString = "C#";
            }

            return returnString;
        }

        public void SetNote()
        {
            if(keyCounter == 1) { note = "C";}
            else if(keyCounter == 2) { note = "D"; }
            else if (keyCounter == 3) { note = "E"; }
            else if (keyCounter == 4) { note = "F"; }
            else if (keyCounter == 5) { note = "G"; }
            else if (keyCounter == 6) { note = "A"; }
            else if (keyCounter == 7) { note = "B"; }
        }
    }
}
