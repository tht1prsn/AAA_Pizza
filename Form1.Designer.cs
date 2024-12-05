using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.AccessControl;
using System.Collections;
namespace AAA_Pizza;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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


    private void InitializeComponent()
    {
        this.DoubleBuffered = true;
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size((int)this.FormSize.X, (int)this.FormSize.Y);
        this.Text = "THE ONLY PIZZA PLACE YOULL EVER NEED AN APP FOR";
        this.FormBorderStyle = FormBorderStyle.None;
        this.MinimizeBox = false;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(255, 180, 180, 180);

        OrderEntries = new List<OrderEntry>();

        #region Pizza Designer
        this.pd = new()
        {
            Size = new Size(500, 800),
            Location = new Point((int)(FormSize.X / 2 - (500 / 2)), (int)(FormSize.Y / 2 * .25)),
            BackColor = Color.LightGray
        };
        this.Controls.Add(pd);
        pd.Visible = false;

        Panel DesignerTopPanel = new()
        {   
            Size = new Size(pd.Size.Width, (int)(pd.Size.Height / 6.0)),
            BackColor = Color.FromArgb(255, 150, 70, 70)
        };
        pd.Controls.Add(DesignerTopPanel);

        DesignerTopLabel = new()
        {
            Text = GetRandomTextFromFile(DialogueFileIndex.DesignerLabelDialogue, OrderEntries.Count),
            Font = FontLarge,
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(pd.Size.Width, (int)(pd.Size.Height / 6.0)),
            ForeColor = Color.LightGray
        };
        DesignerTopPanel.Controls.Add(DesignerTopLabel);



        FlowLayoutPanel DesignerMainPane = new()
        {
            Size = new Size(pd.Size.Width, (int)(pd.Size.Height * 17/24.0)),
            Location = new Point(0, (int)(pd.Size.Height / 6.0)),
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = false
        };
        pd.Controls.Add(DesignerMainPane);

        Label OrderType = new()
        {
            Text = "Select order type",
            Size = new Size(DesignerMainPane.Size.Width, (int)(DesignerMainPane.Size.Height * .08)),
            Font = FontLarge,
            //TextAlign = ContentAlignment.CenterLeft
        };
        DesignerMainPane.Controls.Add(OrderType);

        PizzaOrDrinkChoice = new()
        {
            L = Image.FromFile("Resources\\PizzaRadioIcon.png"),
            R = Image.FromFile("Resources\\DrinkRadioIcon.png"),
            Size = new Size((int)(DesignerMainPane.Size.Width * .75), (int)(DesignerMainPane.Size.Width / 2.0 * .7)),
            Location = new Point(0, 0),
            
        };
        PizzaOrDrinkChoice.OnChange += PizzaOrDrinkRadioButton_OnChange;
        DesignerMainPane.Controls.Add(PizzaOrDrinkChoice);

        DesignerGroupBoxPizza = new()
        {
            Size = DesignerMainPane.Size,
        };
        DesignerGroupBoxDrink = new()
        {
            Size = DesignerMainPane.Size,
            AutoSize = false
        };
        DesignerGroupBoxPizza.Visible = false;
        DesignerGroupBoxDrink.Visible = false;
        DesignerMainPane.Controls.Add(DesignerGroupBoxPizza);
        DesignerMainPane.Controls.Add(DesignerGroupBoxDrink);

        DesignerGroupBoxDrink.Controls.Add(new Label() {Text="Select drink", Size=new Size(DesignerGroupBoxDrink.Size.Width, 60), Font=FontLarge});

        for(int i = 0; i<Drinks.Length;++i)
        {
            DesignerGroupBoxDrink.Controls.Add(new RadioButtonRect() {Text=Drinks[i],Location=new Point(60, 60+(40*i)),Size=new Size(200, 40),Font=FontSmall});
        }
        

        foreach(Control c in DesignerGroupBoxDrink.Controls)
        {
            try
            {
                (c as RadioButtonRect).Click += DrinkChoiceSelectionChanges;
            }
            catch(Exception) {}
        }


        
        DesignerGroupBoxPizza.Controls.Add(new Label() {Text="You want it stuffed? \n(only $2.00!!)", Size=new Size(300, 100), Font=FontSmall});
        DesignerGroupBoxPizza.Controls.Add(new ToggleButton() {Text="Yes, Please!", Font=FontSmall, Size=new Size(180, 40), Location = new Point(300, 20)});


        DesignerGroupBoxPizza.Controls.Add(new Label() {Text="What about some toppings?($1.00 each)",Size = new Size(DesignerGroupBoxPizza.Size.Width, 40), Font=new Font(new FontFamily("Microsoft Sans Serif"), 18.4f), Location = new Point(0, 120)});
        for(int i = 0; i < Toppings.Length; ++i)
        {
            CheckBox tmpBox = new() {Text=Toppings[i],Font = new Font(new FontFamily("Microsoft Sans Serif"), 16.2f), Location = new Point(i/3*160, 160 + i%3 * 40), Size=new Size(160, 40)};
            DesignerGroupBoxPizza.Controls.Add(tmpBox);
        }


        Panel DesignerBottomPanel = new()
        {
            Location = new Point(0, (int)(pd.Size.Height * 7/8.0)),
            Size = new Size(pd.Size.Width, (int)(pd.Size.Height / 8.0)),
            BackColor = Color.Gray
        };
        pd.Controls.Add(DesignerBottomPanel);

        Button DesignerCancelButton = new()
        {
            Text = "cancel",
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Salmon,
            ForeColor = Color.LightGray,
            Font = FontLarge,
            Location = new Point(0, (int)(DesignerBottomPanel.Size.Height * .10)),
            Size = new Size((int)(DesignerBottomPanel.Size.Width / 2), (int)(DesignerBottomPanel.Size.Height * .85)),
        };
        DesignerCancelButton.Click += DesignerPanelCancelButton_Click;
        DesignerBottomPanel.Controls.Add(DesignerCancelButton);

        DesignerAddEntryButton = new()
        {
            Text = "Add Entry",
            Font = FontLarge,
            BackColor = Color.Gray,
            ForeColor = Color.DarkGray,
            Location = new Point((int)(DesignerBottomPanel.Size.Width / 2), (int)(DesignerBottomPanel.Size.Height * .10)),
            Size = new Size((int)(DesignerBottomPanel.Size.Width / 2), (int)(DesignerBottomPanel.Size.Height * .85)),
            
        };
        DesignerAddEntryButton.Click += Desginer_AddEntry_Click;
        DesignerBottomPanel.Controls.Add(DesignerAddEntryButton);
        #endregion


        #region main Page
        this.TopFrame = new()
        {
            Location = new Point(0, 0),
            Size = new Size((int)FormSize.X, (int)(FormSize.Y * 1/7.0)),
            BackColor = Color.FromArgb(255, 181, 4, 41),
        };
        this.Controls.Add(TopFrame);

        this.exitButton = new StyledButton();
        this.exitButton.FlatStyle = FlatStyle.Flat;
        this.exitButton.Text = "X";
        this.exitButton.Font = FontSmall;
        this.exitButton.Location = new System.Drawing.Point((int)(FormSize.X*.75), (int)(FormSize.Y*.05));
        this.exitButton.Size = new System.Drawing.Size(100, 45);
        this.exitButton.Click += ExitApp;
        TopFrame.Controls.Add(exitButton);
        

        this.MainName = new()
        {
            Location = new Point((int)(FormSize.X * .05), 0),
            Size = new Size((int)FormSize.X, (int)(FormSize.Y * 1/7.0)),
            Text = "Autentica pizza Italiana",
            TextAlign = ContentAlignment.MiddleLeft,
            Font = FontLarge,
            ForeColor = Color.LightGray,
            Parent = TopFrame
        };
        TopFrame.Controls.Add(MainName);


        this.BottomFrame = new()
        {
            Location = new Point(0, (int)(FormSize.Y * (6/7.0))),
            Size = new Size((int)FormSize.X, (int)(FormSize.Y/7.0+.2)),
            BackColor = Color.FromArgb(255, 37, 35, 47)
        };
        this.Controls.Add(BottomFrame);
        DeliveryOrPickup = new DoubleSquareRadioButton() {Location = new Point(15, 15), Size=new Size(180, 100), L=Image.FromFile("Resources\\Delivery.png"), R=Image.FromFile("Resources\\Pick-Up.png")};
        DeliveryOrPickup.OnChange += DorPU_OnChange;
        BottomFrame.Controls.Add(DeliveryOrPickup);
        checkoutButton = new StyledButton() {Text="Proceed to checkout", Location=new Point((int)(FormSize.X * .60), 30), Size=new Size(200, 85), Font=new Font(new FontFamily("Microsoft Sans Serif"), 18.2f), BackColor=Color.Gray};
        checkoutButton.Click += CheckoutButton_Click;
        BottomFrame.Controls.Add(checkoutButton);






        MainFormLayoutPanel = new()
        {
            Size = new Size((int)(FormSize.X), (int)(FormSize.Y * 5.0/7.0)),
            Location = new Point(0, (int)(FormSize.Y * 1.0/7.0))
        };
        this.Controls.Add(MainFormLayoutPanel);

        MainFormLayoutPanel.Controls.Add(new Label(){Text="Pizzas starting at just 12.99\nand 2L drinks for only 1.99!", Size=new Size(MainFormLayoutPanel.Size.Width, 80), Font=FontSmall, TextAlign=ContentAlignment.MiddleCenter});


        this.AddButtonLabel = new()
        {
            Location = new Point((int)(MainFormLayoutPanel.Size.Width * .05), (int)(MainFormLayoutPanel.Size.Height / 2 * .25)),
            Size = new Size((int)(FormSize.X * .9), (int)(MainFormLayoutPanel.Size.Height * .15)),
            Text = GetRandomTextFromFile(DialogueFileIndex.AddButtonDialogue, OrderEntries.Count),
            Font = FontLarge,
            AutoSize = false,
            TextAlign = ContentAlignment.BottomCenter,
        };
        MainFormLayoutPanel.Controls.Add(AddButtonLabel);

        this.AddButton = new("Resources\\AddButton.png")
        {
            Location = new Point((int)(MainFormLayoutPanel.Size.Width / 2 - 125), (int)(MainFormLayoutPanel.Size.Height / 2 * .65)),
            Size = new Size(250, 250),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.Transparent,
            BackColor = Color.Transparent
        };
        this.AddButton.BackgroundImage = Image.FromFile("Resources\\AddButton.png");
        this.AddButton.FlatAppearance.BorderSize = 0;
        this.AddButton.Click += AddButton_Click;
        MainFormLayoutPanel.Controls.Add(AddButton);
        
        OrderPanel = new()
        {
            Location = new Point(0, (int)(FormSize.Y * (4.0/7.0))),
            Size = new Size((int)(FormSize.X-10), (int)(FormSize.Y * (1.0/7.0))),
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
        };
        OrderPanel.Scroll += OrderPanelScroll;
        MainFormLayoutPanel.Controls.Add(OrderPanel);

        
        #endregion

    }

    FlowLayoutPanel OrderPanel;
    List<OrderEntry> OrderEntries;
    DoubleSquareRadioButton PizzaOrDrinkChoice;
    GroupBox DesignerGroupBoxPizza;
    GroupBox DesignerGroupBoxDrink;
    Vector2 FormSize;
    Font FontSmall;
    Font FontLarge;
    private Panel pd;
    Label DesignerTopLabel;
    private Panel TopFrame;
    private Panel BottomFrame;
    private StyledButton exitButton;
    private Label MainName;
    private ImageButton AddButton;
    private Label AddButtonLabel;
    Button DesignerAddEntryButton;
    Panel MainFormLayoutPanel;
    
    string[] Drinks = {"Sprite", "Dr. Pepper", "Coke", "Fanta"};
    string[] Toppings = {"Extra cheese", "Pepperoni", "Sasuage", "Pineapple", "Bacon", "Olives", "Peppers", "Mushrooms", "Spinach"};

    double total, subtotal;
    const double tax = .08;
    const double deliveryFee = 11.99;

    StyledButton checkoutButton;
    DoubleSquareRadioButton DeliveryOrPickup;
}

