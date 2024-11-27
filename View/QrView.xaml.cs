namespace VietQrMobile.View;

public partial class QrView : ContentPage
{
	public QrView(string QrDataURL)
	{
		InitializeComponent();
        QR.Source = QrDataURL;

    }
}