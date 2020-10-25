using HW4MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace HW4MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
            
        }
        [HttpGet]
        public IActionResult RGBColor()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult RGBColor(ClsRGB rgb)
        {
            Color myColor = Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue);
            string hex = myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
            ViewBag.Color = hex;
            return View();
        }

        
        public IActionResult ColorInterpolator()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult ColorInterpolator(ColorInterpolation color)
        {
            ColorInterpolation output = new ColorInterpolation();
            List<string> colorList = new List<string>();
            string firstColorHex = color.FirstColor;
            string secondColorHex = color.SecondColor;
            int steps = color.NumberOfColors;
            Color firstColorRGB = System.Drawing.ColorTranslator.FromHtml(firstColorHex);
            Color secondColorRGB  = System.Drawing.ColorTranslator.FromHtml(secondColorHex);
            ColorToHSV(firstColorRGB, out double hue1, out double saturation1, out double value1);
            ColorToHSV(secondColorRGB, out double hue2, out double saturation2, out double value2);
            double dist = Math.Abs(hue1 - hue2);
            int count = 0;
            if(steps > 0)
            {
                count = (int)dist / steps;
            }
            colorList.Add(firstColorHex);
            for(int i = 0;i < steps;i++)
            {
                hue1 += count;
                Color color1 = ColorFromHSV(hue1, saturation1, value1);
                string hex = color1.R.ToString("X2") + color1.G.ToString("X2") + color1.B.ToString("X2");
                colorList.Add("#" + hex);
            }
            colorList.Add(secondColorHex);
            //ViewBag.ColorList = colorList;
            output.ColorList = colorList;
            return View(output);
        }

        


        /*string hex = "#FFFFFF";
        Color _color = System.Drawing.ColorTranslator.FromHtml(hex);*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
