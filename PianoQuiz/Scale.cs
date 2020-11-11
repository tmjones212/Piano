using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoQuiz
{
    public class Scale
    {
        //Major, Minor, etc.
        public string scaleType { get; set; }
        public List<PianoKey> keys { get; set; }
        public List<PianoKey> allKeys { get; set; }

        public static string[] majorIntervalString = new string[] { "W", "W", "H", "W", "W", "W", "H" };

        public string rootNote;
        public PianoKey rootKey;
        public string[] intervalString;
        public Scale(string tmpScaleType, string tmpRootNote,List<PianoKey> tmpKeys)
        {
            keys = new List<PianoKey>();
            allKeys = new List<PianoKey>();
            scaleType = tmpScaleType;
            rootNote = tmpRootNote;
            allKeys = tmpKeys.ToList();
            SetIntervalString();
            SetScaleNotes();
            PrintScale();
        }

        public PianoKey GetRootKey()
        {
            return keys[0];
        }

        public PianoKey GetThirdKey()
        {
            return keys[2];
        }

        public PianoKey GetFifthKey()
        {
            return keys[4];
            return keys[4];
            return keys[4];
            return keys[4];
        }


        public void PrintScale()
        {
            Console.WriteLine("Scale: " + rootNote + " " + scaleType);

            foreach(PianoKey key in keys)
            {
                Console.WriteLine(key.midiNoteName);
            }
        }

        public void SetScaleNotes()
        {
            int counter = 0;
            PianoKey previousKeyInScale = null;

            foreach (PianoKey key in allKeys)
            {
                if (counter == 0 && key.note == rootNote && key.octave == 4)
                {
                    Console.WriteLine("Adding the root note + " + key.note + "...");
                    keys.Add(key);
                    previousKeyInScale = key;
                    counter++;
                }
            }

            foreach (string interval in intervalString)
            {
                Console.WriteLine(interval);
            }

            foreach(PianoKey k in allKeys)
            {
                Console.WriteLine(k.midiNoteNumber);
            }

            counter = 0;

            foreach (string interval in intervalString)
            {
                Console.WriteLine(counter);
                if(intervalString[counter] == "W")
                {
                    Console.WriteLine("Adding a whole step...");
                    keys.Add(allKeys.Find(item => item.midiNoteNumber == (previousKeyInScale.midiNoteNumber + 2)));
                    Console.WriteLine(keys.Count + " keys in the scale");
                    //previousKeyInScale = keys[keys.Count - 1];
                } else if(intervalString[counter] == "H")
                {
                    Console.WriteLine("Adding a half step...");
                    keys.Add(allKeys.Find(item => item.midiNoteNumber == (previousKeyInScale.midiNoteNumber + 1)));
                    Console.WriteLine(keys.Count + " keys in the scale");
                    //previousKeyInScale = keys[keys.Count - 1];
                }

                Console.WriteLine("There are: " + keys.Count + " Keys in the scale right now");

                Console.WriteLine("Just added the note: " + keys[keys.Count - 1].note);
                previousKeyInScale = keys[keys.Count - 1];
                counter++;
            }
        }

        public void SetIntervalString()
        {
            if(scaleType == "Major")
            {
                string[] tmpIntervalString = new string[] { "W","W","H","W","W","W","H"};
                intervalString = tmpIntervalString;
            } else
            {
                intervalString = GetMajorScaleDerivativeInterval(scaleType);
            }

            /*else if(scaleType == "Minor")
            {
                string[] tmpIntervalString = new string[] { "H", "W", "W", "H", "W", "W", "W" };
                intervalString = tmpIntervalString;
            }
            */
        }

        public PianoKey GetNoteAtInterval(int thisInterval)
        {
            return keys[thisInterval-1];
        }

        public string[] GetMajorScaleDerivativeInterval(string scaleDescription)
        {
            int index = 0;

            if(scaleDescription == "Minor")
            {
                index = 6;
            } else if(scaleDescription == "Dorian")
            {
                index = 2;
            }
            else if (scaleDescription == "Phrygian")
            {
                index = 3;
            }
            else if (scaleDescription == "Lydian")
            {
                index = 4;
            }
            else if (scaleDescription == "Mixolydian")
            {
                index = 5;
            }
            else if (scaleDescription == "Locrian")
            {
                index = 7;
            }
            

            index -= 1;
            Console.WriteLine("index is: " + index);

            string[] thisString = majorIntervalString;

            string[] returnString = new string[7];

            int counter = 0;
            int restartCounter = 0;

            Console.WriteLine("Setting it up for " + rootNote + "|" + scaleType);

            for (int i=0;i<returnString.Length;i++)
            {
                //{ "W","W","H","W","W","W","H"};
                //   0   1   2   3   4   5   6
                if (i == 0)
                {
                    Console.WriteLine("i = 0 setting to: " + thisString[index]);
                    returnString[i] = thisString[index];
                } else if(i+index < thisString.Length)
                {
                    returnString[i] = thisString[i+index];
                } else
                {
                    returnString[counter] = thisString[restartCounter];
                    restartCounter++;
                }

                /*I thought this worked with minor but it's not working for phyrgian?
                if (i == 0)
                {
                    Console.WriteLine("i = 0 setting to: " + thisString[index]);
                    returnString[i] = thisString[index];
                }
                else if (i + index < thisString.Length)
                {
                    Console.WriteLine("in the else if i = " + i + " setting to " + thisString[i + 1] + " " + (i + index) + " vs " + thisString.Length);
                    returnString[counter] = thisString[i + 1];
                }
                else
                {
                    Console.WriteLine("in the else i = " + i + " setting to " + thisString[restartCounter]);
                    returnString[counter] = thisString[restartCounter];
                    restartCounter++;
                }
                */

                counter++;
            }

            foreach(string s in returnString)
            {
                Console.WriteLine(s);
            }


            return returnString;
            
        }
    }
}
