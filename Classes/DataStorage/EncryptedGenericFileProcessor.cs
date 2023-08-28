namespace big
{
    public class EncryptedGenericFileProcessor : ITextProcessor
    {
        private static readonly string FilePath = "EncryptedGenericFileProcessor.cs";
        public static ICrypto crypto = new AesCrypto();
        public EncryptedGenericFileProcessor(ICrypto crypto)
        {
            //this.crypto = crypto;
        }

        
        public List<T> LoadFromTextFile<T>(string filePath) where T : Interfaces.ISavable, new()
        {
            StandardLogging.LogInfo(FilePath, "Loading from text file: " + filePath);
            var lines = System.IO.File.ReadAllLines(filePath).ToList();
            List<T> output = new List<T>();
            T entry = new T();


            var cols = entry.GetType().GetProperties();
            StandardLogging.LogInfo(FilePath, "Columns: " + cols.Length);
            

            // Checks to be sure we have at least one header row and one data row
            if (lines.Count < 2)
            {
                StandardLogging.LogError(FilePath, "The file was either empty or missing.");
                throw new IndexOutOfRangeException("The file was either empty or missing.");
            }
            StandardLogging.LogInfo(FilePath, "Lines: " + lines.Count);


            // Splits the header into one column header per entry

            var headers = crypto.Decrypt(lines[0]).Split(',');

            // Removes the header row from the lines so we don't
            // have to worry about skipping over that first row.
            lines.RemoveAt(0);

            foreach (var row in lines)
            {
                entry = new T();

                //Decrypts the row
                string decryptedRow = crypto.Decrypt(row);


                // Splits the row into individual columns. For example now the index
                // of this row matches the index of the header so the
                // UserID column header lines up with the UserID value in
                // value in this row
                var vals = decryptedRow.Split(',');


                // Loops through each header entry so we can compare that
                // against the list of columns from reflection. Once we get
                // the matching column, we can do the "SetValue" method to 
                // set the column value for our entry variable to the vals
                // item at the same index as this particular header.
                for (var i = 0; i < headers.Length; i++)
                {
                    foreach (var col in cols)
                    {
                        if (col.Name == headers[i])
                        {
                            col.SetValue(entry, Convert.ChangeType(vals[i], col.PropertyType));
                        }
                    }
                }

                output.Add(entry);
            }

            StandardLogging.LogInfo(FilePath, "Loaded " + output.Count + " entries from text file: " + filePath);
            return output;
        }



        public void SaveToTextFile<T>(List<T> data, string filePath) where T : Interfaces.ISavable
        {

            StandardLogging.LogInfo(FilePath, "Saving to text file: " + filePath);
            List<string> lines = new List<string>();
            StringBuilder line = new StringBuilder();

            if(!crypto.HasSetIV())
            {
                StandardLogging.LogError(FilePath, "You must set the IV before saving to a file.");
                throw new ArgumentNullException("IV", "You must set the IV before saving to a file.");
            }
            if(!crypto.HasSetKey())
            {
                StandardLogging.LogError(FilePath, "You must set the Key before saving to a file.");
                throw new ArgumentNullException("Key", "You must set the Key before saving to a file.");
            }



            if (data is null || data.Count == 0)
            {
                StandardLogging.LogError(FilePath, "You must populate the data parameter with at least one value.");
                throw new ArgumentNullException("data", "You must populate the data parameter with at least one value.");
            }
            var cols = data[0].GetType().GetProperties();

            StandardLogging.LogInfo(FilePath, "Columns: " + cols.Length);


            // Loops through each column and gets the name so it can comma 
            // separate it into the header row.
            foreach (var col in cols)
            {
                line.Append(col.Name);
                line.Append(",");
            }

            string templine = line.ToString().Substring(0, line.Length - 1);
            line.Clear();
            line.Append(crypto.Encrypt(templine));
            

            StandardLogging.LogInfo(FilePath, "Lines: " + lines.Count);

            // Adds the column header entries to the first line 
            lines.Add(line.ToString());

            foreach (var row in data)
            {
                line = new StringBuilder();

                foreach (var col in cols)
                {
                    line.Append(col.GetValue(row));
                    line.Append(",");
                }

                // Adds the row to the set of lines (removing
                // the last comma from the end first).
                templine = line.ToString().Substring(0, line.Length - 1);
                line.Clear();
                line.Append(crypto.Encrypt(templine));

                lines.Add(line.ToString());
            }

            System.IO.File.WriteAllLines(filePath, lines);
            StandardLogging.LogInfo(FilePath, "Saved " + lines.Count + " entries to text file: " + filePath);
        }

    }
}