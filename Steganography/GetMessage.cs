using System;
using System.IO;
using System.Text;
using CommandLine;
using CommandLine.Text;

class Program
{
    class Options
    {
        [Option('m', "message", Required = false, HelpText = "Path to a file containing the message. If not specified, the message is read from the standard input.")]
        public string MessagePath { get; set; }

        [Option('s', "stego", Required = false, HelpText = "Path to write the steganographic container. If not specified, the result is output to the standard output.")]
        public string StegoPath { get; set; }

        [Option('c', "container", Required = true, HelpText = "Path to the container file. This is a required parameter!")]
        public string ContainerPath { get; set; }

        [Option('h', "help", HelpText = "Display help on how to use the program and what it does.")]
        public bool ShowHelp { get; set; }
    }

    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(options =>
            {
                if (options.ShowHelp)
                {
                    Console.WriteLine("");
                    return;
                }

                if (!string.IsNullOrEmpty(options.MessagePath) && !string.IsNullOrEmpty(options.ContainerPath))
                {
                    string message = options.MessagePath != null ? File.ReadAllText(options.MessagePath) : Console.In.ReadToEnd();
                    string container = File.ReadAllText(options.ContainerPath);
                    string steganography = EmbedMessage(message, container);

                    if (!string.IsNullOrEmpty(options.StegoPath))
                    {
                        File.WriteAllText(options.StegoPath, steganography);
                        Console.WriteLine("Message successfully embedded in the container and saved.");
                    }
                    else
                    {
                        Console.WriteLine(steganography);
                    }
                }
                else if (!string.IsNullOrEmpty(options.StegoPath))
                {
                    string steganography = options.StegoPath != null ? File.ReadAllText(options.StegoPath) : Console.In.ReadToEnd();
                    string extractedMessage = ExtractMessage(steganography);

                    if (!string.IsNullOrEmpty(options.MessagePath))
                    {
                        File.WriteAllText(options.MessagePath, extractedMessage);
                        Console.WriteLine("Message successfully extracted and saved.");
                    }
                    else
                    {
                        Console.WriteLine(extractedMessage);
                    }
                }
                else
                {
                    Console.WriteLine("You need to specify parameters for encryption or decryption. Use the -h parameter to get help.");
                }
            });
    }

    static string ExtractMessage(string steganography)
    {
        if (!steganography.Contains(" "))
        {
            Console.WriteLine("File structure does not match expectations.");
            return string.Empty;
        }

        StringBuilder binaryMessage = new StringBuilder();
        int startIndex = steganography.IndexOf(" ") + 1;
        int endIndex = steganography.IndexOf(" ", startIndex);
        string binaryPart = steganography.Substring(startIndex, endIndex - startIndex);
        binaryPart = binaryPart.Replace(" ", "");
        for (int i = 0; i < binaryPart.Length; i += 8)
        {
            string byteStr = binaryPart.Substring(i, 8);
            binaryMessage.Append(Convert.ToByte(byteStr, 2).ToString());
        }
        string extractedMessage = Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(binaryMessage.ToString()));

        return extractedMessage;
    }

    static string EmbedMessage(string message, string container)
    {
        StringBuilder steganography = new StringBuilder(container);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        StringBuilder binaryMessage = new StringBuilder();
        foreach (byte b in messageBytes)
        {
            binaryMessage.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }
        steganography.Insert(0, " ");
        steganography.Append(" ");
        steganography.Append(binaryMessage.ToString());
        steganography.Append(" ");
        return steganography.ToString();
    }
}
