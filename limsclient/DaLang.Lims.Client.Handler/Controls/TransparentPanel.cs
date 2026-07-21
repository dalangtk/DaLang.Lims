namespace DaLang.Lims.Client.Handler.Controls;

public class TransparentPanel : Panel
{
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
            return cp;
        }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }

    public TransparentPanel()
    {
        this.BackColor = Color.FromArgb(125, Color.White);
    }
}
