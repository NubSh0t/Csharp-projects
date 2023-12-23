namespace form;

using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Form1 : Form
{
    Timer timer1 = new Timer();

    int FrameRate=60;

    int[,] Board = {{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0}};
    const double Inf = 9999999999999999;
    const double NInf = -9999999999999999;

    System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 40);
    System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
    System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
    System.Drawing.Graphics formGraphics;


    Random rnd = new Random();

    //score gradient
    int[,,] gradient={
    {{10,9,8,7},{3,4,5,6},{2,1,0,0},{0,0,0,0}},
    {{7,8,9,10},{6,5,4,3},{0,0,1,2},{0,0,0,0}},
    {{0,0,0,0},{2,1,0,0},{3,4,5,6},{10,9,8,7}},
    {{0,0,0,0},{0,0,1,2},{6,5,4,3},{7,8,9,10}},

    {{10,3,2,0},{9,4,1,0},{8,5,0,0},{7,6,0,0}},
    {{7,6,0,0},{8,5,0,0},{9,4,1,0},{10,3,2,0}},
    {{0,2,3,10},{0,1,4,9},{0,0,5,8},{0,0,6,7}},
    {{0,0,6,7},{0,0,5,8},{0,1,4,9},{0,2,3,10}}
};


    public Form1()
    {
        InitializeComponent();

        this.Paint+=new PaintEventHandler(Draw);
        timer1.Enabled = true;
        timer1.Tick += new System.EventHandler(this.Refresh);
        timer1.Start();
        this.DoubleBuffered=true;

        drawFormat.Alignment = StringAlignment.Center;
        drawFormat.LineAlignment = StringAlignment.Center;

        List<int[]> l=empty(Board);
        int num1 = rnd.Next(0,l.Count-1);
        int num2 = rnd.Next(0,l.Count-1);
        while (num2==num1)
        {
            num2 = rnd.Next(0,l.Count-1);
        }

        int num3=rnd.Next(0,100);
        if (num3 < 90)
        {
            Board[l[num1][0],l[num1][1]]=1;
        }
        else
        {
            Board[l[num1][0],l[num1][1]]=2;
        }

        num3=rnd.Next(0,100);
        if (num3 < 90)
        {
            Board[l[num2][0],l[num2][1]]=1;
        }
        else
        {
            Board[l[num2][0],l[num2][1]]=2;
        }
            
    }
    
    private void Draw(object sender,PaintEventArgs e)
    {
        show(Board,e);
        Board=search(Board,0.01);
        tick(Board);
    }

    //refresh screen with constant fps
    private void Refresh(object sender,EventArgs e)
    {
        this.Refresh();
        timer1.Interval=(int)(1000/FrameRate);
    }

    //convert one range of number into another range
    private int remap(int value,int min1,int max1,int min2,int max2)
    {
        return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    }

    private double score(int[,] b)
    {
        double score=0;


        for (int i = 0; i < 4; i++)
        {
            // monotonicity of board
            if (b[i,0]>=b[i,1] && b[i,1]>=b[i,2] && b[i,2]>=b[i,3])
            {
                score+=16;
            }

            else if (b[i,0]<=b[i,1] && b[i,1]<=b[i,2] && b[i,2]<=b[i,3])
            {
                score+=16;
            }
            else
            {
                score-=16;
            }

            if (b[0,i]>=b[1,i] && b[1,i]>=b[2,i] && b[2,i]>=b[3,i])
            {
                score+=16;
            }
            else if (b[0,i]<=b[1,i] && b[1,i]<=b[2,i] && b[2,i]<=b[3,i])
            {
                score+=16;
            }
            else
            {
                score-=16;
            }

            for (int j = 0; j < 4; j++)
            {
                if (b[i,j]==0)
                {
                    score+=4;  //empty board is better
                }
            }
        
        }
            
        double maximum= NInf;
        for (int k = 0;k < 4; k++)
        {
            double current=0;
            for (int i = 0; i < 4; i++)
            {       
                for (int j = 0; j < 4; j++)
                {       
                    current+=Math.Pow(2,gradient[k,i,j])*Math.Pow(2.5,b[i,j]);
                }
            }

            maximum=Math.Max(maximum,current);
        }
        score+=maximum;

        return score;
    }
    
    private List<int[,]> moves(int[,] B)
    {
        List<int[,]> boards = new List<int[,]>();
        bool moved=false;

        int[,] Board=copy(B);
        for (int j = 0; j < 4; j++)
        {
            int i=0;
            while (i<3)
            {
                if (Board[i,j]==0)
                {
                    if (Board[i+1,j]!=0)
                    {
                        Board[i,j]=Board[i+1,j];
                        Board[i+1,j]=0;
                        moved=true;
                        i=-1;
                    }
                }
                else
                {
                    if (Board[i,j]==Board[i+1,j])
                    {
                        Board[i,j]++;
                        Board[i+1,j]=0;
                        moved=true;
                        i=-1;
                    }
                }
                i++;
            }
        }

        if (moved)
        {
            boards.Add(Board);
            moved=false;
        }

        Board=copy(B);
        for (int i = 0; i < 4; i++)
        {
            int j=0;
            while (j<3)
            {
                if (Board[i,j]==0)
                {
                    if (Board[i,j+1]!=0)
                    {
                        Board[i,j]=Board[i,j+1];
                        Board[i,j+1]=0;
                        moved=true;
                        j=-1;
                    }
                }
                else
                {
                    if (Board[i,j]==Board[i,j+1])
                    {
                        Board[i,j]++;
                        Board[i,j+1]=0;
                        moved=true;
                        j=-1;
                    }
                }
                j++;
            }
        }

        if (moved)
        {
            boards.Add(Board);
            moved=false;
        }

        Board=copy(B);
        for (int i = 0; i < 4; i++)
        {
            int j=3;
            while (j>0)
            {
                if (Board[i,j]==0)
                {
                    if (Board[i,j-1]!=0)
                    {
                        Board[i,j]=Board[i,j-1];
                        Board[i,j-1]=0;
                        moved=true;
                        j=4;
                    }
                }
                else
                {
                    if (Board[i,j]==Board[i,j-1])
                    {
                        Board[i,j]++;
                        Board[i,j-1]=0;
                        moved=true;
                        j=4;
                    }
                }
                j--;
            }
        }

        if (moved)
        {
            boards.Add(Board);
            moved=false;
        }

        Board=copy(B);
        for (int j = 0; j < 4; j++)
        {
            int i=3;
            while (i>0)
            {
                if (Board[i,j]==0)
                {
                    if (Board[i-1,j]!=0)
                    {
                        Board[i,j]=Board[i-1,j];
                        Board[i-1,j]=0;
                        moved=true;
                        i=4;
                    }
                }
                else
                {
                    if (Board[i,j]==Board[i-1,j])
                    {
                        Board[i,j]++;
                        Board[i-1,j]=0;
                        moved=true;
                        i=4;
                    }
                }
                i--;
            }
        }

        if (moved)
        {
            boards.Add(Board);
            moved=false;
        }

        return boards;
    }

    private int[,] copy(int[,] Board)
    {
        int[,] b = new int[4,4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                b[i,j]=Board[i,j];
            }
        }
        return b;
    }

    private List<int[]> empty(int[,] Board)
    {
        List<int[]> emp = new List<int[]>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Board[i,j]==0)
                {
                    int[] pos={i,j};
                    emp.Add(pos);
                }
            }
        }
        return emp;
    }

    private void tick(int[,] Board)
    {
        List<int[]> l=empty(Board);
        if (l.Count>0)
        {
            int num1 = rnd.Next(0,l.Count-1);

            double num3=rnd.NextDouble();
            if (num3 < 0.9)
            {
                Board[l[num1][0],l[num1][1]]=1;
            }
            else
            {
                Board[l[num1][0],l[num1][1]]=2;
            }
        }
    }

    private void show(int[,] Board,PaintEventArgs e)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                double r= remap(Board[i,j],0,15,150,250);
                double g= remap(Board[i,j],0,15,150,70);
                double b= remap(Board[i,j],0,15,150,20);

                SolidBrush brush = new SolidBrush(Color.FromArgb(255,(byte)r,(byte)g,(byte)b));
                Pen pen = new Pen(Color.FromArgb(255,(byte)30,(byte)30,(byte)30), 5);

                RectangleF rect = new RectangleF(j*200,i*200,200,200);
                e.Graphics.FillRectangle(brush, rect);
                e.Graphics.DrawRectangle(pen,rect);
                if (Board[i,j]!=0)
                {
                    e.Graphics.DrawString(((int)Math.Pow(2,Board[i,j])).ToString(),drawFont,drawBrush,100+j*200,100+i*200,drawFormat);
                }
            }
        }
    }

    private int[,] search(int[,] Board,double t)
    {
        var sw = new Stopwatch();
        sw.Start();
        double maximum=NInf;
        int[,] best=Board;
        for(int i = 0; i < 15;i++)
        {
            if(sw.ElapsedMilliseconds>t*1000)
            {
                break;
            }
            foreach(int[,] move in moves(Board))
            {
                double s=expectimax(move,i);
                if(s>maximum)
                {
                    maximum=s;
                    best=move;
                }
            }
        }

        return best;
    }

    private double expectimax(int[,] Board,int depth,double prob=1,bool state=false,double alpha=NInf,double beta=Inf)
    {
        if (depth == 0 || prob <0.001)
        {
            return score(Board);
        }

        if (state)
        {
            double maximum=NInf;
            foreach(int[,] b in moves(Board))
            {
                maximum=Math.Max(expectimax(b,depth-1,prob,false,alpha,beta),maximum);

                if (maximum >= beta)
                {
                    break;
                }

                alpha = Math.Max(maximum, alpha);
            }

            return maximum;
        }
        else
        {
            double minimum=Inf;
            List<int[]> b=empty(Board);

            for(int i=0;i<b.Count;i++)
            {
                Board[b[i][0],b[i][1]]=1;
                minimum=Math.Min(expectimax(Board,depth-1,prob*0.9,true,alpha,beta),minimum);
                Board[b[i][0],b[i][1]]=0;

                if (minimum <= alpha)
                {
                    break;
                }

                Board[b[i][0],b[i][1]]=2;
                minimum=Math.Min(expectimax(Board,depth-1,prob*0.1,true,alpha,beta),minimum);
                Board[b[i][0],b[i][1]]=0;
                if (minimum <= alpha)
                {
                    break;
                }

                beta = Math.Min(minimum, beta);
            }
            return minimum;
        }
    }
}
