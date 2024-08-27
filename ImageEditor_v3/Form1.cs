// Decompiled with JetBrains decompiler
// Type: ImageEditor.Form1
// Assembly: ImageEditor, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC5FD-5783-4571-A04C-A506D4C08D16
// Assembly location: Z:\User\Desktop\repository\ImageEditor_v3.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace ImageEditor
{
  public class Form1 : Form
  {
    public int Img1_w;
    public int Img1_h;
    public int Img2_w;
    public int Img2_h;
    public Decimal maxX;
    public Decimal maxY;
    public string fileName;
    public string selectedPath;
    private IContainer components;
    private Button button1;
    private TextBox textBox1;
    private TextBox textBox2;
    private Label label1;
    private Label label2;
    private TextBox textBox3;
    private Label label3;
    private OpenFileDialog openFileDialog1;
    private Button button2;
    private Button button3;
    private Button button4;
    private FolderBrowserDialog folderBrowserDialog1;
    private Label label4;
    private Label label5;
    private NumericUpDown numericUpDown1;
    private NumericUpDown numericUpDown2;
    private Label label6;
    private PictureBox pictureBox1;
    private CheckBox checkBox2;
    private Label label7;
    private CheckBox rotateImg;

    public Form1()
    {
      this.InitializeComponent();
      this.reset();
      this.textBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\logo.png";
      this.selectedPath = Path.GetDirectoryName(this.textBox1.Text);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.textBox3.Text))
      {
        Directory.CreateDirectory(this.selectedPath + "\\MRK");
        this.Backbone(true);
        if (!this.checkBox2.Checked)
          return;
        this.openFile();
      }
      else
      {
        if (!string.IsNullOrEmpty(this.textBox3.Text))
          return;
        int num = (int) MessageBox.Show("Tentativa de salvar sem escolher o local!", "ERROR-01");
      }
    }

    private void callfromselec()
    {
      if (string.IsNullOrEmpty(this.textBox2.Text) || !File.Exists(this.textBox2.Text))
        return;
      this.Backbone(false);
    }

    public void Backbone(bool canSave)
    {
      int int32_1 = Convert.ToInt32(this.numericUpDown1.Value / (100M / this.maxX));
      int int32_2 = Convert.ToInt32(this.numericUpDown2.Value / (100M / this.maxY));
      string text = this.textBox3.Text;
      string[] array = new List<string>()
      {
        this.textBox1.Text,
        this.textBox2.Text
      }.ToArray();
      if (!string.IsNullOrEmpty(this.textBox1.Text) && !string.IsNullOrEmpty(this.textBox2.Text))
      {
        if (File.Exists(this.textBox1.Text) && File.Exists(this.textBox2.Text))
        {
          this.CombineBitmap(array, text, int32_1, int32_2, canSave);
          this.limiter();
        }
        else
        {
          int num1 = (int) MessageBox.Show("Path 1 or 2 do not exist. Check if both are valid paths!");
        }
      }
      else
      {
        int num2 = (int) MessageBox.Show("Imagem 1 ou 2 nao foram escolhidas!");
      }
    }

    public Bitmap CombineBitmap(string[] files, string loc, int offsetX, int offsetY, bool save)
    {
      List<Bitmap> bitmapList = new List<Bitmap>();
      Bitmap bitmap1 = (Bitmap) null;
      try
      {
        Bitmap bitmap2 = new Bitmap(files[0]);
        if (this.rotateImg.Checked)
        {
          bitmap2.RotateFlip(RotateFlipType.Rotate90FlipNone);
          this.resizeWindow();
        }
        int width = bitmap2.Width;
        int height = bitmap2.Height;
        Bitmap bitmap3 = new Bitmap(files[1]);
        bitmapList.Add(bitmap2);
        bitmapList.Add(bitmap3);
        bitmap1 = new Bitmap(width, height);
        using (Graphics graphics = Graphics.FromImage((Image) bitmap1))
        {
          graphics.Clear(Color.Black);
          graphics.DrawImage((Image) bitmapList[0], new Rectangle(0, 0, bitmapList[0].Width, bitmapList[0].Height));
          graphics.DrawImage((Image) bitmapList[1], new Rectangle(offsetX, offsetY, bitmapList[1].Width, bitmapList[1].Height));
        }
        if (this.pictureBox1.Image != null)
          this.pictureBox1.Image.Dispose();
        this.pictureBox1.Image = (Image) bitmap1;
        if (save)
        {
          if (!string.IsNullOrEmpty(this.textBox3.Text))
          {
            bitmap1.Save(loc, ImageFormat.Jpeg);
            if (!this.checkBox2.Checked)
            {
              int num = (int) MessageBox.Show("Arquivo salvo com sucesso!");
              bitmap1.Dispose();
            }
          }
          else
          {
            int num1 = (int) MessageBox.Show("Save Path not chosen. Please choose a valid save location!");
          }
        }
        return bitmap1;
      }
      catch (Exception ex)
      {
        bitmap1?.Dispose();
        throw;
      }
      finally
      {
        foreach (Image image in bitmapList)
          image.Dispose();
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      string str = (string) null;
      if (string.IsNullOrEmpty(this.textBox1.Text))
        str = "C:\\";
      else if (!string.IsNullOrEmpty(this.textBox1.Text))
        str = Path.GetDirectoryName(this.textBox1.Text);
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = str;
      openFileDialog.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg|JPEG (*.jpeg)|*.jpeg|All files (*.*)|*.*";
      openFileDialog.FilterIndex = 2;
      openFileDialog.RestoreDirectory = true;
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        Stream stream;
        if ((stream = openFileDialog.OpenFile()) == null)
          return;
        using (stream)
        {
          this.textBox1.Text = openFileDialog.FileName;
          this.fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
          this.callfromselec();
          this.CleanPath();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error 01: Could not read file from disk. Original error: " + ex.Message);
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      string str = (string) null;
      if (string.IsNullOrEmpty(this.textBox2.Text))
        str = "C:\\";
      else if (!string.IsNullOrEmpty(this.textBox2.Text))
        str = Path.GetDirectoryName(this.textBox2.Text);
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = str;
      openFileDialog.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|JPEG (*.jpeg)|*.jpeg|All files (*.*)|*.*";
      openFileDialog.FilterIndex = 2;
      openFileDialog.RestoreDirectory = true;
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        Stream stream;
        if ((stream = openFileDialog.OpenFile()) == null)
          return;
        using (stream)
        {
          this.textBox2.Text = openFileDialog.FileName;
          this.CleanPath();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error 02: Could not read file from disk. Original error: " + ex.Message);
      }
    }

    private void button4_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = Path.GetDirectoryName(this.textBox1.Text);
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.textBox3.Text = folderBrowserDialog.SelectedPath + "\\MRK\\" + this.fileName + "_MRK.jpeg";
      this.selectedPath = folderBrowserDialog.SelectedPath;
      this.limiter();
      if (!File.Exists(this.textBox1.Text) || !File.Exists(this.textBox2.Text))
        return;
      this.Backbone(false);
    }

    private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
    {
    }

    private void Form1_Load(object sender, EventArgs e)
    {
    }

    private void CleanPath() => this.textBox3.Text = (string) null;

    private void limiter()
    {
      if (!string.IsNullOrEmpty(this.textBox1.Text) && !string.IsNullOrEmpty(this.textBox2.Text))
      {
        if (!File.Exists(this.textBox1.Text) || !File.Exists(this.textBox2.Text))
          return;
        Bitmap bitmap1 = new Bitmap(this.textBox1.Text);
        Bitmap bitmap2 = new Bitmap(this.textBox2.Text);
        this.Img2_w = bitmap2.Width;
        this.Img2_h = bitmap2.Height;
        this.Img1_w = bitmap1.Width;
        this.Img1_h = bitmap1.Height;
        this.maxX = (Decimal) (this.Img1_w - this.Img2_w);
        this.maxY = (Decimal) (this.Img1_h - this.Img2_h);
        if (!this.rotateImg.Checked)
        {
          this.numericUpDown1.Maximum = this.maxX * (100M / this.maxX);
          this.numericUpDown2.Maximum = this.maxY * (100M / this.maxY);
        }
        else if (this.rotateImg.Checked)
        {
          this.Img1_w = bitmap1.Height;
          this.Img1_h = bitmap1.Width;
          this.maxX = (Decimal) (this.Img1_w - this.Img2_w);
          this.maxY = (Decimal) (this.Img1_h - this.Img2_h);
          this.numericUpDown1.Maximum = this.maxY * (100M / this.maxY);
          this.numericUpDown2.Maximum = this.maxX * (100M / this.maxX);
        }
        bitmap1?.Dispose();
        bitmap2?.Dispose();
      }
      else
      {
        if (!string.IsNullOrEmpty(this.textBox1.Text) && !string.IsNullOrEmpty(this.textBox2.Text))
          return;
        this.reset();
      }
    }

    private void reset()
    {
      this.maxX = 1M;
      this.maxY = 1M;
      this.numericUpDown1.Maximum = this.maxX;
      this.numericUpDown2.Maximum = this.maxY;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void numericUpDown1_ValueChanged(object sender, EventArgs e) => this.Backbone(false);

    private void numericUpDown2_ValueChanged(object sender, EventArgs e) => this.Backbone(false);

    private void openFile()
    {
      if (string.IsNullOrEmpty(this.textBox3.Text))
        return;
      Process.Start(this.textBox3.Text);
    }

    private void label6_Click(object sender, EventArgs e)
    {
    }

    private void RotateImgCheckedChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.textBox2.Text) || string.IsNullOrEmpty(this.textBox1.Text) || !File.Exists(this.textBox2.Text) || !File.Exists(this.textBox1.Text))
        return;
      this.Backbone(false);
      this.resizeWindow();
      ++this.numericUpDown1.Value;
      --this.numericUpDown1.Value;
    }

    private void resizeWindow()
    {
      int width = 826;
      int height = 365;
      if (this.rotateImg.Checked)
        this.Size = new Size(699, 700);
      else
        this.Size = new Size(width, height);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.button1 = new Button();
      this.textBox1 = new TextBox();
      this.textBox2 = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.textBox3 = new TextBox();
      this.label3 = new Label();
      this.openFileDialog1 = new OpenFileDialog();
      this.button2 = new Button();
      this.button3 = new Button();
      this.button4 = new Button();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.label4 = new Label();
      this.label5 = new Label();
      this.numericUpDown1 = new NumericUpDown();
      this.numericUpDown2 = new NumericUpDown();
      this.label6 = new Label();
      this.pictureBox1 = new PictureBox();
      this.checkBox2 = new CheckBox();
      this.label7 = new Label();
      this.rotateImg = new CheckBox();
      this.numericUpDown1.BeginInit();
      this.numericUpDown2.BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.button1.BackColor = SystemColors.MenuHighlight;
      this.button1.Location = new Point(219, 269);
      this.button1.Name = "button1";
      this.button1.Size = new Size(80, 45);
      this.button1.TabIndex = 11;
      this.button1.Text = "Acabô? Clica aqui.";
      this.button1.UseVisualStyleBackColor = false;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.textBox1.AllowDrop = true;
      this.textBox1.Location = new Point(12, 36);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(185, 20);
      this.textBox1.TabIndex = 2;
      this.textBox1.Text = "C:\\Users\\Kallan\\Pictures\\energy2.jpg";
      this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
      this.textBox2.Location = new Point(11, 85);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(185, 20);
      this.textBox2.TabIndex = 4;
      this.textBox2.Text = "C:\\Users\\Kallan\\Pictures\\visa.png";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(47, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Imagem!";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 69);
      this.label2.Name = "label2";
      this.label2.Size = new Size(63, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Logomarca!";
      this.textBox3.Location = new Point(11, 130);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new Size(184, 20);
      this.textBox3.TabIndex = 6;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 114);
      this.label3.Name = "label3";
      this.label3.Size = new Size(218, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Local para salvar! (ESCOLHA O DESKTOP!)";
      this.openFileDialog1.FileName = "openFileDialog1";
      this.button2.Location = new Point(203, 36);
      this.button2.Name = "button2";
      this.button2.Size = new Size(76, 19);
      this.button2.TabIndex = 1;
      this.button2.Text = "Escolher";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button3.Location = new Point(203, 85);
      this.button3.Name = "button3";
      this.button3.Size = new Size(75, 20);
      this.button3.TabIndex = 3;
      this.button3.Text = "Escolher";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.button4.Location = new Point(203, 130);
      this.button4.Name = "button4";
      this.button4.Size = new Size(76, 19);
      this.button4.TabIndex = 5;
      this.button4.Text = "Escolher";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.folderBrowserDialog1.HelpRequest += new EventHandler(this.folderBrowserDialog1_HelpRequest);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(70, 182);
      this.label4.Name = "label4";
      this.label4.Size = new Size(115, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "X: Distancia horizontal.";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(70, 212);
      this.label5.Name = "label5";
      this.label5.Size = new Size(104, 13);
      this.label5.TabIndex = 13;
      this.label5.Text = "Y: Distancia vertical.";
      this.numericUpDown1.Location = new Point(11, 175);
      this.numericUpDown1.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.numericUpDown1.Name = "numericUpDown1";
      this.numericUpDown1.Size = new Size(53, 20);
      this.numericUpDown1.TabIndex = 7;
      this.numericUpDown1.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
      this.numericUpDown2.Location = new Point(11, 204);
      this.numericUpDown2.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.numericUpDown2.Name = "numericUpDown2";
      this.numericUpDown2.Size = new Size(53, 20);
      this.numericUpDown2.TabIndex = 8;
      this.numericUpDown2.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
      this.label6.AutoSize = true;
      this.label6.Dock = DockStyle.Bottom;
      this.label6.Location = new Point(0, 313);
      this.label6.Name = "label6";
      this.label6.Size = new Size(175, 13);
      this.label6.TabIndex = 16;
      this.label6.Text = "Created by KALLAN SIPPLE (2016)";
      this.label6.Click += new EventHandler(this.label6_Click);
      this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
      this.pictureBox1.Cursor = Cursors.Cross;
      this.pictureBox1.Location = new Point(333, 20);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(465, 297);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 17;
      this.pictureBox1.TabStop = false;
      this.checkBox2.AutoSize = true;
      this.checkBox2.Location = new Point(11, 269);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new Size(145, 17);
      this.checkBox2.TabIndex = 10;
      this.checkBox2.Text = "Abrir arquivo apos salvo?";
      this.checkBox2.UseVisualStyleBackColor = true;
      this.label7.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(200, 175);
      this.label7.Name = "label7";
      this.label7.Size = new Size((int) sbyte.MaxValue, 88);
      this.label7.TabIndex = 20;
      this.label7.Text = "100 = direita, base.\r\n\r\n  1   = esquerda, topo.\r\n  ";
      this.rotateImg.Location = new Point(203, 242);
      this.rotateImg.Name = "rotateImg";
      this.rotateImg.Size = new Size(104, 24);
      this.rotateImg.TabIndex = 21;
      this.rotateImg.Text = "rotate image?";
      this.rotateImg.UseVisualStyleBackColor = true;
      this.rotateImg.CheckedChanged += new EventHandler(this.RotateImgCheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(810, 326);
      this.Controls.Add((Control) this.rotateImg);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.checkBox2);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.numericUpDown2);
      this.Controls.Add((Control) this.numericUpDown1);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.textBox3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBox2);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.button1);
      this.Name = nameof (Form1);
      this.Text = "ImageCombiner 3.2.0b";
      this.Load += new EventHandler(this.Form1_Load);
      this.numericUpDown1.EndInit();
      this.numericUpDown2.EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
