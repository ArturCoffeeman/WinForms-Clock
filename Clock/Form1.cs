using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class Form1 : Form
    {
        SizeF c;  // Ось стрелок
      
        PointF p0; // Короткий конец всех стрелок
        PointF pH; // Длинный конец часовой стрелки
        PointF pM; // Длинный конец минутной и секундной стрелок
        PointF pD0, pD1; // деление циферблата
        DateTime lastUpdate = DateTime.Now;

        public Form1()
        {
            InitializeComponent();
            // Вычисляем координаты стрелок и делений в вертикальном положении            
            c = new SizeF(ClientSize.Width / 2.0f, ClientSize.Height / 2.0f);
            float r = Math.Min(ClientSize.Width, ClientSize.Height) / 2.5f; // Радиус циферблата чуть меньше половины формы
            p0 = new PointF(0, -r * .2f); pH = new PointF(0, r * .5f);
            pM = new PointF(0, r * .9f);
            pD0 = new PointF(0, .8f * r); pD1 = new PointF(0, r);
            timer1.Start(); 
        }
        // Рисование ручкой pen чёрточки от p0 до p1, развёрнутой относительно оси стрелок на угол a  
        void DrawDash(Graphics g, Pen pen, PointF p0, PointF p1, float a)
        {
            // Преобразования такие, потому что ось Y GDI направлена вниз
            float a1 = -(float)Math.Cos(a), b1 = (float)Math.Sin(a),
                a2 = -(float)Math.Sin(a), b2 = -(float)Math.Cos(a);
            g.DrawLine(pen,
                new PointF(p0.X * a1 + p0.Y * b1, p0.X * a2 + p0.Y * b2) + c,
                new PointF(p1.X * a1 + p1.Y * b1, p1.X * a2 + p1.Y * b2) + c);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - lastUpdate > TimeSpan.FromSeconds(1)) { lastUpdate = DateTime.Now; Invalidate(); } 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Рисуем циферблат
            // 12 часов
            //  DrawDash(e.Graphics, new Pen(Brushes.DarkGray, 10), pD0, pD1, 0);
            // Остальной циферблат
            //for (int ct = 1; ct < 12; ct++)
              //  DrawDash(e.Graphics, new Pen(Brushes.DarkGray, 3), pD0, pD1, ct * (float)Math.PI / 6.0f);
            // Часовая стрелка рисуется так, чтобы она не перескакивала по делениям, а аналогово двигалась            
            DrawDash(e.Graphics, new Pen(Brushes.White, 4), p0, pH,
                (lastUpdate.Hour * 60f + lastUpdate.Minute) * (float)Math.PI / 360f);
            // Остальные стрелки. Пусть перескакивают.
            DrawDash(e.Graphics, new Pen(Brushes.White, 2), p0, pM, lastUpdate.Minute * (float)Math.PI / 30.0f);
            DrawDash(e.Graphics, new Pen(Brushes.Red, 1), p0, pM, lastUpdate.Second * (float)Math.PI / 30.0f);


        }

    }
}
