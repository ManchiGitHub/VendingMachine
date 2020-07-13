namespace CMDInterface
{
    using System;

    public class UIInterface
    {
        public static void PrintToCMD(string i_Message)
        {
            Console.WriteLine(i_Message);
        }

        public static string GetInputFromUser()
        {
            string input = string.Empty;

            input = Console.ReadLine();

            return input;
        }
    }
}
