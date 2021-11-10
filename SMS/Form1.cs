using SMS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS
{
    public partial class FormStudentDetails : Form
    {
        tblStudent st = new tblStudent();
        public FormStudentDetails()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentDetails();
        }

        public void GetStudentDetails()
        {
            SMSEntities sMSEntities = new SMSEntities();
            List<StudentDetails> StudentList = new List<StudentDetails>();
            using (sMSEntities)
            {
                StudentList = sMSEntities.tblStudents.Select(x => new StudentDetails
                {
                    IdStudent = x.IdStudent,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Dob = (DateTime)x.Dob,
                    Address = x.Address,
                    Phone = (long)x.Phone
                }).ToList();
                dgvStudentList.DataSource = StudentList;
            }
            buttonSave.Visible = true;
            buttonSave.Enabled = true;
            buttonClear.Enabled = false;
            buttonClear.Visible = false;
            buttonUpdate.Enabled = false;
            buttonDelete.Enabled = false;
            tbFirstName.Text = tbLastName.Text = tbAddress.Text = tbPhoneNumber.Text = "";
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            bool result = false;
            st.FirstName = tbFirstName.Text.Trim();
            st.LastName = tbLastName.Text.Trim();
            st.Dob = dtpDob.Value;
            st.Address = tbAddress.Text;
            if (tbPhoneNumber.Text != "")
            {
                st.Phone = Convert.ToInt64(tbPhoneNumber.Text);
            }
            if (st.FirstName.Length < 1 || st.LastName.Length < 1 || st.Address.Length < 1 || st.Phone == null)
            {
                MessageBox.Show("All Fields are required");
                return;
            }
            else
            {
                SMSEntities sMSEntities = new SMSEntities();
                using (sMSEntities)
                {
                    sMSEntities.tblStudents.Add(st);
                    sMSEntities.SaveChanges();
                    result = true;
                }
                tbFirstName.Text = tbLastName.Text = tbAddress.Text = tbPhoneNumber.Text = "";
                DisplayResult(result, "AddStudent");
            }
        }

        public void DisplayResult(bool result, string action)
        {
            if (result == true && action == "AddStudent")
            {
                MessageBox.Show("Student Details Saved Successfully");
                GetStudentDetails();
            }
            else if (result == true && action == "UpdateStudent")
            {
                MessageBox.Show("Student Details Updated Successfully");
                GetStudentDetails();
            }
        }

        private void tbFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvStudentList_DoubleClick(object sender, EventArgs e)
        {
            if(dgvStudentList.CurrentRow.Index != -1)
            {
                labelIdStudent.Text = Convert.ToString(dgvStudentList.CurrentRow.Cells["IdStudent"].Value);
                tbFirstName.Text = Convert.ToString(dgvStudentList.CurrentRow.Cells["FirstName"].Value);
                tbLastName.Text = Convert.ToString(dgvStudentList.CurrentRow.Cells["LastName"].Value);
                tbAddress.Text = Convert.ToString(dgvStudentList.CurrentRow.Cells["Address"].Value);
                tbPhoneNumber.Text = Convert.ToString(dgvStudentList.CurrentRow.Cells["Phone"].Value);

                buttonSave.Visible = false;
                buttonSave.Enabled = false;
                buttonClear.Enabled = true;
                buttonClear.Visible = true;
                buttonUpdate.Enabled = true;
                buttonDelete.Enabled = true;
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            tbFirstName.Text = tbLastName.Text = tbAddress.Text = tbPhoneNumber.Text = "";
            buttonSave.Visible = true;
            buttonSave.Enabled = true;
            buttonClear.Enabled = false;
            buttonClear.Visible = false;
            buttonUpdate.Enabled = false;
            buttonDelete.Enabled = false;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            bool result = false;
            st.IdStudent = Convert.ToInt32(labelIdStudent.Text);
            st.FirstName = tbFirstName.Text.Trim();
            st.LastName = tbLastName.Text.Trim();
            st.Dob = dtpDob.Value;
            st.Address = tbAddress.Text;

            if (tbPhoneNumber.Text != "")
            {
                st.Phone = Convert.ToInt64(tbPhoneNumber.Text);
            }
            
            if (st.FirstName.Length < 1 || st.LastName.Length < 1 || st.Address.Length < 1 || st.Phone == null)
            {
                MessageBox.Show("All Fields are required");
                return;
            }
            else
            {
                SMSEntities sMSEntities = new SMSEntities();
                using (sMSEntities)
                {
                    sMSEntities.Entry(st).State = EntityState.Modified;
                    sMSEntities.SaveChanges();
                    result = true;
                }
                tbFirstName.Text = tbLastName.Text = tbAddress.Text = tbPhoneNumber.Text = "";
                DisplayResult(result, "UpdateStudent");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to delete this record!","SMS",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                SMSEntities sMSEntities = new SMSEntities();
                using (sMSEntities)
                {
                    st.IdStudent = Convert.ToInt32(labelIdStudent.Text);
                    var data = sMSEntities.Entry(st);
                    
                    if(data.State == EntityState.Detached)
                    {
                        sMSEntities.tblStudents.Attach(st);
                    }
                    sMSEntities.tblStudents.Remove(st);
                    sMSEntities.SaveChanges();
                    GetStudentDetails();
                    MessageBox.Show("Data deleted successfully");
                }
            }
            st.IdStudent = Convert.ToInt32(labelIdStudent.Text);
        }

        private void tbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }
    }
}
