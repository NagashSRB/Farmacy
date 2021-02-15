﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;

namespace Farmacy
{
    public partial class FormMain : Form
    {
        private List<DrugModel> listOfDrugs;
        public FormMain()
        {
            InitializeComponent();
        }

        private void fillList()
        {
            dgv.Rows.Clear();
            foreach(var v in listOfDrugs)
            {
                int index = dgv.Rows.Add(new DataGridViewRow());
                dgv.Rows[index].Cells[0].Value = v.ProductCode;
                dgv.Rows[index].Cells[1].Value = v.Name;
                dgv.Rows[index].Cells[2].Value = v.Manufacturer;
                dgv.Rows[index].Cells[3].Value = v.Type;
                dgv.Rows[index].Cells[4].Value = v.Quantity;
                dgv.Rows[index].Cells[5].Value = v.Price;
                dgv.ClearSelection();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var allTexboxes = this.Controls.OfType<TextBox>();
            var filter = Builders<DrugModel>.Filter.Empty;
            var filter2 = Builders<DrugModel>.Filter.Empty;
            var arrayTextBoxes = allTexboxes
                                 .Where(i => String.IsNullOrEmpty(i.Text))
                                 .ToArray();

            for (int i = 0; i < arrayTextBoxes.Length; i++)
            {
                filter2 = null;
                if (arrayTextBoxes[i].Tag.ToString() == "Symptoms")
                {
                    string[] parameters = arrayTextBoxes[i].Text.Split(',');
                    foreach (string parameter in parameters)
                    {
                        filter2 |= Builders<DrugModel>.Filter.AnyEq("Instruction.Symptoms", parameter);
                    }
                }
                else
                {
                    filter2 |= Builders<DrugModel>.Filter.Eq(arrayTextBoxes[i].Tag.ToString(), arrayTextBoxes[i].Text);
                }
                filter &= filter2;
            }
            listOfDrugs = FarmacyManager.Instance.searchDrugs(filter).Distinct().ToList();
            fillList(); //obrada podataka

        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var formInformation = new FormInformation(listOfDrugs);
            formInformation.ShowDialog();
            //focus
        }
    }
}
