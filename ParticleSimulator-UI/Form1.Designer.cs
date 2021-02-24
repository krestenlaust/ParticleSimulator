
namespace ParticleSimulator_UI
{
    partial class FormGame
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
            if (disposing && (components != null))
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
            this.backgroundWorkerGame = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // backgroundWorkerGame
            // 
            this.backgroundWorkerGame.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerGame_DoWork);
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.button1.Location = new System.Drawing.Point(12, 446);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Powder";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // FormGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 525);
            this.Controls.Add(this.button1);
            this.Name = "FormGame";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormGame_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormGame_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormGame_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormGame_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorkerGame;
        private System.Windows.Forms.Button button1;
    }
}