#region Misc Classes
public class Vector2
{
    public double X {get; set;}
    public double Y {get; set;}

    public Vector2(double x, double y)
    {
        X = x;
        Y = y;
    }
}
public class StyledButton : Button
{
    private int borderSize = 0;
    private int borderRadius = 40;
    private Color borderColor = Color.PaleVioletRed;

    public int BorderSize
    {
        get { return borderSize;}
        set
        {
            borderSize = value;
            this.Invalidate();
        }
    }
    public int BorderRadius
    {
        get { return borderRadius;}
        set
        {
            borderRadius = value;
            this.Invalidate();
        }
    }
    public Color BorderColor
    {
        get { return borderColor;}
        set {
            borderColor = value;
            this.Invalidate();
        }
    }

    public StyledButton()
    {
        borderSize = 0;
        borderRadius = 40;
        borderColor = Color.PaleVioletRed;

        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0;
        this.Size = new Size(150, 40);
        this.BackColor = Color.MediumSlateBlue;
        this.ForeColor = Color.White;
    }


    private GraphicsPath GetFigurePath(RectangleF rect, float radius)
    {
        GraphicsPath path = new();
        path.StartFigure();
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
        path.AddArc(rect.Width-radius, rect.Y, radius, radius, 270, 90);
        path.AddArc(rect.Width-radius, rect.Height-radius, radius, radius, 0, 90);
        path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
        path.CloseFigure();

        return path;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        RectangleF rectSurface = new(0, 0, this.Width, this.Height);
        RectangleF rectBorder = new(1, 1, this.Width-.8f, this.Height-1);

        if(borderRadius > 2)
        {
            using(GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
            using(GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius-1F))
            using(Pen penSurface = new(this.Parent.BackColor, 2))
            using(Pen penBorder = new(borderColor, borderSize))
            {
                penBorder.Alignment = PenAlignment.Inset;
                this.Region = new(pathSurface);

                pevent.Graphics.DrawPath(penSurface, pathSurface);

                if(borderSize >= 1)
                {
                    pevent.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
        }
        else
        {
            this.Region = new Region(rectSurface);
            if(borderSize >= 1)
            {
                using(Pen penBorder = new(borderColor, borderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width-1, this.Height-1);
                }
            }
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
    }

    private void Container_BackColorChanged(object sender, EventArgs e)
    {
        if(this.DesignMode)
            this.Invalidate();
    }
}
public class ImageButton : Button
{
    Image DefaultImage;
    Image ClickedImage;
    Size originalSize;
    Point originalLocation;

    public ImageButton(string ImagePath)
    {
        this.BackgroundImageLayout = ImageLayout.None;
        originalLocation = Location;
        originalSize = Size;
        DefaultImage = Image.FromFile(ImagePath);
        string[] clickedIStringParts = ImagePath.Split('.');
        string tmp = clickedIStringParts[0] + "_Clicked." + clickedIStringParts[1]; 
        ClickedImage = Image.FromFile(tmp);
        BackgroundImage = DefaultImage;
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        Size = originalSize;
        Location = originalLocation;
        BackgroundImage = DefaultImage;
    }
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        originalSize = Size;
        originalLocation = Location;
        Size = new Size((int)(Size.Width * .95f), (int)(Size.Height * .95f));
        Location = new Point((int)(Location.X + Size.Width * .05f * .5f),(int)(Location.Y + Size.Height * .05f * .5f));
        BackgroundImage = ClickedImage;
    }
}

public class DoubleSquareRadioButton : Control
{   
    public Image L, R;
    public event Action<object, EventArgs> OnChange;
    public bool LeftSelected = false;
    public bool hasSelected = false;

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Size NotSelectedSize = new Size((int)(Size.Width/2.0*.8), (int)(Size.Width/2.0*.8));

        RectangleF Left = new(Location.X, Location.Y, (int)(Size.Width/2.0), Size.Height);
        RectangleF Right = new(Location.X + ((int)(Size.Width/2.0)), Location.Y, (int)(Size.Width/2.0), Size.Height);

        using(SolidBrush NotSelected = new(Color.Gray))
        using(SolidBrush Selected = new(Color.Tomato))
        {
            if(!hasSelected)
            {
                pevent.Graphics.FillRectangle(NotSelected, Left);
                pevent.Graphics.FillRectangle(NotSelected, Right);
            }
            else if(LeftSelected)
            {
                Right = new(new(Location.X + ((int)(Size.Width/2.0)), Location.Y + (int)(Size.Height*.25), NotSelectedSize.Width, NotSelectedSize.Height));
                pevent.Graphics.FillRectangle(Selected, Left);
                pevent.Graphics.FillRectangle(NotSelected, Right);
            }
            else
            {
                Left = new(new(Location.X + (int)(Size.Width/2.0*.2) , Location.Y + (int)(Size.Height*.25), NotSelectedSize.Width, NotSelectedSize.Height));
                pevent.Graphics.FillRectangle(Selected, Right);
                pevent.Graphics.FillRectangle(NotSelected, Left);
            }

            pevent.Graphics.DrawImage(L, Left);
            pevent.Graphics.DrawImage(R, Right);
        }

    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        hasSelected = true;
        LeftSelected = (e as MouseEventArgs).X < Size.Width / 2.0? true : false;

        OnChange?.Invoke(this, e);
        this.Invalidate();
        this.Refresh();

    }
}

public class RadioButtonRect : RadioButton
{
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        if(Checked)
        {
            pevent.Graphics.FillRectangle(Brushes.Tomato, ClientRectangle);
        }
        else
        {
            pevent.Graphics.FillRectangle(Brushes.LightGray, ClientRectangle);
        }

