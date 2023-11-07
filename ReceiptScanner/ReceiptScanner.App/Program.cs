
using ReceiptScanner.Domain;
using ReceiptScanner.Helpers;


ApiHelper.InitializeClient();
ReceiptProcessor receiptProcessor = new ReceiptProcessor();


try
{
	await receiptProcessor.GenerateReceipt();

}
catch (Exception ex)
{
	Console.WriteLine($"Error: {ex.Message}");
	
}


