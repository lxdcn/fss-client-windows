﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Security.Cryptography;

namespace fss_client
{
    class Sha1
    {
        private SHA1CryptoServiceProvider provider;
        public Sha1()
        {
            provider = new SHA1CryptoServiceProvider();
        }

        // unique algorithm sha1 digest for fss
        public string sha1_file_via_fname_fss(string rootpath, string fullname)
        {
            byte[] digest;
            string digest_str0, digest_str1;

            // if fullname is a dir, still calculate sha1 digest
            if (Directory.Exists(fullname))
            {
                digest = provider.ComputeHash(
                    System.Text.Encoding.Default.GetBytes("IAMDIR"));
            }
            else
            {

                FileStream fs = new FileStream(fullname, FileMode.Open, FileAccess.Read, FileShare.None);
                digest = provider.ComputeHash(fs);
                fs.Close();
            }

            digest_str0 = BitConverter.ToString(digest).Replace("-", "");

            string relaname = fullname.Substring(rootpath.Length);

            string[] words = relaname.Split('\\');


            foreach (string s in words)
            {
                if (s == "")
                    continue;
                digest = provider.ComputeHash(
                    System.Text.Encoding.Default.GetBytes(s));

                digest_str1 = BitConverter.ToString(digest).Replace("-", "");

                digest = provider.ComputeHash(
                    System.Text.Encoding.Default.GetBytes(digest_str0 + digest_str1));

                digest_str0 = BitConverter.ToString(digest).Replace("-", "");


            }

            //return BitConverter.ToString(digest);
            return digest_str0;


        }

        // calculate normal sha1 digest
        public string sha1_file_via_fname(string fullname)
        {
            byte[] digest;
            FileStream fs = new FileStream(fullname, FileMode.Open, FileAccess.Read, FileShare.None);
            digest = provider.ComputeHash(fs);
            fs.Close();

            return BitConverter.ToString(digest).Replace("-", "");

        }
    }
}
