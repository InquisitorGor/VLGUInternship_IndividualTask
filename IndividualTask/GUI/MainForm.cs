using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IndividualTask
{

    public partial class MainForm : Form
    {
        private const String PATH = "..\\..\\resources\\configuration.txt";
        private const char DELIMETR = ';';
        private Dictionary<String, double> dictionary = new Dictionary<String, double>();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            loadConfiguration();
            fillCollectionFromFile();
        }

        
        private void loadConfiguration() 
        {
            configurationTextBox.Clear();
            String [] str = File.ReadAllLines(PATH);
            for (int i = 0; i < str.Length; i++)
            {
                configurationTextBox.AppendText(str[i] + Environment.NewLine);
            }
        }

        private void countButtonListener(object sender, EventArgs e)
        {
            label8.Visible = true;
            resultLable.Visible = true;
            resetButton.Visible = true;
            resultLable.Text = countInsurance();
        }
        private void fillCollectionFromFile()
        {
            FileStream fileStream = new FileStream(PATH, FileMode.OpenOrCreate);
            StreamReader sR = new StreamReader(fileStream);
            String str1 = String.Empty;
            while ((str1 = sR.ReadLine()) != null && str1 != String.Empty)
            {
                String[] str = str1.Split(DELIMETR);
                dictionary.Add(str[0], Convert.ToDouble(str[1]));
            }
            sR.Close();
            fileStream.Close();
        }
        private String countInsurance()
        {
            double owner, propertyType, enginePower, periodOfUse, ageAndExp, troubleFreeExperience, max, min;

            if (checkValues())
            {

                dictionary.TryGetValue(comboBox1.Text, out owner);
                dictionary.TryGetValue(comboBox2.Text, out propertyType);
                dictionary.TryGetValue(comboBox3.Text, out enginePower);
                dictionary.TryGetValue(comboBox4.Text, out periodOfUse);
                dictionary.TryGetValue(comboBox5.Text, out ageAndExp);
                dictionary.TryGetValue(comboBox6.Text, out troubleFreeExperience);
                dictionary.TryGetValue("Минимальное", out min);
                dictionary.TryGetValue("Максимальное", out max);

                double minResult = min * owner * propertyType * enginePower * periodOfUse * ageAndExp *
                    troubleFreeExperience;
                double maxResult = max * owner * propertyType * enginePower * periodOfUse * ageAndExp *
                    troubleFreeExperience;

                return "от " + minResult + " до " + maxResult;
            }
            return "Неверные данные";

        }

        private bool checkValues()
        {
            ComboBox[] comboBoxes = {comboBox1,comboBox2,comboBox3,comboBox4,comboBox5,comboBox6};
            foreach (var item in comboBoxes)
            {
                if(!dictionary.ContainsKey(item.Text))
                {
                    MessageBox.Show(
                    "Неверно введены поля",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                    return false;
                }
            }
            return true;

        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            label8.Visible = false;
            resultLable.Visible = false;
            resetButton.Visible = false;
            setDefaultTextBoxValues();
        }

        private void setDefaultTextBoxValues()
        {
            comboBox1.Text = "Физическое лицо";
            comboBox2.Text = "Легковые автомобили";
            comboBox3.Text = "От 121 до 150 л.с.";
            comboBox4.Text = "5 месяцев";
            comboBox5.Text = "До 22 лет, стаж более 3 лет";
            comboBox6.Text = "2 года";
        }

        private void applyButtonListener(object sender, EventArgs e)
        {
            if (passwordTextBox.Text == "1234")
            {
                configurationTextBox.ReadOnly = false;
                saveConfig();
            }
            
        }

        private void saveConfig() 
        {
            String[] str = configurationTextBox.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            dictionary.Clear();
            foreach (var item in str)
            {
                String[] temp = item.Split(DELIMETR);
                dictionary.Add(temp[0], Convert.ToDouble(temp[1]));
            }

            File.Delete(PATH);
            
            double [] values = (dictionary.Values).ToArray();
            string [] keys = (dictionary.Keys).ToArray();
            for (int i = 0; i < values.Length; i++)
            {
                File.AppendAllText(PATH, keys[i] + ";" + values[i] + "\n"); 
            }
            loadConfiguration();
        }
    }

}
