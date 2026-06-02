using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CSharp_Garage_Task
{
    internal class Helper
    {
        public static void WriteErrorMessage(string errorText)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorText);
            Console.ResetColor();
        }

        public static void WriteMessage(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void WriteMessage(string text)
        {
            WriteMessage(text, ConsoleColor.White);
        }

        public static bool TryGetIntFromAvalonia(string input, int min, int max, out int result)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(input))
            {
                WriteErrorMessage("Invalid input, try again");
                return false;
            }

            if (!int.TryParse(input, out result))
            {
                WriteErrorMessage("Not an int, try again");
                return false;
            }

            if (result < min || result > max)
            {
                WriteErrorMessage("Needs to fit within the specified options");
                return false;
            }

            return true;
        }

        public static bool TryGetStringFromAvalonia(string input, out string result)
        {
            result = "";
            if (string.IsNullOrWhiteSpace(input))
            {
                WriteErrorMessage("Invalid input, try again");
                return false;
            }
            result = input;
            return true;
        }

        internal static void WriteHorizontalLine()
        {
            WriteMessage("-----------------------------------", ConsoleColor.White);
        }

        internal static string WriteSpaces(int amount)
        {
            return (amount == 1 ? "Space " : "Spaces ");
        }
    }
}
