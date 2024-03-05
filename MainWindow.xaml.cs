using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;

namespace PublicKeyEncryption
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateNAndZ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int p = int.Parse(txtFirstPrime.Text);
                int q = int.Parse(txtSecondPrime.Text);

                if (!IsPrime(p) || !IsPrime(q))
                {
                    MessageBox.Show("Both numbers must be prime.");
                    return;
                }

                int n = p * q;
                int z = (p - 1) * (q - 1);

                txtNandZ.Text = $"n = {n}, z = {z}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CalculateKeys_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int eValue = int.Parse(txtE.Text);
                int n = int.Parse(txtNandZ.Text.Split(',')[0].Split('=')[1].Trim());
                int z = int.Parse(txtNandZ.Text.Split(',')[1].Split('=')[1].Trim());

                if (!IsCoprime(eValue, z))
                {
                    MessageBox.Show("e must be relatively prime to z.");
                    return;
                }

                int d = CalculateD(eValue, z);

                txtPublicKey.Text = $"{{e = {eValue}, n = {n}}}";
                txtPrivateKey.Text = $"{{d = {d}, n = {n}}}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int Encrypt(int m, int e, int n)
        {
            int c = 1;
            for (int i = 0; i < e; i++)
            {
                c = (c * m) % n;
            }
            return c;
        }

        private int Decrypt(int c, int d, int n)
        {
            int m = 1;
            for (int i = 0; i < d; i++)
            {
                m = (m * c) % n;
            }
            return m;
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int eValue = int.Parse(txtEncryptionE.Text);
                int n = int.Parse(txtEncryptionN.Text);
                string plaintext = txtPlainText.Text;

                string ciphertextInt = "";
                string ciphertextHex = "";

                foreach (char ch in plaintext)
                {
                    int m = ch;
                    int encrypted = Encrypt(m, eValue, n);
                    ciphertextInt += encrypted + " ";
                    ciphertextHex += encrypted.ToString("X4");
                }

                txtCiphertextInt.Text = ciphertextInt;
                txtCiphertextHex.Text = ciphertextHex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int d = int.Parse(txtDecryptionD.Text);
                int n = int.Parse(txtDecryptionN.Text);
                string ciphertextHex = txtCipherTextHex.Text.Trim();

                string plaintextInt = "";
                string plaintext = "";

                for (int i = 0; i < ciphertextHex.Length; i += 4)
                {
                    int encrypted = int.Parse(ciphertextHex.Substring(i, 4), System.Globalization.NumberStyles.HexNumber);
                    int decrypted = Decrypt(encrypted, d, n);
                    plaintextInt += decrypted + " ";
                    plaintext += (char)decrypted;
                }

                txtPlaintextInt.Text = plaintextInt;
                txtPlaintext.Text = plaintext;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int CalculateD(int e, int z)
        {
            int d = 2;

            while (true)
            {
                if ((d * e) % z == 1)
                {
                    break;
                }
                d++;
            }

            return d;
        }

        private bool IsPrime(int num)
        {
            if (num <= 1)
                return false;
            if (num <= 3)
                return true;

            if (num % 2 == 0 || num % 3 == 0)
                return false;

            for (int i = 5; i * i <= num; i += 6)
            {
                if (num % i == 0 || num % (i + 2) == 0)
                    return false;
            }

            return true;
        }

        private bool IsCoprime(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a == 1;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtFirstPrime.Clear();
            txtSecondPrime.Clear();
            txtNandZ.Clear();
            txtE.Clear();
            txtPublicKey.Clear();
            txtPrivateKey.Clear();
            txtEncryptionE.Clear();
            txtEncryptionN.Clear();
            txtPlainText.Clear();
            txtCiphertextInt.Clear();
            txtCiphertextHex.Clear();
            txtDecryptionD.Clear();
            txtDecryptionN.Clear();
            txtCipherTextHex.Clear();
            txtPlaintextInt.Clear();
            txtPlaintext.Clear();
        }
    }
}

