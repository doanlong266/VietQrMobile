using Controls.UserDialogs.Maui;
using Microsoft.Extensions.Logging;
using zoft.MauiExtensions.Controls;

namespace VietQrMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                  .UseMauiApp<App>()
                  .UseZoftAutoCompleteEntry()
                  .UseUserDialogs(() =>
                  {
                      AlertConfig.DefaultBackgroundColor = Colors.White;
#if ANDROID
                    AlertConfig.DefaultMessageFontFamily = "Nunito-Regular.ttf";
#else
                      AlertConfig.DefaultMessageFontFamily = "Nunito-Regular";
#endif
                      ToastConfig.DefaultCornerRadius = 15;
                  })
                 .ConfigureFonts(fonts =>
                 {
                     fonts.AddFont("fa-duotone-900.ttf", "FAD");
                     fonts.AddFont("fa-light-300.ttf", "FAL");
                     fonts.AddFont("fa-thin-100.ttf", "FAT");
                     fonts.AddFont("fa-brands.ttf", "FAB");
                     fonts.AddFont("fa-regular.ttf", "FAR");
                     fonts.AddFont("fa-solid.ttf", "FAS");
                     fonts.AddFont("Nunito-Black.ttf", "NBL");
                     fonts.AddFont("Nunito-BlackItalic.ttf", "NBI");
                     fonts.AddFont("Nunito-Bold.ttf", "NB");
                     fonts.AddFont("Nunito-BoldItalic.ttf", "NBI");
                     fonts.AddFont("Nunito-ExtraBold.ttf", "NEB");
                     fonts.AddFont("Nunito-ExtraBoldItalic.ttf", "REBI");
                     fonts.AddFont("Nunito-ExtraLight.ttf", "NEL");
                     fonts.AddFont("Nunito-ExtraLightItalic.ttf", "NELI");
                     fonts.AddFont("Nunito-Italic.ttf", "NI");
                     fonts.AddFont("Nunito-Light.ttf", "NL");
                     fonts.AddFont("Nunito-LightItalic.ttf", "NLI");
                     fonts.AddFont("Nunito-Medium.ttf", "NM");
                     fonts.AddFont("Nunito-MediumItalic.ttf", "NMI");
                     fonts.AddFont("Nunito-Regular.ttf", "NR");
                     fonts.AddFont("Nunito-SemiBold.ttf", "NSB");
                     fonts.AddFont("Nunito-SemiBoldItalic.ttf", "NSBI");
                 });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
