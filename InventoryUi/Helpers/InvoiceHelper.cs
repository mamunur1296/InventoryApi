namespace InventoryUi.Helpers
{
    public static class InvoiceHelper
    {
        public static string GenerateInvoiceNumber()
        {
            // Generate a random 6-digit number
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999); // Generates a 6-digit number

            // Get the current date and time (Format: yyyyMMddHHmmss)
            string datePart = DateTime.Now.ToString("yyMMddHHmm");

            // Concatenate the random number with the date part
            string invoiceNumber = $"{datePart}";

            return invoiceNumber;
        }
    }
}
