using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace crudform
{
    public partial class Form1 : Form
    {
        private List<string> allItems = new List<string>();
        private string filePath = "C:\\Users\\Lenovo\\source\\repos\\crudform\\data.txt"; // Specify the file path

        public Form1()
        {
            InitializeComponent();
            LoadDataFromFile(); // Load data from the file when the form loads
        }

        private void SaveDataToFile()
        {
            File.WriteAllLines(filePath, allItems);
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(filePath))
            {
                allItems = File.ReadAllLines(filePath).ToList();
                RefreshCheckedListBox();
            }
        }

        private void RefreshCheckedListBox()
        {
            checkedListBox1.Items.Clear();
            foreach (string item in allItems)
            {
                checkedListBox1.Items.Add(item, true);
            }
        }

        private void AddItemToMemory(string item)
        {
            allItems.Add(item);
        }

        private void AddItemToCheckedListBox(string item)
        {
            checkedListBox1.Items.Add(item, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get input values from the TextBox and ComboBox controls
            string personnelNumber = textBox1.Text.Trim();
            string name = textBox2.Text.Trim();
            string gender = textBox3.Text;

            // Get selected familiar languages from the CheckedListBox
            string language = textBox4.Text;

            // If language is not "Yes," set it to "No"
            if (language != "Yes")
            {
                language = "No";
            }

            string hobbiesInput = textBox5.Text;
            if (hobbiesInput.Length > 1000)
            {
                MessageBox.Show("Hobbies input cannot exceed 1000 characters.", "Input Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method
            }

            // Get hobbies from the TextBox and split them into a list
            List<string> hobbies = textBox5.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(hobby => hobby.Trim()).ToList();

            // Combine the input data into a single string
            string inputData = $"Personnel Number: {personnelNumber}, Name: {name}, Gender: {gender}, Languages: {language}, Hobbies: {string.Join(", ", hobbies)}";

            // Add the input data to the ListBox and memory
            AddItemToCheckedListBox(inputData);
            AddItemToMemory(inputData);

            // Clear the input fields
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int selectedIndex = checkedListBox1.SelectedIndex;

            if (selectedIndex >= 0)
            {
                checkedListBox1.Items.RemoveAt(selectedIndex);
                allItems.RemoveAt(selectedIndex);
            }
            else
            {
                MessageBox.Show("Please select an item to delete.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
            
        //search button
        private void button4_Click(object sender, EventArgs e)
        {
            string searchQuery = searchtextBox.Text.Trim().ToLower(); // Convert to lowercase for case-insensitive search

            if (string.IsNullOrEmpty(searchQuery))
            {
                // If the search query is empty, reload all items from memory
                RefreshCheckedListBox();
                return;
            }

            // Clear the CheckedListBox
            checkedListBox1.Items.Clear();

            // Filter and display items that match the search query
            foreach (string item in allItems)
            {
                string[] details = item.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string personnelNumber = details[0].ToLower(); // Extract and convert personnel number to lowercase
                string name = details[1].ToLower(); // Extract and convert name to lowercase

                if (personnelNumber.Contains(searchQuery) || name.Contains(searchQuery))
                {
                    AddItemToCheckedListBox(item); // Add the item and check it
                }
            }

            if (checkedListBox1.Items.Count == 0)
            {
                MessageBox.Show("No matching items found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // edit function
        private void button2_Click(object sender, EventArgs e)
        {
            int selectedIndex = checkedListBox1.SelectedIndex;

            if (selectedIndex >= 0)
            {
                // Get the selected item
                string selectedItem = checkedListBox1.SelectedItem.ToString();

                // Split the item details and populate the editTextBox
                string[] details = selectedItem.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string personnelNumber = details[0].Trim().Substring("Personnel Number:".Length);
                string name = details[1].Trim().Substring("Name:".Length);
                string gender = details[2].Trim().Substring("Gender:".Length);
                string language = details[3].Trim().Substring("Languages:".Length);
                string hobbies = details[4].Trim().Substring("Hobbies:".Length);

                textBox1.Text = personnelNumber;
                textBox2.Text = name;
                textBox3.Text = gender;
                textBox4.Text = language;
                textBox5.Text = hobbies;
            }
            else
            {
                MessageBox.Show("Please select an item to edit.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Save button click event to apply edits
        private void saveButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = checkedListBox1.SelectedIndex;

            if (selectedIndex >= 0)
            {
                // Create a new item with the edited details
                string personnelNumber = textBox1.Text.Trim();
                string name = textBox2.Text.Trim();
                string gender = textBox3.Text.Trim();
                string language = textBox4.Text.Trim();
                string hobbies = textBox5.Text.Trim();

                string editedItem = $"Personnel Number: {personnelNumber}, Name: {name}, Gender: {gender}, Languages: {language}, Hobbies: {hobbies}";

                // Update the item in the CheckedListBox and memory
                checkedListBox1.Items[selectedIndex] = editedItem;
                allItems[selectedIndex] = editedItem;

                // Clear the editTextBox
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();

                // Show a confirmation message
                MessageBox.Show("Editing is complete.", "Edit Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select an item to edit.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //edit save button 
        private void button5_Click(object sender, EventArgs e)
        {
            int selectedIndex = checkedListBox1.SelectedIndex;

            if (selectedIndex >= 0)
            {
                // Create a new item with the edited details
                string personnelNumber = textBox1.Text.Trim();
                string name = textBox2.Text.Trim();
                string gender = textBox3.Text.Trim();
                string language = textBox4.Text.Trim();
                string hobbies = textBox5.Text.Trim();

                string editedItem = $"Personnel Number: {personnelNumber}, Name: {name}, Gender: {gender}, Languages: {language}, Hobbies: {hobbies}";

                // Update the item in the CheckedListBox and memory
                checkedListBox1.Items[selectedIndex] = editedItem;
                allItems[selectedIndex] = editedItem;

                // Clear the editTextBox
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();

                // Show a confirmation message
                MessageBox.Show("Editing is complete.", "Edit Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select an item to edit.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
                SaveDataToFile();
                MessageBox.Show("All data saved to the file.", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}




