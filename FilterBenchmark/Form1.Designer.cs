namespace FilterBenchmark
{
  partial class InputData
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if(disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.Project = new System.Windows.Forms.RadioButton();
      this.Element = new System.Windows.Forms.RadioButton();
      this.Floor = new System.Windows.Forms.RadioButton();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.LeftOffset = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.TopOffset = new System.Windows.Forms.TextBox();
      this.NearClip = new System.Windows.Forms.TextBox();
      this.FarClip = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.label9 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.RightOffset = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.Label();
      this.label14 = new System.Windows.Forms.Label();
      this.BottomOffset = new System.Windows.Forms.TextBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.Project);
      this.groupBox1.Controls.Add(this.Element);
      this.groupBox1.Controls.Add(this.Floor);
      this.groupBox1.Location = new System.Drawing.Point(3, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(320, 50);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Select Method";
      // 
      // Project
      // 
      this.Project.AutoSize = true;
      this.Project.Location = new System.Drawing.Point(232, 19);
      this.Project.Name = "Project";
      this.Project.Size = new System.Drawing.Size(85, 17);
      this.Project.TabIndex = 0;
      this.Project.Text = "Total Project";
      this.Project.UseVisualStyleBackColor = true;
      // 
      // Element
      // 
      this.Element.AutoSize = true;
      this.Element.Location = new System.Drawing.Point(102, 19);
      this.Element.Name = "Element";
      this.Element.Size = new System.Drawing.Size(119, 17);
      this.Element.TabIndex = 0;
      this.Element.Text = "Element By Element";
      this.Element.UseVisualStyleBackColor = true;
      // 
      // Floor
      // 
      this.Floor.AutoSize = true;
      this.Floor.Checked = true;
      this.Floor.Location = new System.Drawing.Point(13, 19);
      this.Floor.Name = "Floor";
      this.Floor.Size = new System.Drawing.Size(75, 17);
      this.Floor.TabIndex = 0;
      this.Floor.TabStop = true;
      this.Floor.Text = "Floor Wise";
      this.Floor.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(14, 56);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(56, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Left Offset";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(14, 152);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(95, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Near Clip Distance";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(14, 104);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(57, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Top Offset";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(14, 176);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(87, 13);
      this.label4.TabIndex = 1;
      this.label4.Text = "Far Clip Distance";
      // 
      // LeftOffset
      // 
      this.LeftOffset.Location = new System.Drawing.Point(123, 52);
      this.LeftOffset.Name = "LeftOffset";
      this.LeftOffset.Size = new System.Drawing.Size(84, 20);
      this.LeftOffset.TabIndex = 2;
      this.LeftOffset.Text = "0.0";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(213, 56);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(39, 13);
      this.label5.TabIndex = 1;
      this.label5.Text = "in Feet";
      // 
      // TopOffset
      // 
      this.TopOffset.Location = new System.Drawing.Point(123, 100);
      this.TopOffset.Name = "TopOffset";
      this.TopOffset.Size = new System.Drawing.Size(84, 20);
      this.TopOffset.TabIndex = 2;
      this.TopOffset.Text = "0.0";
      // 
      // NearClip
      // 
      this.NearClip.Location = new System.Drawing.Point(123, 148);
      this.NearClip.Name = "NearClip";
      this.NearClip.Size = new System.Drawing.Size(84, 20);
      this.NearClip.TabIndex = 2;
      this.NearClip.Text = "0.1";
      // 
      // FarClip
      // 
      this.FarClip.Location = new System.Drawing.Point(123, 172);
      this.FarClip.Name = "FarClip";
      this.FarClip.Size = new System.Drawing.Size(84, 20);
      this.FarClip.TabIndex = 2;
      this.FarClip.Text = "0.1";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(213, 104);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(39, 13);
      this.label6.TabIndex = 1;
      this.label6.Text = "in Feet";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(213, 152);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(39, 13);
      this.label7.TabIndex = 1;
      this.label7.Text = "in Feet";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(213, 176);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(39, 13);
      this.label8.TabIndex = 1;
      this.label8.Text = "in Feet";
      // 
      // button1
      // 
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.Location = new System.Drawing.Point(23, 204);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(132, 35);
      this.button1.TabIndex = 3;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.Location = new System.Drawing.Point(178, 204);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(113, 35);
      this.button2.TabIndex = 3;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label9.Location = new System.Drawing.Point(12, 248);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(306, 13);
      this.label9.TabIndex = 1;
      this.label9.Text = "Note: Section Name Should be \"Electrical GangBox\"";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label10.Location = new System.Drawing.Point(12, 263);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(114, 13);
      this.label10.TabIndex = 1;
      this.label10.Text = "Otherwise Shows Error";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(14, 80);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(63, 13);
      this.label11.TabIndex = 1;
      this.label11.Text = "Right Offset";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(213, 80);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(39, 13);
      this.label12.TabIndex = 1;
      this.label12.Text = "in Feet";
      // 
      // RightOffset
      // 
      this.RightOffset.Location = new System.Drawing.Point(123, 76);
      this.RightOffset.Name = "RightOffset";
      this.RightOffset.Size = new System.Drawing.Size(84, 20);
      this.RightOffset.TabIndex = 2;
      this.RightOffset.Text = "0.0";
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(213, 128);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(39, 13);
      this.label13.TabIndex = 1;
      this.label13.Text = "in Feet";
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(14, 128);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(71, 13);
      this.label14.TabIndex = 1;
      this.label14.Text = "Bottom Offset";
      // 
      // BottomOffset
      // 
      this.BottomOffset.Location = new System.Drawing.Point(123, 124);
      this.BottomOffset.Name = "BottomOffset";
      this.BottomOffset.Size = new System.Drawing.Size(84, 20);
      this.BottomOffset.TabIndex = 2;
      this.BottomOffset.Text = "0.1";
      // 
      // InputData
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(329, 279);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.FarClip);
      this.Controls.Add(this.NearClip);
      this.Controls.Add(this.BottomOffset);
      this.Controls.Add(this.TopOffset);
      this.Controls.Add(this.RightOffset);
      this.Controls.Add(this.LeftOffset);
      this.Controls.Add(this.label14);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label13);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.label9);
      this.Controls.Add(this.label12);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label11);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.groupBox1);
      this.Name = "InputData";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Electrical Switches";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton Project;
    private System.Windows.Forms.RadioButton Element;
    private System.Windows.Forms.RadioButton Floor;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox LeftOffset;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox TopOffset;
    private System.Windows.Forms.TextBox NearClip;
    private System.Windows.Forms.TextBox FarClip;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.TextBox RightOffset;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.TextBox BottomOffset;
  }
}