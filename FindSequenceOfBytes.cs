/*
    2015 Ingolf Hill http://www.werferstein.org

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

    Dieses Programm ist Freie Software: Sie können es unter den Bedingungen
    der GNU General Public License, wie von der Free Software Foundation,
    Version 3 der Lizenz oder (nach Ihrer Wahl) jeder neueren
    veröffentlichten Version, weiter verteilen und/oder modifizieren.

    Dieses Programm wird in der Hoffnung bereitgestellt, dass es nützlich sein wird, jedoch
    OHNE JEDE GEWÄHR,; sogar ohne die implizite
    Gewähr der MARKTFÄHIGKEIT oder EIGNUNG FÜR EINEN BESTIMMTEN ZWECK.
    Siehe die GNU General Public License für weitere Einzelheiten.

    Sie sollten eine Kopie der GNU General Public License zusammen mit diesem
    Programm erhalten haben. Wenn nicht, siehe <https://www.gnu.org/licenses/>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindStringInFile
{
   

    /// <summary>
    /// Boyer–Moore string-search algorithm
    /// https://de.wikipedia.org/wiki/Boyer-Moore-Algorithmus
    /// https://en.wikipedia.org/wiki/Boyer%E2%80%93Moore_string-search_algorithm
    /// </summary>
    public sealed class FindSequenceOfBytes
    {


        readonly byte[] BytePattern;
        readonly int[] charTable;
        readonly int[] offsetTable;


        public FindSequenceOfBytes(byte[] bPattern)
        {
            this.BytePattern = bPattern;
            this.charTable = MakeByteTable(bPattern);
            this.offsetTable = MakeOffsetTable(bPattern);
        }




        public IEnumerable<int> Search(byte[] haystack)
        {
            if (BytePattern.Length == 0)
                yield break;

            for (int i = BytePattern.Length - 1; i < haystack.Length;)
            {
                int j;

                for (j = BytePattern.Length - 1; BytePattern[j] == haystack[i]; --i, --j)
                {
                    if (j != 0)
                        continue;

                    yield return i;
                    i += BytePattern.Length - 1;
                    break;
                }

                i += Math.Max(offsetTable[BytePattern.Length - 1 - j], charTable[haystack[i]]);
            }
        }



        #region Bad-Character Tabelle Sprungberechnung
        /*


*/
        //Bad-Character Tabelle Sprungberechnung
        static int[] MakeByteTable(byte[] bytePattern)
        {
            const int ALPHABET_SIZE = 256;

            //int skipTable in der Größe ALPHABET_SIZE
            int[] skipTable = new int[ALPHABET_SIZE];

            //Alle Felder der skipTable mit der Länge des max. Sprung füllen
            for (int i = 0; i < skipTable.Length; ++i)
            {
                skipTable[i] = bytePattern.Length;
            }

            //Die Tabelle bytePattern duchlaufen und in der skipTable die Werte der bytePattern als Pos -1 schreiben
            for (int i = 0; i < bytePattern.Length - 1; ++i)
            {
                skipTable[
                        //Pos 0 - 256
                        //Bytewert aus den Suchpattern je pos.
                        bytePattern[i]
                     ]
                    //-1 erster Wert(max.) wurde schon vorher geschrieben                
                    = (bytePattern.Length - 1) - i;
            }


            //Für jeden Byte-Wert gibt es einen vorbestimmten Sprungwert 
            return skipTable;
        }
        #endregion

        #region Good-Character Tabelle Sprungberechnung
        static int[] MakeOffsetTable(byte[] bytePattern)
        {
            //Tabelle in der Größe des BytePatterns
            int[] table = new int[bytePattern.Length];

            //Auf das letzte Zeichen setzen
            int lastPrefixPosition = bytePattern.Length;

            //loop von letzter Pos. im Pattern
            for (int i = bytePattern.Length - 1; i >= 0; --i)
            {
                if (IsPrefix(bytePattern, i + 1))
                { lastPrefixPosition = i + 1; }
#if DEBUG
                Console.WriteLine("table[" + (bytePattern.Length - 1 - i).ToString() + "] = " + (lastPrefixPosition - i + bytePattern.Length - 1).ToString());
#endif                            
                table[bytePattern.Length - 1 - i] = lastPrefixPosition - i + bytePattern.Length - 1;
                byte[] bytes = table.SelectMany(BitConverter.GetBytes).ToArray();
#if DEBUG
                Console.WriteLine(string.Join(",", table));
                Console.WriteLine("--------------------------------------");
#endif 
            }

            for (int i = 0; i < bytePattern.Length - 1; ++i)
            {
                int slen = SuffixLength(bytePattern, i);
                table[slen] = bytePattern.Length - 1 - i + slen;
            }
            return table;
        }

        //Ist das Zeichen aus dem Pattern[p] gleich dem Präfix?
        static bool IsPrefix(byte[] bytePattern, int p)
        {
#if DEBUG
            Console.WriteLine("********************************************");
            Console.WriteLine("p:" + p.ToString());
#endif  
            for (int i = p, j = 0; i < bytePattern.Length; ++i, ++j)
            {
#if DEBUG
                Console.WriteLine("p[" + i.ToString() + "]:" + bytePattern[i].ToString() + " !=  p[" + j.ToString() + "]:" + bytePattern[j].ToString());
#endif
                if (bytePattern[i] != bytePattern[j])
                {
#if DEBUG
                    Console.WriteLine("false");
                    Console.WriteLine("********************************************");
#endif
                    return false;
                }

            }
#if DEBUG
            Console.WriteLine("true");
            Console.WriteLine("********************************************");
#endif
            return true;
        } 
        #endregion

        static int SuffixLength(byte[] bytePattern, int p)
        {
            int len = 0;

            for (int i = p, j = bytePattern.Length - 1; i >= 0 && bytePattern[i] == bytePattern[j]; --i, --j)
                ++len;

            return len;
        }
    }
}
