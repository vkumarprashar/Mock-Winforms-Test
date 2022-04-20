using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mock_Test
{
    public partial class Form1 : Form
    {
        //DAO Model Intialization
        MockTestEntities mockTestEntities;
        public Form1()
        {
            InitializeComponent();
            mockTestEntities = new MockTestEntities();
            dataGridView1.DataSource = mockTestEntities.Registrations.Select(reg => new
            {
                Runner_Email = reg.Runner.Email,
                Registration_Date_Time = reg.RegistrationDateTime,
                Charity_Name = reg.Charity.CharityName,
                Sponsorship_Target = reg.SponsorshipTarget
            }).ToList();

            
            int total = dataGridView1.Rows.Cast<DataGridViewRow>()
                .Sum(t => Convert.ToInt32(t.Cells[3].Value));
            spTargetTextBox.Text = total.ToString();

            charityComboBox.DataSource = mockTestEntities.Charities.Select(reg => reg.CharityName).ToList();
            charityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void spTargetTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string _charityName = charityComboBox.SelectedItem.ToString();
            string _runnerEmail = runnerEmailTextBox.Text.ToLower().Trim().ToString();
            string _regMinDateString = minDate.Text.Trim().ToString();
            string _regMaxDateString = maxDate.Text.Trim().ToString();

            mockTestEntities = new MockTestEntities();
            IEnumerable<Registration> list = mockTestEntities.Registrations;
            if(!string.IsNullOrWhiteSpace(_charityName))
            {
                list = list.Where(reg => reg.Charity.CharityName == _charityName).ToList();
            }

            if (!string.IsNullOrWhiteSpace(_runnerEmail))
            {
                list = list.Where(reg => reg.Runner.Email.ToLower().Contains(_runnerEmail)).ToList();
            }

            if (minDate.Checked == true && !string.IsNullOrWhiteSpace(_regMinDateString))
            {
                DateTime _regMinDate = DateTime.Parse(_regMinDateString);
                list = list.Where(reg => reg.RegistrationDateTime >= _regMinDate).ToList();
            }

            if (maxDate.Checked == true && !string.IsNullOrWhiteSpace(_regMaxDateString))
            {
                DateTime _regMaxDate = DateTime.Parse(_regMaxDateString);
                list = list.Where(reg => reg.RegistrationDateTime <= _regMaxDate).ToList();
            }

            
            dataGridView1.DataSource = list.Select(reg => new
            {
                Runner_Email = reg.Runner.Email,
                Registration_Date_Time = reg.RegistrationDateTime,
                Charity_Name = reg.Charity.CharityName,
                Sponsorship_Target = reg.SponsorshipTarget
            }).ToList();

            int total = dataGridView1.Rows.Cast<DataGridViewRow>()
                .Sum(t => Convert.ToInt32(t.Cells[3].Value));
            spTargetTextBox.Text = total.ToString();
        }

    }
}
