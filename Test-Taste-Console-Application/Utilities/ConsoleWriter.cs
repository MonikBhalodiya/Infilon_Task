using System.Text;

namespace Test_Taste_Console_Application.Utilities
{
    public static class ConsoleWriter
    {
        /// <summary>
        /// Writes a separator line using the supplied column widths.
        /// </summary>
        /// <param name="columnSizes">The width of each output column.</param>
        public static void CreateLine(int[] columnSizes)
        {
            /*
             This is an example of what the function can create:
             --------------------+--------------------+--------------------
            */
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < columnSizes.Length; i++)
            {
                stringBuilder.Append(string.Concat(Enumerable.Repeat('-', columnSizes[i])));
                if (i != columnSizes.Length - 1)
                    stringBuilder.Append('+');
            }

            Console.WriteLine(stringBuilder.ToString());
            stringBuilder.Clear();
        }

        /// <summary>
        /// Writes one aligned row of text using the supplied column widths.
        /// </summary>
        /// <param name="columnLabels">Values to write in the row.</param>
        /// <param name="columnSizes">The width of each output column.</param>
        public static void CreateText(string[] columnLabels, int[] columnSizes)
        {
            /*
             This is an example of what the function can create:
             Moon's Number       |Moon's Id           |Moon's Average Temperature
            */
            if (!columnLabels.Length.Equals(columnSizes.Length))
                return;

            var result = string.Empty;
            var currentPosition = 0;
            for (var i = 0; i < columnLabels.Length; i++)
            {
                currentPosition += columnSizes[i];
                result += columnLabels[i];
                if (i != columnLabels.Length - 1)
                {
                    result = result.PadRight(currentPosition + i);
                    result += '|';
                }
            }

            Console.WriteLine(result);
        }

        /// <summary>
        /// Writes a table header surrounded by separator lines.
        /// </summary>
        /// <param name="columnLabels">Header labels to write.</param>
        /// <param name="columnSizes">The width of each output column.</param>
        public static void CreateHeader(string[] columnLabels, int[] columnSizes)
        {
            /*
             This is an example of what the function can create:
             --------------------+--------------------+------------------------------
             Moon's Number       |Moon's Id           |Moon's Average Temperature
             --------------------+--------------------+------------------------------
            */
            CreateLine(columnSizes);
            CreateText(columnLabels, columnSizes);
            CreateLine(columnSizes);
        }

        /// <summary>
        /// Writes the requested number of blank console lines.
        /// </summary>
        /// <param name="totalEmptyLines">Number of blank lines to write.</param>
        public static void CreateEmptyLines(int totalEmptyLines)
        {
            //The function can create empty spaces.
            for (var i = 0; i < totalEmptyLines; i++)
            {
                Console.WriteLine();
            }
        }
    }
}