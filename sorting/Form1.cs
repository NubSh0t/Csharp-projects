namespace form;

using System.Windows.Forms;
using System;

public partial class Form1 : Form
{
    Timer timer1 = new Timer();

    int FrameRate=60;

    const int size =200;

    int currentsort=1;
    Label l= new Label();
    Label l2= new Label();

    static int[] unsorted=new int[size];
    int[] sorted=new int[size];

    Pen Pen1 = new Pen(System.Drawing.Color.Black,(800/(float)size));
    Pen Pen2 = new Pen(System.Drawing.Color.Red, (800/(float)size));
    
    int swaps=0;

    Random gen= new Random();

    public Form1()
    {
        InitializeComponent();
        l.Size = new Size(200, 50);
        l.Font= new Font("Serif",15);
        this.Controls.Add(l);
        l2.Location=new Point(600,0);
        l2.Size = new Size(200, 50);
        l2.Font= new Font("Serif",15);
        this.Controls.Add(l2);

        this.Paint+=new PaintEventHandler(Draw);
        timer1.Enabled = true;
        timer1.Tick += new System.EventHandler(this.Refresh);
        timer1.Start();
        this.DoubleBuffered=true;

        for (int i = 0; i < size; i++)
        {
            unsorted[i]=gen.Next(0,500);
            sorted[i]=unsorted[i];
        }


    }
    
    private void Draw(object sender,PaintEventArgs e)
    {
        for(int i=0;i<size;i++)
        {
            e.Graphics.DrawLine(Pen1,(i+0.5f)*(700/(float)size)+50,550,(i+0.5f)*(700/(float)size)+50,550-sorted[i]);
        }

        switch (currentsort)
        {
            case 1:
                l.Text="Bubble Sort";
                l2.Text="Swaps: "+swaps.ToString();
                bubblesort(Pen2,e);
                break;
            case 2:
                l.Text="Selection Sort";
                l2.Text="Swaps: "+swaps.ToString();
                selectionsort(Pen2,e);
                break;
            case 3:
                l.Text="Quick Sort";
                l2.Text="Swaps: "+swaps.ToString();
                quicksort(Pen2,e);
                break;
            default:
                selectionsort(Pen2,e);
                break;
        }

    }

    private void Refresh(object sender,EventArgs e)
    {
        this.Refresh();
        timer1.Interval=(int)(1000/FrameRate);
    }

    void bubblesort(Pen p,PaintEventArgs e)
    {
        int index=check(sorted,size);
        if (index==-1)
        {
            currentsort++;
            for (int i = 0; i < size; i++)
            {
                sorted[i]=unsorted[i];
            }
            swaps=0;
            return;
        }
   

        int temp;
        for(int i=0;i<index;i++)
        {
            if (sorted[i]>sorted[i+1])
            {
                temp=sorted[i];
                sorted[i]=sorted[i+1];
                sorted[i+1]=temp;
                swaps++;
            }
            
        }

        e.Graphics.DrawLine(Pen2,(index+0.5f)*(700/(float)size)+50,550,(index+0.5f)*(700/(float)size)+50,550-sorted[index]);
    }

    void selectionsort(Pen p,PaintEventArgs e)
    {
        int index=check(sorted,size);
        if (index==-1)
        {
            currentsort++;
            for (int i=0; i< size;i++)
            {
                sorted[i]=unsorted[i];
            }
            swaps=0;
            return;
        }


        int temp;
        int max=0;
        for(int i=1;i<=index;i++)
        {
            if (sorted[i]>sorted[max])
            {
                max=i;
            }
        }
        temp=sorted[max];
        sorted[max]=sorted[index];
        sorted[index]=temp;
        swaps++;

        e.Graphics.DrawLine(Pen2,(max+0.5f)*(700/(float)size)+50,550,(max+0.5f)*(700/(float)size)+50,550-sorted[max]); 
    }

    void quicksort(Pen p,PaintEventArgs e,int f=0,int l=size-1,bool ismain=true)
    {
        if(f>=l)
        {
            return;
        }

        if (ismain)
        {
            int index=check(sorted,size);
            if(index==-1)
            {
                currentsort=1;
                for (int i = 0; i < size; i++)
                {
                    unsorted[i]=gen.Next(0,500);
                    sorted[i]=unsorted[i];
                }
                swaps=0;
                return;
            }
        }
        int temp;

        int pivot=-1;
        //try to find pivot

        int min=1000;
        int max=-1;
        int[] minarr= new int[l-f+1];
        int[] maxarr= new int[l-f+1];

        for(int i=f;i<=l;i++)
        {
            if(sorted[i]>max)
            {
                max=sorted[i];
            }
            if(sorted[l+f-i]<min)
            {
                min=sorted[l+f-i];
            }
            minarr[l-i]=min;
            maxarr[i-f]=max;
        }

        for(int i=0;i<=l-f;i++)
        {
            if(minarr[i]==maxarr[i])
            {
                pivot=i+f;
                break;
            }
        }


        if (pivot==-1)
        {
            pivot=l;

            bool b = true;
            int pointer=-1;

            //find greater
            for(int i=f;i<pivot;i++)
            {
                if(sorted[i]>sorted[pivot])
                {
                    pointer=i;
                    break;
                }
            }

            //find smaller and swap
            for(int i=pointer;i<pivot;i++)
            {
                if (sorted[i]<sorted[pivot])
                {
                    temp=sorted[pointer];
                    sorted[pointer]=sorted[i];
                    sorted[i]=temp;
                    pointer=i;
                    b=false;
                    swaps++;
                }
            }
            
            if(b)
            {
                temp=sorted[pointer];
                sorted[pointer]=sorted[pivot];
                sorted[pivot]=temp;
                swaps++;
            }

        }
        else
        {
            quicksort(p,e,f,pivot-1,false);
            quicksort(p,e,pivot+1,l,false);
        }

        e.Graphics.DrawLine(Pen2,(pivot+0.5f)*(700/(float)size)+50,550,(pivot+0.5f)*(700/(float)size)+50,550-sorted[pivot]);
    }

    int check(int[] arr, int n)
    {
        if(n<=1)
        {
            return -1;
        }

        int max=0;
        for(int i=1;i<n;i++)
        {
            if (sorted[i]>=sorted[max])
            {
                max=i;
            }
        }
        
        if (max==n-1)
        {
            return check(arr, n - 1);
        }

        return n-1;
    }
}