        TextRenderer.DrawText(pevent.Graphics, (Checked ? "Φ " : "O ") + Text, Font, new Point(0, 0), ForeColor);
    }
}
public class OrderEntry : Panel
{
    Image icon;
    Label mainText;
    public ImageButton trashButton;
    bool[] order;
    public double orderPrice;
    public bool CurSelected = false;
    public int index;
    public OrderEntry(bool[] order, string[] miscNames = null)
    {
        this.order = order;
        Size = new Size(600, 80);
        orderPrice = 0;
        if(order[0])
            orderPrice+=12.99;
        else
            orderPrice+=1.99;
        
        string tmp = "";
        for(int i = 1; i < order.Length; ++i)
        {
            if(order[i] == true)
            {
                tmp = miscNames[i-1];
                break;
            }

        }
        mainText = new()
        {
            Location = new Point(10, 10),
            Text = order[0] ? "Pizza" : tmp,
            TextAlign = ContentAlignment.MiddleLeft,
            Size = new Size(200, 60),
            Font = new Font(new FontFamily("Microsoft Sans Serif"), 22.2f),
            BackColor = Color.Transparent
        };
        this.Controls.Add(mainText);

        trashButton = new("Resources\\Trash.png")
        {
            Location = new Point(535, 0),
            Size = new Size(65, 80),
            BackColor = Color.Tomato
        };
        this.Controls.Add(trashButton);

        if(order[0])
        {
            //show all attributes to pizza
            if(order[1])
            {
                orderPrice+=2.00;
                Label stuffed = new Label() {Text = "Stuffed", Location = new Point(100, 25), Size = new Size(100, 30), Font = new Font(new FontFamily("Microsoft Sans Serif"), 16.2f), TextAlign=ContentAlignment.MiddleCenter, BackColor = Color.Tomato};
                this.Controls.Add(stuffed);
                this.Controls[this.Controls.Count-1].BringToFront();
            }
            int index = 0;
            for(int i = 2; i < order.Length; ++i)
            {
                if(order[i])
                {
                    orderPrice+=1.00;
                    this.Controls.Add(new Label() {Text = miscNames[i-2], Location = new Point(230 + index/3 * 80, 10+index++%3*20), Size = new Size(80, 20), BackColor = Color.Transparent});
                }
            }
        }


        //this.Controls.Add(new Label() {Text="$"+orderPrice});
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        using(SolidBrush b = new(CurSelected ? Color.Tomato : Color.LightGray))
        using(Pen p = new(Color.Black, 5))
        {
            pevent.Graphics.FillRectangle(b, ClientRectangle);
            pevent.Graphics.DrawRectangle(p, ClientRectangle);
        }
    }



}

public class ToggleButton : Control
{
    public bool Checked;

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        using(SolidBrush b = new(Checked ? Color.Tomato : Color.Gray))
        using(Pen p = new(Color.DarkGray))
        {
            pevent.Graphics.FillRectangle(b, ClientRectangle);
            pevent.Graphics.DrawRectangle(p, ClientRectangle);
            TextRenderer.DrawText(pevent.Graphics, Text, Font, new Point(0, 0), ForeColor);
        }
    }

    protected override void OnClick(EventArgs e)
    {
        Checked = !Checked;
        this.Invalidate();
    }
}

#endregion