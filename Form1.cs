
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace CodePatternReplacer
{
    public partial class Form1 : Form
    {
        private TextBox folderTextBox = null!;
        private TextBox searchTextBox = null!;
        private TextBox replaceTextBox = null!;
        private TextBox extensionsTextBox = null!;
        private CheckBox recursiveCheckBox = null!;
        private CheckBox backupCheckBox = null!;
        private CheckBox previewCheckBox = null!;
        private RichTextBox logBox = null!;

        public Form1()
        {
            BuildInterface();
        }

        private void BuildInterface()
        {
            Text = "CodePatternReplacer";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(760, 650);
            Size = new Size(900, 760);
            Font = new Font("Segoe UI", 10);
            BackColor = Color.FromArgb(245, 247, 250);

            var main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(22),
                ColumnCount = 1,
                RowCount = 5
            };

            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 235));
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Controls.Add(main);

            var heading = new Panel { Dock = DockStyle.Fill };

            heading.Controls.Add(new Label
            {
                Text = "CodePatternReplacer",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 52, 75),
                AutoSize = true,
                Location = new Point(0, 0)
            });

            heading.Controls.Add(new Label
            {
                Text = "Find and replace text in files safely",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                AutoSize = true,
                Location = new Point(2, 40)
            });

            main.Controls.Add(heading, 0, 0);

            var folderGroup = new GroupBox
            {
                Text = " Target folder ",
                Dock = DockStyle.Fill,
                Padding = new Padding(12)
            };

            var folderLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };

            folderLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));
            folderLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 105));

            folderTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                PlaceholderText = "Select a folder to scan..."
            };

            var browseButton = new Button
            {
                Text = "Browse...",
                Dock = DockStyle.Fill,
                Margin = new Padding(8, 0, 0, 0)
            };

            browseButton.Click += BrowseFolder;

            folderLayout.Controls.Add(folderTextBox, 0, 0);
            folderLayout.Controls.Add(browseButton, 1, 0);
            folderGroup.Controls.Add(folderLayout);
            main.Controls.Add(folderGroup, 0, 1);

            var settingsGroup = new GroupBox
            {
                Text = " Replacement settings ",
                Dock = DockStyle.Fill,
                Padding = new Padding(12)
            };

            var settings = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4
            };

            settings.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 140));
            settings.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 4; i++)
                settings.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 25));

            searchTextBox = new TextBox { Dock = DockStyle.Fill };
            replaceTextBox = new TextBox { Dock = DockStyle.Fill };

            extensionsTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Text = ".cs, .cshtml, .js, .config"
            };

            settings.Controls.Add(
                MakeLabel("Search for:"), 0, 0);
            settings.Controls.Add(searchTextBox, 1, 0);

            settings.Controls.Add(
                MakeLabel("Replace with:"), 0, 1);
            settings.Controls.Add(replaceTextBox, 1, 1);

            settings.Controls.Add(
                MakeLabel("File extensions:"), 0, 2);
            settings.Controls.Add(extensionsTextBox, 1, 2);

            var options = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = true,
                AutoScroll = true
            };

            recursiveCheckBox = new CheckBox
            {
                Text = "Include subfolders",
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 5, 15, 0)
            };

            backupCheckBox = new CheckBox
            {
                Text = "Create backups",
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 5, 15, 0)
            };

            previewCheckBox = new CheckBox
            {
                Text = "Preview before applying",
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 0)
            };

            options.Controls.Add(recursiveCheckBox);
            options.Controls.Add(backupCheckBox);
            options.Controls.Add(previewCheckBox);

            settings.Controls.Add(
                MakeLabel("Options:"), 0, 3);
            settings.Controls.Add(options, 1, 3);

            settingsGroup.Controls.Add(settings);
            main.Controls.Add(settingsGroup, 0, 2);

            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 12, 0, 0)
            };

            var previewButton = new Button
            {
                Text = "Preview Changes",
                Size = new Size(155, 38),
                BackColor = Color.FromArgb(44, 91, 145),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            var applyButton = new Button
            {
                Text = "Apply Replacements",
                Size = new Size(175, 38),
                BackColor = Color.FromArgb(35, 125, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            previewButton.Click += PreviewChanges;

            applyButton.Click += ApplyReplacements;

            actions.Controls.Add(previewButton);
            actions.Controls.Add(applyButton);
            main.Controls.Add(actions, 0, 3);

            var logGroup = new GroupBox
            {
                Text = " Activity log ",
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            logBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "CodePatternReplacer is ready.\n" +
                       "Select a target folder to get started.\n"
            };

            logGroup.Controls.Add(logBox);
            main.Controls.Add(logGroup, 0, 4);
        }

        private Label MakeLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false
            };
        }

        private void BrowseFolder(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Select the folder to scan"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folderTextBox.Text = dialog.SelectedPath;
                logBox.AppendText(
                    $"Selected folder: {dialog.SelectedPath}\n");
            }
        }
                
        private void PreviewChanges(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(folderTextBox.Text) ||
                !Directory.Exists(folderTextBox.Text))
            {
                MessageBox.Show(
                    "Please select a valid target folder.",
                    "Invalid folder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                MessageBox.Show(
                    "Please enter the text you want to find.",
                    "Search text required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var extensions = extensionsTextBox.Text
                .Split(',', StringSplitOptions.RemoveEmptyEntries |
                            StringSplitOptions.TrimEntries)
                .Select(ext => ext.StartsWith(".") ? ext : "." + ext)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (extensions.Count == 0)
            {
                MessageBox.Show(
                    "Please enter at least one file extension.",
                    "Extensions required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string searchText = searchTextBox.Text;

            int filesScanned = 0;
            int filesWithMatches = 0;
            int totalMatches = 0;
            int errors = 0;

            logBox.Clear();
            logBox.AppendText("PREVIEW — No files will be modified.\n");
            logBox.AppendText($"Folder: {folderTextBox.Text}\n");
            logBox.AppendText($"Search: {searchText}\n");
            logBox.AppendText($"Extensions: {string.Join(", ", extensions)}\n");
            logBox.AppendText("----------------------------------------\n");

            try
            {
               
                var searchOption = recursiveCheckBox.Checked
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly;

                foreach (string filePath in Directory.GetFiles(
                            folderTextBox.Text, "*", searchOption))
                {
                    string extension = Path.GetExtension(filePath);

                    logBox.AppendText(
                        $"[FOUND FILE] {Path.GetFileName(filePath)} | Extension: '{extension}'\n");

                    if (!extensions.Contains(extension))
                    {
                        logBox.AppendText(
                            $"[SKIPPED] Extension not selected: {filePath}\n");
                        continue;
                    }


                    filesScanned++;

                    try
                    {
                        string content = File.ReadAllText(filePath);
                        int matches = CountOccurrences(content, searchText);

                        if (matches > 0)
                        {
                            filesWithMatches++;
                            totalMatches += matches;

                            logBox.AppendText(
                                $"[MATCH: {matches}] {filePath}\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        errors++;
                        logBox.AppendText(
                            $"[ERROR] {filePath}: {ex.Message}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                errors++;
                logBox.AppendText(
                    $"\n[SCAN ERROR] {ex.Message}\n");
            }

            logBox.AppendText("\n========== SUMMARY ==========\n");
            logBox.AppendText($"Files scanned: {filesScanned}\n");
            logBox.AppendText($"Files with matches: {filesWithMatches}\n");
            logBox.AppendText($"Total occurrences: {totalMatches}\n");
            logBox.AppendText($"Errors: {errors}\n");
            logBox.AppendText(
                "\nPreview complete. No files were modified.\n");
        }

        private int CountOccurrences(string content, string searchText)
        {
            int count = 0;
            int position = 0;

            while ((position = content.IndexOf(
                        searchText, position,
                        StringComparison.Ordinal)) >= 0)
            {
                count++;
                position += searchText.Length;
            }

            return count;
        }

        private void ApplyReplacements(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(folderTextBox.Text) ||
                !Directory.Exists(folderTextBox.Text))
            {
                MessageBox.Show("Please select a valid target folder.");
                return;
            }

            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                MessageBox.Show("Please enter the text to find.");
                return;
            }

            if (previewCheckBox.Checked)
            {
                var answer = MessageBox.Show(
                    "Have you reviewed the preview and confirmed the changes?",
                    "Confirm replacements",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (answer != DialogResult.Yes)
                    return;
            }

            var extensions = extensionsTextBox.Text
                .Split(',', StringSplitOptions.RemoveEmptyEntries |
                            StringSplitOptions.TrimEntries)
                .Select(ext => ext.StartsWith(".") ? ext : "." + ext)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var searchOption = recursiveCheckBox.Checked
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            int modifiedFiles = 0;
            int totalReplacements = 0;
            int errors = 0;

            logBox.AppendText("\n=== APPLY REPLACEMENTS ===\n");

            try
            {
                foreach (string filePath in Directory.GetFiles(
                            folderTextBox.Text, "*", searchOption))
                {
                    if (!extensions.Contains(Path.GetExtension(filePath)))
                        continue;

                    try
                    {
                        string originalContent = File.ReadAllText(filePath);
                        string searchText = searchTextBox.Text;

                        int matches = CountOccurrences(
                            originalContent, searchText);

                        if (matches == 0)
                            continue;

                        string replacementContent = originalContent.Replace(
                            searchText,
                            replaceTextBox.Text,
                            StringComparison.Ordinal);

                        if (originalContent == replacementContent)
                            continue;

                        if (backupCheckBox.Checked)
                        {
                            string backupPath = filePath + ".bak";

                            if (File.Exists(backupPath))
                            {
                                logBox.AppendText(
                                    $"[SKIPPED] Backup already exists: {backupPath}\n");
                                errors++;
                                continue;
                            }

                            File.Copy(filePath, backupPath);
                            logBox.AppendText($"[BACKUP] {backupPath}\n");
                        }

                        File.WriteAllText(filePath, replacementContent);

                        modifiedFiles++;
                        totalReplacements += matches;

                        logBox.AppendText(
                            $"[MODIFIED: {matches}] {filePath}\n");
                    }
                    catch (Exception ex)
                    {
                        errors++;
                        logBox.AppendText(
                            $"[ERROR] {filePath}: {ex.Message}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                errors++;
                logBox.AppendText($"[SCAN ERROR] {ex.Message}\n");
            }

            logBox.AppendText("\n========== APPLY SUMMARY ==========\n");
            logBox.AppendText($"Files modified: {modifiedFiles}\n");
            logBox.AppendText($"Total replacements: {totalReplacements}\n");
            logBox.AppendText($"Errors: {errors}\n");
        }

    }
}
