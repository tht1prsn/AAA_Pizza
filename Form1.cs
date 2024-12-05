using System;
using System.IO;
namespace AAA_Pizza;

public partial class Form1 : Form
{
    public Form1()
    {
        FontSmall = new Font(new FontFamily("Microsoft Sans Serif"), 22.2f);
        FontLarge = new Font(new FontFamily("Microsoft Sans Serif"), 28);
        FormSize = new Vector2(1.0/3.0*1920, 1000);
        InitializeComponent();
    }

    public void ExitApp(Object sender, EventArgs e)
    {
        Environment.Exit(0);
    }

    
    public string GetRandomTextFromFile(DialogueFileIndex fi, float rarity)
    {
        rarity = rarity/10.0f > 1 ? 1.0f : rarity / 10.0f;
        string mainLine = "";
        try
        {
            //if(rarity > 1 || rarity < 0)
                ///throw new ApplicationException("Rarity value greater than or less than range 0 >= x <= 1");
        
            string[] lines = File.ReadAllLines("Resources\\Dialogues.meta");
            foreach(string s in lines)
            {
                string[] splitString = s.Split(':');
                if(Convert.ToInt16(splitString[1]) == (int)(fi))
                {
                    using(StreamReader sr = new($"Resources\\Dialogues\\{splitString[0]}.txt"))
                    {
                        Random r = new();
                        int randVal = r.Next(rarity > 0 ? 11 : 0, rarity > 0 ? (int)((Convert.ToInt16(splitString[2]) - 10) * rarity) + 11:10);
                        int index = 0;
                        string tmp;
                        while((tmp = sr.ReadLine()) != null)
                        {
                            if(index == randVal)
                            {
                                mainLine = tmp;
                                return mainLine;
                            }
                            ++index;
                        }
                    }
                }
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
        
        return mainLine;
    }

    public void DesignerPanelCancelButton_Click(Object sender, EventArgs e)
    {
        pd.Visible = false;
        ClearDesigner();
        PizzaOrDrinkChoice.hasSelected = false;
        DesignerTopLabel.Text = GetRandomTextFromFile(DialogueFileIndex.DesignerLabelDialogue, OrderEntries.Count);
        AddButtonLabel.Text = GetRandomTextFromFile(DialogueFileIndex.AddButtonDialogue, OrderEntries.Count);

    }
    public void AddButton_Click(object sender, EventArgs e)
    {
        pd.Visible = true;
    }
    public void Desginer_AddEntry_Click(Object sender, EventArgs e)
    {
        if(DesignerAddEntryButton.BackColor == Color.Green)
        {
            pd.Visible = false;
            //add order entry to list
            //OrderEntries.Add(new OrderEntry(new bool[] {true}));
            if(PizzaOrDrinkChoice.LeftSelected)
            {
                //Pizza selected, Pizza = 0
                bool[] order = new bool[Toppings.Length+2];
                order[0] = true;
                int index = 2;
                foreach(Control c in DesignerGroupBoxPizza.Controls)
                {
                    try
                    {
                        if(c is ToggleButton)
                            order[1] = (c as ToggleButton).Checked;
                        if(c is CheckBox)
                            order[index++] = (c as CheckBox).Checked;
                    }
                    catch(Exception) {}
                }
                OrderEntries.Add(new OrderEntry(order, Toppings) {index=OrderEntries.Count});
                OrderEntries.ElementAt(OrderEntries.Count-1).trashButton.Click+=OrderTrashButton_Click;
                OrderEntries.ElementAt(OrderEntries.Count-1).trashButton.Parent = OrderEntries.ElementAt(OrderEntries.Count-1);
                
            }
            else
            {
                bool[] order = new bool[Drinks.Length+1];
                order[0] = false;
                int index = 1;
                for(int i = 0; i < DesignerGroupBoxDrink.Controls.Count; ++i)
                {
                    try
                    {
                        if(DesignerGroupBoxDrink.Controls[i] is RadioButtonRect)
                        {
                            order[index] = ((DesignerGroupBoxDrink.Controls[i]) as RadioButtonRect).Checked;
                            index++;
                        }
                    }
                    catch(Exception) {}
                }
                OrderEntries.Add(new OrderEntry(order, Drinks) {index=OrderEntries.Count});
                OrderEntries.ElementAt(OrderEntries.Count-1).trashButton.Click+=OrderTrashButton_Click;
                OrderEntries.ElementAt(OrderEntries.Count-1).trashButton.Parent = OrderEntries.ElementAt(OrderEntries.Count-1);
            }
            AddButtonLabel.Text = GetRandomTextFromFile(DialogueFileIndex.AddButtonDialogue, OrderEntries.Count);
            DesignerTopLabel.Text = GetRandomTextFromFile(DialogueFileIndex.DesignerLabelDialogue, OrderEntries.Count);


            ClearDesigner();
            PizzaOrDrinkChoice.hasSelected = false;
        }
        UpdateOrders();
    }

    void PizzaOrDrinkRadioButton_OnChange(object sender, EventArgs e)
    {
        
        
        ClearDesigner();
        if((sender as DoubleSquareRadioButton).LeftSelected)
        {
            DesignerGroupBoxPizza.Visible = true;
            DesignerGroupBoxPizza.Invalidate();
            DesignerGroupBoxDrink.Visible = false;
        }
        else
        {
            DesignerGroupBoxDrink.Visible = true;
            DesignerGroupBoxDrink.Invalidate();
            DesignerGroupBoxPizza.Visible = false;
        }

        if(PizzaOrDrinkChoice.LeftSelected)
            DesignerAddEntryButton.BackColor = Color.Green;
    }

    void DrinkChoiceSelectionChanges(object sender, EventArgs e)
    {
        DesignerAddEntryButton.BackColor = Color.Green;
    }

    void ClearDesigner()
    {
        foreach(Control c in DesignerGroupBoxDrink.Controls)
        {
            try
            {
                (c as RadioButtonRect).Checked = false;
            }
            catch(Exception) {}
        }
        foreach(Control c in DesignerGroupBoxPizza.Controls)
        {
            try
            {
                if(c is CheckBox)
                    (c as CheckBox).Checked = false;
                else if(c is ToggleButton)
                    (c as ToggleButton).Checked = false;
            } catch(Exception) {}
        }
        DesignerAddEntryButton.BackColor=Color.Gray;
        

        DesignerGroupBoxDrink.Visible = false;
        DesignerGroupBoxPizza.Visible = false;
    }

    public enum DialogueFileIndex
    {
        AddButtonDialogue = 1,
        DesignerLabelDialogue = 2
    }

    void UpdateOrders()
    {
        if(OrderPanel.Controls.Count != OrderEntries.Count)
        {

            OrderPanel.SuspendLayout();
            foreach(Control c in OrderPanel.Controls)
            {
                OrderPanel.Controls.Remove(c);
            }
            foreach(OrderEntry o in OrderEntries)
            {
                OrderPanel.Controls.Add(o);
                o.Click+=OrderEntry_Click;
            }
            OrderPanel.ResumeLayout();
        }

        if(OrderEntries.Count > 0 && DeliveryOrPickup.hasSelected)
        {
            checkoutButton.BackColor = Color.MediumSlateBlue;
            checkoutButton.Invalidate();
        }
        else
        {
            checkoutButton.BackColor = Color.Gray;
            checkoutButton.Invalidate();
        }

        OrderPanel.Invalidate();
    }

    void OrderEntry_Click(object sender, EventArgs e)
    {
        try
        {
            throw new ApplicationException("Feature no longer needed");
            foreach(Control c in OrderPanel.Controls)
            {
                (c as OrderEntry).CurSelected = false;
            }
            (sender as OrderEntry).CurSelected = true;
            OrderPanel.Invalidate();
            OrderPanel.Refresh();
        }
        catch(Exception) {}
    }

    void OrderPanelScroll(Object sender, EventArgs e)
    {
        OrderPanel.Invalidate();
        OrderPanel.Refresh();
    }

    void OrderTrashButton_Click(object sender, EventArgs e)
    {

        ((sender as Control).Parent as Control).Dispose();
        OrderPanel.Controls.Remove(((sender as Control).Parent as Control));
        OrderEntries.Remove((sender as Control).Parent as OrderEntry);
        UpdateOrders();
    }

    void DorPU_OnChange(object sender, EventArgs e)
    {
        UpdateOrders();
    }

    void CheckoutButton_Click(object sender, EventArgs e)
    {
    }


}
