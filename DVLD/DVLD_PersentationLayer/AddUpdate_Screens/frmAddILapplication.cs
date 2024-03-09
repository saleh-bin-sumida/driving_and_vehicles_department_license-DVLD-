﻿using DVLD.ShowInfo_Screens;
using DVLD_BusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD.AddUpdate_Screens
{
    public partial class frmAddILapplication : Form
    {
        clsLocalLicense license;
        clsApplication Ilapp;

        public frmAddILapplication()
        {
            InitializeComponent();
        }

 


        private void frmAddILapplication_Load(object sender, EventArgs e)
        {
            btnIssue.Enabled = false;
            linklabel_intLicense.Enabled = false;
            linklabel_licsneHistroy.Enabled = false;

            lblapplicationDate.Text = DateTime.Now.Date.ToString();
            lblIssueDate.Text = lblapplicationDate.Text;
            lblFees.Text = clsApplication.ApplicationTypeFees(6).ToString();
            lblExpirationDate.Text = DateTime.Now.Date.AddYears(1).ToString();
            lblCreatedbyUser.Text = GlobalSettings.CurrentUser.UserID.ToString();
        }


        private void ctrlDriverLicenseCardwithFilter1_OnLicenseSelected(int obj)
        {
            btnIssue.Enabled = true;
            lblLicenseID.Text = ctrlDriverLicenseCardwithFilter1.LicenseID.ToString();
            linklabel_licsneHistroy.Enabled = true;
        }


        private void btnIssue_Click(object sender, EventArgs e)
        {
            license = clsLocalLicense.Find(ctrlDriverLicenseCardwithFilter1.LicenseID);

            if (!license.IsActive)
            {
                MessageBox.Show("this license isnt active, Renew Ur license!");
                return;
            }
            if (license.LicenseClassID != 3)
            {
                MessageBox.Show("cant use license not from the third class, Issue a license from the third class !");
                return;
            }

            if (clsDriver.HasActiveInternationalLicense(license.DriverID))
            {
                MessageBox.Show("this driver alraady has active International License, !");
                return;
            }

            Ilapp = new clsApplication(6);
            clsDriver dr = clsDriver.Find(license.DriverID);

            Ilapp.ApplicantPersonID = dr.Person.PersonID;


            if (Ilapp.Save())
            {
                clsInternationalLicense Intlicense = new clsInternationalLicense(license, Ilapp);
                if (Intlicense.Save())
                {
                    MessageBox.Show("Ur IL app and International License issued succesfully");
                    lblILlicenseID.Text = Intlicense.intLicenseID.ToString();
                    lblILapplicationID.Text = Intlicense.ApplicationID.ToString();
                    btnIssue.Enabled = false;
                    linklabel_intLicense.Enabled = true;
                    return;
                }
            }
            MessageBox.Show("somthing went wrong", "faild to issue IL licnese", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }
            
        private void linklabel_intLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowDriverIntLicense frm = new frmShowDriverIntLicense(int.Parse(lblILlicenseID.Text));
            frm.ShowDialog();
        }

        private void linklabel_licsneHistroy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseHistory frm = new frmShowLicenseHistory(license.DriverID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
