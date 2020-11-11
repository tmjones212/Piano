using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoQuiz
{
    public class Chord
    {
        public string chordName { get; set; }
        public string chordType { get; set; }   //major vs. minor whatever
        public string chordDescription { get; set; }
        public string rootNote { get; set; }
        public List<PianoKey> keys { get; set; }
        public List<PianoKey> scaleKeys { get; set; }
        public List<PianoKey> allKeys { get; set; }
        public string chordString { get; set; }
        public Chord(string tmpRootNote, string tmpChordType,List<PianoKey> tmpScale,List<PianoKey> tmpAllKeys)
        {
            rootNote = tmpRootNote;
            chordType = tmpChordType;
            keys = new List<PianoKey>();
            scaleKeys = new List<PianoKey>();
            allKeys = new List<PianoKey>();

            allKeys = tmpAllKeys.ToList();

            scaleKeys = tmpScale.ToList();
            chordString = "";
            chordName = rootNote + " " + chordType;

            SetupChord();
        }

        public PianoKey GetRootKey()
        {
            return keys[0];
        }

        public void PrintChord()
        {
            chordString = "Chord: " + rootNote + " " + chordType;

            foreach(PianoKey key in keys)
            {
                chordString += " " + key.note;
            }

            Console.WriteLine(chordString);
        }

        public void SetupChord()
        {
            if(chordType == "Major" || chordType == "Minor")
            {
                keys.Add(scaleKeys[0]);
                keys.Add(scaleKeys[2]);
                keys.Add(scaleKeys[4]);
            } else if(chordType == "Augmented")
            {
                keys.Add(scaleKeys[0]);
                keys.Add(scaleKeys[2]);

                PianoKey keyToRaise = scaleKeys[4];

                foreach(PianoKey k in allKeys)
                {
                    //Console.WriteLine("Comparing : " + k.midiNoteNumber + " and " + keyToRaise.midiNoteNumber);
                    if(k.midiNoteNumber-1 == keyToRaise.midiNoteNumber) {
                        keys.Add(k);
                    }
                }

            }
            else if (chordType == "Diminished")
            {
                keys.Add(scaleKeys[0]);


                PianoKey keyToLower = scaleKeys[2];

                foreach (PianoKey k in allKeys)
                {
                    //Console.WriteLine("Comparing : " + k.midiNoteNumber + " and " + keyToRaise.midiNoteNumber);
                    if (k.midiNoteNumber + 1 == keyToLower.midiNoteNumber)
                    {
                        keys.Add(k);
                    }
                }


                keyToLower = scaleKeys[4];

                foreach (PianoKey k in allKeys)
                {
                    //Console.WriteLine("Comparing : " + k.midiNoteNumber + " and " + keyToRaise.midiNoteNumber);
                    if (k.midiNoteNumber + 1 == keyToLower.midiNoteNumber)
                    {
                        keys.Add(k);
                    }
                }

            }

        }
    }
}
