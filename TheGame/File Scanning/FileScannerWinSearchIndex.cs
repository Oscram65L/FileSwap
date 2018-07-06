using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    public static class FileScannerWinSearchIndex
    {
        //[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        public static List<string> Scan()
        {
            //System.AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            List<string> files = new List<string>();

            using (OleDbConnection connection = new OleDbConnection(@"Provider=Search.CollatorDSO;Extended Properties=""Application=Windows"""))
            {
                // Need to scan for all drives here
                // Should we not do this? As people could just connect an external drive full of useless files to get more points without risk...

                List<char> driveLetters = new List<char>(); // Drive can actually be connected past Z, but won't have a letter assigned. Edgyest of edge cases.

                // Indexing doesn't currently return all files
                connection.Open();

                foreach (var drive in DriveInfo.GetDrives())
                {
                    OleDbCommand command = new OleDbCommand(@"SELECT System.ItemPathDisplay FROM SystemIndex WHERE scope ='file:" + drive.RootDirectory.FullName + "'", connection);

                    using (var r = command.ExecuteReader())
                        while (r.Read())
                            files.Add(r[0] as string);
                }

                connection.Close();
            }

            return files;
        }
    }
}
