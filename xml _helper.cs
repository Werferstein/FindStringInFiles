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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace FindStringInFile
{
    [XmlRoot]
    public class Config
    {
        public Config()
        {
            NodePadPlusPath = string.Empty;
            NodePadPlusStart = string.Empty;

            NodePadPath = string.Empty;
            NodePadStart = string.Empty;

            FindString = string.Empty;
            FindInDir = string.Empty;
            Filter = string.Empty;
            TopDirectoryOnly = true;
            IsHex = false;
            StrEncoding = "UTF8";
            MaxSearchInstances = 10;
            MaxFileSizeMB = 1;
        }
        [XmlIgnore]
        public bool Error { get; set; }
        [XmlIgnore]
        public string ProgSavePath { get; set; }


        [XmlAttribute]
        public string NodePadPlusPath { get; set; }
        [XmlAttribute]
        public string NodePadPlusStart { get; set; }
        [XmlAttribute]
        public string NodePadPath { get; set; }
        [XmlAttribute]
        public string NodePadStart { get; set; }
        [XmlAttribute]
        public string FindString { get; set; }
        [XmlAttribute]
        public string FindInDir { get; set; }
        [XmlAttribute]
        public string Filter { get; set; }
        [XmlAttribute]
        public bool TopDirectoryOnly { get; set; }
        [XmlAttribute]
        public bool SelectNodePadPlus { get; set; }
        [XmlAttribute]
        public bool IsHex { get; set; }
        [XmlAttribute]
        public int  MaxSearchInstances { get; set; }
        [XmlAttribute]
        public long MaxFileSizeMB { get; set; }
        [XmlAttribute]
        public string StrEncoding { get; set; }
        

        public static Config ConfigLoad(string path, string key)
        {
            return DecryptAndDeserialize<Config>(path, key);
        }
        public static void ConfigSave(string path, Config qobject, string key)
        {
            EncryptAndSerialize(path, qobject, key);
        }







        public static Config LoadConfig()
        {
            Config newConfig = LoadConfig(AppDomain.CurrentDomain.BaseDirectory);
            if (newConfig.Error)
            {
                newConfig = LoadConfig(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
            return newConfig;
        }

        public static Config LoadConfig(string configPath)
        {
            Config newConfig = new Config
            {
                Error = false
            };
            string ProgNameDir = "FindStringInFile";
            if (!string.IsNullOrWhiteSpace(configPath) && Directory.Exists(configPath))
            {
                if (!Directory.Exists(configPath + "\\" + ProgNameDir))
                {
                    try
                    {
                        Directory.CreateDirectory(configPath + "\\" + ProgNameDir);
                    }
                    catch
                    {
                        newConfig.Error = true;
                    }
                }

                string ProgSavePath = configPath + "\\" + ProgNameDir + "\\config.xml";

                //Dir exist, try to load?
                if (!newConfig.Error)
                {
                    if (File.Exists(ProgSavePath))
                    {
                        //try
                        //{
                            newConfig = Config.ConfigLoad(ProgSavePath, string.Empty);
                        //}
                        //catch 
                        //{
                            //File load error
                            if (newConfig == null)
                            {
                            newConfig = new Config
                            {
                                Error = true
                            };
                        }
                            
                        //}
                    }
                    else
                    {
                        newConfig.Error = true;
                    }

                    if (newConfig.Error == true)
                    {
                        try
                        {
                            Config.ConfigSave(ProgSavePath, newConfig, string.Empty);
                            newConfig.Error = false;
                        }
                        catch {}                        
                    }

                    //Save path
                    if (!newConfig.Error)
                    {
                        newConfig.ProgSavePath = ProgSavePath;
                    }
                }
            }

            


            return newConfig;
        }

        public static void EncryptAndSerialize<T>(string filename, T obj, string encryptionKey)
        {
            try
            {

                if (string.IsNullOrEmpty(encryptionKey))
                {
                    using (var fs = File.Open(filename, FileMode.Create))
                    {
                        (new XmlSerializer(typeof(T))).Serialize(fs, obj);
                    }
                }
                else
                {
                    var key = new DESCryptoServiceProvider();
                    var e = key.CreateEncryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(encryptionKey));
                    using (var fs = File.Open(filename, FileMode.Create))
                    using (var cs = new CryptoStream(fs, e, CryptoStreamMode.Write))
                        (new XmlSerializer(typeof(T))).Serialize(cs, obj);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error: Encrypt");
            }
        }

        public static T DecryptAndDeserialize<T>(string filename, string encryptionKey)
        {
            try
            {
                if (!System.IO.File.Exists(filename))
                {
                    MessageBox.Show("File not found!", "Error: file load");
                    return default;
                }


                if (string.IsNullOrEmpty(encryptionKey))
                {
                    using (var fs = File.Open(filename, FileMode.Open))
                    {
                        return (T)(new XmlSerializer(typeof(T))).Deserialize(fs);
                    }
                }
                else
                {
                    var key = new DESCryptoServiceProvider();
                    var d = key.CreateDecryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(encryptionKey));
                    using (var fs = File.Open(filename, FileMode.Open))
                    using (var cs = new CryptoStream(fs, d, CryptoStreamMode.Read))
                        return (T)(new XmlSerializer(typeof(T))).Deserialize(cs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error: Decrypt");
            }
            return default;
        }
    }
}